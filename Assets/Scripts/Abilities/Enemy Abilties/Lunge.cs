// ## - JV
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunge : Ability
{
    [SerializeField] private float startup; // How long does the enemy indicate before it lunges?
    [SerializeField] private float startupSpeed; // While starting up, the entity will back up. How fast does it do this?
    [SerializeField] private float lungeForce; // How far does the entity lunge?
    [SerializeField] private float endtime; // After lunging, how long (in seconds) before the entity can move again

    private Rigidbody rb;
    private BoxCollider2D hitbox;
    private Entity entity;
    private bool lunging;

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        entity = GetComponent<Entity>();
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
        rb.velocity = -transform.up * startupSpeed;
        yield return new WaitForSeconds(startup);
        
        // Reset velocity and apply a powerful force forward to lunge
        rb.velocity = Vector2.zero;
        lunging = true;
        rb.AddForce(transform.up * lungeForce, ForceMode2D.Force);
        
        // Wait for endTime seconds before ending the lunge
        yield return new WaitForSeconds(endtime*(100/(100+entity.totalSpeed)));
        lunging = false;

        sprite.color = baseColor;

        // Re-enable A* Pathfinding movement
        try { GetComponent<AIPath>().canMove = true; }
        catch { }
    }

    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lunging)
        {
            try 
            { 
                collision.gameObject.GetComponent<Entity>().Damage(10); // 10 should be replaced by the appropriate damage calculation
            }
            catch { }
        }
    }
}