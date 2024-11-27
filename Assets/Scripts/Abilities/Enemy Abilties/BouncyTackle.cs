using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyTackle : Ability
{
    public LayerMask canDamage = 8; // Objects on these layers are valid attack targets
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private int shockwavesAmount;
    [SerializeField] private float shockwaveSpread; // The spread of all the shockwaves, in angles, on either side of the collision normal
    [SerializeField] private float startup; // How long does the enemy indicate before it lunges?
    [SerializeField] private float bounceDelay; // How long does the enemy pause between bounces?
    [SerializeField] private float lungeForce; // How far does the entity lunge?
    [SerializeField] private float endtime; // After lunging, how long (in seconds) before the entity can move again
    [SerializeField] private int maxBounces; // The maximum number of times it can bounce
    private int bounceCounter;

    private Rigidbody2D rb;
    private BoxCollider2D hitbox;
    private bool lunging;
    [SerializeField] private GameObject[] shockwaves;
    
    

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;


    private void Awake()
    {
        // Create the shockwave objects
        shockwaves = new GameObject[shockwavesAmount];
        for (int i = 0; i < shockwavesAmount; i++)
        {
            shockwaves[i] = Instantiate(shockwavePrefab);
            shockwaves[i].GetComponent<Projectile>().shooter = gameObject;
            shockwaves[i].SetActive(false);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        entity = GetComponent<Entity>();
        baseColor = sprite.color;
        Physics2D.queriesStartInColliders = false; // Raycasts that start inside a collider should not register that collider
    }

    public override void OnActivate()
    {
        available = false;
        inUse = true;
        StartCoroutine(BeginLunge(transform.up));
    }

    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lunging)
        {
            if ((canDamage & (1 << collision.gameObject.layer)) != 0)
            {
                // Try to damage the object
                try
                {
                    collision.gameObject.GetComponent<Entity>().Damage(entity.totalAttack * damageModifier);
                }
                catch { }
            }
            

            // Fetch the object's rigidbody
            Rigidbody2D collidedRB = collision.gameObject.gameObject.GetComponent<Rigidbody2D>();

            // If the object has no rigidbody or the rigidbody is set to kinematic, get a reflection angle and prepare to bounce
            if (collidedRB == null || collidedRB.isKinematic)
            {
                Debug.Log(bounceCounter + " bounces");

                if (bounceCounter > 0) { CalculateBounce(collision); }

                else
                {
                    CrashShockwave(collision);

                    lunging = false;
                    sprite.color = baseColor;
                    StartCoroutine(BeginCooldown());
                    // Re-enable A* Pathfinding movement
                    try { GetComponent<AIPath>().canMove = true; }
                    catch { }
                    inUse = false;
                }
            }
        }
    }

    // Based on the angle of collision, get a vector that reflects around the collision normal for the next bounce
    private void CalculateBounce(Collision2D collision)
    {
        bounceCounter--;

        var contactPoint = collision.GetContact(0);
        // Find the line from the center of the object to the point that was clicked.
        Vector3 incomingVec = contactPoint.point - (Vector2)transform.position;

        // Draw lines representing the angle of approach and the normal of the collider
        Debug.DrawRay(transform.position, contactPoint.point - (Vector2)transform.position, Color.magenta, 10f);
        Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.green, 10f);

        // Use the point's normal to calculate the reflection vector.
        Vector3 reflectVec = Vector3.Reflect(incomingVec, contactPoint.normal);
        Debug.DrawRay(contactPoint.point, reflectVec, Color.cyan, 10f);

        StartCoroutine(LungeForward(reflectVec));
    }

    private void CrashShockwave(Collision2D collision) 
    {
        var contactPoint = collision.GetContact(0);
        float angleFromNormal;

        for (int i = 0; i < shockwaves.Length; i++)
        {
            angleFromNormal = (shockwaveSpread * 2 * i) / shockwavesAmount - shockwaveSpread;
            shockwaves[i].transform.position = transform.position;
            shockwaves[i].transform.up = contactPoint.normal;
            shockwaves[i].transform.Rotate(Vector3.forward * angleFromNormal);
            shockwaves[i].GetComponent<Projectile>().timeRemaining = shockwaves[i].GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
            shockwaves[i].SetActive(true);
        }
    }

    private IEnumerator BeginLunge(Vector2 direction)
    {
        bounceCounter = Random.Range(maxBounces, 0); // Sets a number between 1 and maxBounces inclusive

        // Stop the entity from moving using A* Pathfinding
        try { GetComponent<AIPath>().canMove = false; }
        catch { Debug.LogWarning("No AIPath component detected."); }

        sprite.color = Color.gray; // WHITEBOX ONLY: Change the color to indicate the lunge is starting

        transform.up = direction;

        yield return new WaitForSeconds(startup);

        StartCoroutine(LungeForward(direction));
    }

    private IEnumerator LungeForward(Vector2 direction)
    {
        if (bounceCounter == 0) { sprite.color = Color.red; } // WHITEBOX ONLY: Change the color to indicate the lunge is starting

        if (bounceDelay > 0) { yield return new WaitForSeconds(bounceDelay); } // Pause for bounceDelay if there is one
        
        transform.up = direction;

        // Reset velocity and apply a powerful force forward to lunge
        rb.velocity = Vector2.zero;
        lunging = true;
        rb.velocity = transform.up * lungeForce;

        yield return new WaitForEndOfFrame();
        
    }
}
