// ## - JV
using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
[RequireComponent(typeof (AIPath))]

// Class <Enemy> now inherits directly from the Abstract Entity class
public abstract class Enemy : Entity
{
    [Header("Spawning")]
    public int spawnCost; // How many credits does it cost to spawn this enemy

    [Header("Component References")]
    public Transform target;
    public Seeker seeker;
    public AIPath aiPath;
    public SpriteRenderer sprite;
    private EnemyCounter enemyCounter; // Added by Nathaniel Klassen
    [SerializeField] private ScoreManager scoreManager; // Added by Nathaniel Klassen
    private SpecialUpgradeSpawner specialUpgradeSpawner; // Added by Nathaniel Klassen

    [Header("Targeting")]
    public LayerMask hostile = 8; // Objects on these layers are valid attack targets
    protected float distanceToTarget; // Distance from the enemy to the identified target
    [SerializeField] protected float sightRange = 20f; // How far away can the enemy see
    [SerializeField] protected float stayDistance = 5f; // How close the enemy will get to the player
    [SerializeField] protected float chaseDistance = 7f; // How far can the player get before the enemy chases them
    [SerializeField] protected float pursuitDuration; // How long the enemy will pursue the player while outside FOV before losing interest
    protected float pursuitTimer; // Used to measure pursuitDuration

    [Header("State Management")]
    // Unless specified otherwise, state 0 is Wandering, state 1 is Pursuing. States beyond that should be written into the derivative class
    public int state;
    private float stateChangeCooldown = 0.5f; // How long after a state change before the state can change again
    private int lastState = -1; // What was the enemy state last frame
    private float stateChangeCooldownTimer; // Used to time stateChangeCooldown

    // I don't know what category to put this in. - NK
    [Header("Other")]
    [SerializeField]
    protected bool isBoss = false; // Added by Nathaniel Klassen
    [SerializeField]
    protected bool isLeader = false; // Added by Nathaniel Klassen
    [SerializeField]
    private string enemyName = ""; // Used to track kills of specific enemy types. - NK
    [SerializeField]
    private int score = 0; // Added by Nathaniel Klassen
    [SerializeField]
    public float enemyCounterValue = 1; // How many enemies are counted as defeated when this enemy dies? Usually 1, but may be 0 in some circumstances
    [SerializeField]
    protected float leaderUpgradeSpawnChance = 0; // 0 is 0%, 1 is 100% - Added by Nathaniel Klassen
    [SerializeField]
    private bool rotateSpriteToFacePlayer = false;
    private bool dead;

    protected virtual void Start()
    {
        seeker = GetComponent<Seeker> ();
        aiPath = GetComponent<AIPath> ();
        enemyCounter = FindAnyObjectByType<EnemyCounter> ();
        scoreManager = FindObjectOfType<ScoreManager> ();
        specialUpgradeSpawner = FindObjectOfType<SpecialUpgradeSpawner>(); // Added by Nathaniel Klassen
        if (sprite == null) { Debug.LogError(gameObject.name + " has no sprite assigned."); }

        attackStat *= 1 + GameManager.difficultyCoefficient * 0.2f;
        hpStat *= 1 + GameManager.difficultyCoefficient * 0.2f;
        speedStat *= 1 + GameManager.difficultyCoefficient * 0.1f;
        recoverStat *= 1 + GameManager.difficultyCoefficient * 0.1f;
    }

    public abstract void DerivativeUpdate(); // Used by derivative classes to contain class specific logic, called by the abstract class Update() every frame
    new private void Update()
    {
        base.Update();
        // As long as a target exists, record how far it is from this object
        if (target) { distanceToTarget = Vector2.Distance(target.transform.position, transform.position); }

        aiPath.maxSpeed = totalSpeed;
        
        DerivativeUpdate(); // Run derived class-specific logic
    }

    protected void LateUpdate()
    {
        if (!rotateSpriteToFacePlayer) { sprite.transform.rotation = Quaternion.Euler(Vector2.zero); }
    }

    // Required implementation of the abstract function Entity.Death()
    public override void Death()
    {
        if (!dead)
        {
            dead = true;
            //Debug.Log(gameObject.name + " was killed");
            FindObjectOfType<PlayerBehaviour>().ProjectileKillEffect(this); // Added to make upgrade effects upon killing enemies happen. - NK

            // Purge any dependant objects from the enemy's abilities
            if (primary != null) { primary.PurgeDependantObjects(); }
            if (secondary != null) { secondary.PurgeDependantObjects(); }
            if (utility != null) { utility.PurgeDependantObjects(); }
            if (additional != null) { additional.PurgeDependantObjects(); }

            if (scoreManager != null)
            {
                scoreManager.AddPoints(score);
            }
            else
            {
                scoreManager = FindAnyObjectByType<ScoreManager>();
                scoreManager.AddPoints(score);
            }
            // Added by Nathaniel Klassen
            if (isLeader)
            {
                if (Random.Range(0f,1f) <= leaderUpgradeSpawnChance)
                {
                    if (specialUpgradeSpawner != null)
                    {
                        specialUpgradeSpawner.SpawnLeaderUpgrade(transform.position);
                    }
                }
            }
            // Added to make the enemy counter count down when an enemy is defeated, unless it's a boss, in which case something else happens. - NK
            if (!isBoss)
            {
                enemyCounter.EnemyDefeated(this);
            }
            else
            {
                // Added by Nathaniel Klassen
                if (specialUpgradeSpawner != null)
                {
                    specialUpgradeSpawner.SpawnBossUpgrade(transform.position);
                }
                Debug.Log("You beat the boss!");
                // Whatever happens when a boss is defeated goes here.
                enemyCounter.BossDefeated(this);
            }
            //Debug.Log(gameObject.name + " SHOULD BE DESTROYED NOW");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.enemyDeath, this.transform.position);
            
            DataManager.Instance.gameData.AddKills(enemyName); // Added by Nathaniel Klassen

            Destroy(gameObject);
        }
    }

