using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject target;

    public float sightRange; // How far away can the enemy see
    public float pursuitDuration; // How long the enemy will pursue the player while outside FOV before losing interest
    public float pursuitTimer;
    public float stayDistance; // How close the enemy will get to the player
    public float acceleration = 1f; // How fast does the enemy accelerate
    public float speed = 1f; // How fast can the enemy move
    public LayerMask hostile;
    public LayerMask wall;
    public Ability primary, secondary, utility, additional;

    // Unless specified otherwise, state 0 is Wandering, state 1 is Pursuing. States beyond that should be written into the derivative class
    public int state;

    private float stateChangeCooldown = 0.5f;
    private int lastState;
    private float stateChangeCooldownTimer;

    // Checks to see if a valid target is within sightRange
    public void SearchForTarget()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, sightRange, hostile);
        try { target = targetCollider.gameObject; }
        catch { }
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

    // Chase the player at speed
    public void Pursue()
    {
        Vector2 dir = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        transform.up = dir;
        rb.velocity = transform.up * speed;
    }

    // Stop moving towards the player, and strafe right around the player
    public void Strafe()
    {
        Vector2 dir = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        transform.up = dir;
        rb.velocity = transform.right * speed;
    }

    // Check for wall
    public void WallCheck(Vector2 direction, float range)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, direction, range, wall);
    }

    public bool ReadyToStateChange()
    {
        // If the player's state changed last frame OR if the cooldown is already in progress
        if (state != lastState || stateChangeCooldownTimer != 0) 
        { 
            // Add to the cooldown and return true if the cooldown is complete, and false otherwise
            stateChangeCooldownTimer += Time.deltaTime;
            if (stateChangeCooldownTimer > stateChangeCooldown) { stateChangeCooldownTimer = 0; return true; }
            else { return false; }
        }

        // If the player's state has not changed, and the cooldown is not in progress, then return true 
        else { return true; }       
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stayDistance);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        if (target != null) { Gizmos.DrawLine(transform.position, target.transform.position); }
    }
}
