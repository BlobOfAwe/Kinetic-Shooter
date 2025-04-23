// ## - JV
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunge : Ability
{
    public LayerMask canDamage = 8; // Objects on these layers are valid attack targets
    [SerializeField] private float startup; // How long does the enemy indicate before it lunges?
    [SerializeField] private float startupSpeed; // While starting up, the entity will back up. How fast does it do this?
    [SerializeField] private float lungeForce; // How far does the entity lunge?
    [SerializeField] private float endtime; // After lunging, how long (in seconds) before the entity can move again

    private Rigidbody2D rb;
    private BoxCollider2D hitbox;
    private bool lunging;
    private AIPath aipath;
    [SerializeField] private GameObject normalEnemy;

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<Enemy>().sprite;
        aipath = GetComponent<AIPath>();
        baseColor = sprite.color;
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
        if (normalEnemy != null)
        {
            Animator animator = normalEnemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isAttacking");
            }
        }
        
        rb.velocity = -transform.up * startupSpeed;
        yield return new WaitForSeconds(startup);
        // Reset velocity and apply a powerful force forward to lunge
        rb.velocity = Vector2.zero;
        lunging = true;
        rb.AddForce(transform.up * lungeForce, ForceMode2D.Force);
        
        // Wait for endTime seconds before ending the lunge
        yield return new WaitForSeconds(endtime*(100 / (100 + thisEntity.totalSpeed)));
        lunging = false;

        sprite.color = baseColor;

        // Re-enable A* Pathfinding movement
        aipath.canMove = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

    }

    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lunging)
        {
            if ((canDamage & (1 << collision.gameObject.layer)) != 0)
            {
                collision.gameObject.GetComponent<Entity>().Damage(thisEntity.totalAttack * damageModifier, true); // 10 should be replaced by the appropriate damage calculation
            }
        }
    }
}