    // Checks to see if a valid target is within sightRange
    public void SearchForTarget()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, sightRange, hostile);
        if (targetCollider != null) { target = targetCollider.transform; }
        else { target = null; }
    }

    // If called each frame, checks SearchForTarget() every pursuitDuration seconds
    public void RefreshTarget()
    {
        pursuitTimer += Time.deltaTime;
        if (pursuitTimer > pursuitDuration)
        {
            pursuitTimer = 0;
            SearchForTarget();
        }
    }

    public void FaceTarget()
    {
        aiPath.enableRotation = false;
        Vector2 dir = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        transform.up = dir;
    }

    // Wander aimlessly
    public void Wander()
    {
        aiPath.enableRotation = true;
        if (!aiPath.pathPending && aiPath.reachedEndOfPath || !ReadyToStateChange())
        {
            // Choose a random destination point somewhere within range.
            aiPath.destination = RandomPointWithinRadius(transform, stayDistance);
        }

        aiPath.maxSpeed = 0.6f * totalSpeed;
    }


    // Chase the player at speed
    public void Pursue()
    {
        aiPath.enableRotation = true;
        if (target != null) { aiPath.destination = target.transform.position; }
    }

    // Stop moving towards the player, and move around the player
    public void Strafe()
    {
        if (!aiPath.pathPending && aiPath.reachedEndOfPath || !ReadyToStateChange())
        {
            // Choose a random destination point `stayDistance` distance away from the target.
            aiPath.destination = RandomPointOnEdge(target.transform, stayDistance);
        }
        FaceTarget();
    }

    public IEnumerator Stagger(float duration)
    {
        aiPath.canMove = false;
        yield return new WaitForSeconds(duration);
        aiPath.canMove = true;
    }

    // Stay a fixed distance back from the player
    public void KeepBack()
    {
        float angle = Mathf.Atan2(transform.position.y - target.transform.position.y, transform.position.x - target.transform.position.x);
        Vector2 newPos = new Vector3(Mathf.Cos(angle) * stayDistance, Mathf.Sin(angle) * stayDistance) + target.transform.position;

        if (!aiPath.pathPending && aiPath.reachedEndOfPath || !ReadyToStateChange())
        {
            if (distanceToTarget < stayDistance) { aiPath.destination = newPos; }
        }

        FaceTarget();
    }

    public bool ReadyToStateChange()
    {
        // If the player's state changed last frame OR if the cooldown is already in progress
        if (state != lastState || stateChangeCooldownTimer != 0) 
        {
            // Add to the cooldown and return true if the cooldown is complete, and false otherwise
            lastState = state;
            stateChangeCooldownTimer += Time.deltaTime;
            if (stateChangeCooldownTimer > stateChangeCooldown) 
            { stateChangeCooldownTimer = 0; return true; }
            else { lastState = state; return false; }
        }

        // If the player's state has not changed, and the cooldown is not in progress, then return true 
        else { lastState = state; return true; }

    }

    // Returns a random point within a radius around centerPoint
    public Vector2 RandomPointWithinRadius(Transform centerPoint, float radius)
    {
        var point = Random.insideUnitCircle * radius;

        point += (Vector2)centerPoint.position;
        return point;
    }

    // Returns a random point on the edge of a circle with radius around centerPoint
    public Vector2 RandomPointOnEdge(Transform centerPoint, float radius)
    {
        var point = Random.insideUnitCircle.normalized * radius;

        point += (Vector2)centerPoint.position;
        return point;
    }

    private void OnDrawGizmosSelected()
    {
        // Draws the circle around which the enemy strafes
        Gizmos.color = Color.red;
        try { Gizmos.DrawWireSphere(target.transform.position, stayDistance); } catch { }

        // The range beyond which the enemy pursues the player
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, stayDistance + chaseDistance);
        Gizmos.DrawWireSphere(transform.position, stayDistance);

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); // The sight range of the enemy
        if (target != null) { Gizmos.DrawLine(transform.position, target.transform.position); } // A direct line to the target
    }
}
