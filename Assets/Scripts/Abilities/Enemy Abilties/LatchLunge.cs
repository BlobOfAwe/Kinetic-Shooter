using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LatchLunge : Ability
{
    public LayerMask canDamage = 8; // Objects on these layers are valid attack targets
    [SerializeField] private float startup; // How long does the enemy indicate before it lunges?
    [SerializeField] private float startupSpeed; // While starting up, the entity will back up. How fast does it do this?
    [SerializeField] private float lungeForce; // How far does the entity lunge?
    [SerializeField] private float endtime; // After lunging, how long (in seconds) before the entity can move again
    [SerializeField] private bool latched;
    [SerializeField] private float stunDuration;

    private Enemy thisEnemy;
    private PlayerBehaviour player;
    private Vector2 lockRelativePosition;

    private Rigidbody2D rb;
    private BoxCollider2D hitbox;
    private bool lunging;

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<Enemy>().sprite;
        baseColor = sprite.color;
        thisEnemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (latched)
        {
            available = false;
            thisEnemy.aiPath.canMove = false;
            player.Damage(thisEntity.totalAttack * damageModifier * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        if (latched)
        {
            transform.localPosition = lockRelativePosition;
            sprite.color = Color.blue;
        }
    }

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        StartCoroutine(LungeForward());
    }

    private IEnumerator LungeForward()
    {
        // Stop the entity from moving using A* Pathfinding
        try { GetComponent<AIPath>().canMove = false; }
        catch { Debug.LogWarning("No AIPath component detected."); }

        sprite.color = Color.gray; // WHITEBOX ONLY: Change the color to indicate the lunge is starting

        // Make the entity back up slowly for startup seconds to telegraph the lunge
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * startupSpeed;
        yield return new WaitForSeconds(startup);

        // Reset velocity and apply a powerful force forward to lunge
        rb.velocity = Vector2.zero;
        lunging = true;
        rb.AddForce(transform.up * lungeForce, ForceMode2D.Force);

        // Wait for endTime seconds before ending the lunge
        yield return new WaitForSeconds(endtime * (100 / (100 + thisEntity.totalSpeed)));
        lunging = false;

        sprite.color = baseColor;

        // Re-enable A* Pathfinding movement
        if (!latched) { GetComponent<AIPath>().canMove = true; }
    }

    IEnumerator Dazed()
    {
        thisEnemy.aiPath.canMove = false;
        available = false;
        yield return new WaitForSeconds(stunDuration);
        thisEnemy.aiPath.canMove = true;
        StartCoroutine(BeginCooldown());
    }

    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // The entity should be on layer "OnlyCollideWithObstacle" when latched, so this should only run when hitting a wall
        if (latched)
        {
            StopAllCoroutines();

            sprite.color = baseColor;
            lunging = false;

            // Set the object layer to the standard enemy layer
            int layer = LayerMask.NameToLayer("Enemy");
            gameObject.layer = layer;

            transform.parent = null;
            latched = false;
            StartCoroutine(Dazed());
        }

        else if (lunging)
        {
            if ((canDamage & (1 << collision.gameObject.layer)) != 0)
            {
                // Latch onto the player
                latched = true;
                Debug.Log("Latched");

                // set the parent
                transform.parent = collision.transform;
                
                // set the position
                player = transform.parent.GetComponent<PlayerBehaviour>();
                thisEnemy.aiPath.canMove = false;
                lockRelativePosition = transform.localPosition;
                
                // Set the object layer to only collide with obstacles while latched
                int layer = LayerMask.NameToLayer("OnlyCollideWithObstacle");
                gameObject.layer = layer;

                rb.velocity = Vector2.zero;

                lunging = false;
            }
        }

        
    }
}
