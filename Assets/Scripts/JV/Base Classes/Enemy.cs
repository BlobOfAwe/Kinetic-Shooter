using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
[RequireComponent(typeof (AIPath))]

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject target;
    public Seeker seeker;
    public AIPath aiPath;

    public float distanceToTarget; // Distance from the enemy to the identified target

    public float sightRange; // How far away can the enemy see
    public float stayDistance; // How close the enemy will get to the player
    public float chaseDistance; // How far can the player get before the enemy chases them

    public float pursuitDuration; // How long the enemy will pursue the player while outside FOV before losing interest
    public float pursuitTimer; // Used to measure pursuitDuration
    
    
    public float acceleration = 1f; // How fast does the enemy accelerate
    public float speed = 1f; // How fast can the enemy move

    public LayerMask hostile; // Objects on these layers are valid attack targets
    public Ability primary, secondary, utility, additional;

    // Unless specified otherwise, state 0 is Wandering, state 1 is Pursuing. States beyond that should be written into the derivative class
    public int state;

    private float stateChangeCooldown = 0.5f; // How long after a state change before the state can change again
    private int lastState = -1; // What was the enemy state last frame
    private float stateChangeCooldownTimer; // Used to time stateChangeCooldown

    private void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        seeker = GetComponent<Seeker> ();
        aiPath = GetComponent<AIPath> ();
    }

    public abstract void DerivativeUpdate(); // Used by derivative classes to contain class specific logic, called by the abstract class Update() every frame
    private void Update()
    {
        // As long as a target exists, record how far it is from this object
        if (target) { distanceToTarget = Vector2.Distance(target.transform.position, transform.position); }
        
        DerivativeUpdate(); // Run derived class-specific logic
    }

    // Checks to see if a valid target is within sightRange
    public void SearchForTarget()
    {
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, sightRange, hostile);
        try { target = targetCollider.gameObject; }
        catch { target = null; }
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
        aiPath.enableRotation = true;
        aiPath.destination = target.transform.position;
    }

    // Stop moving towards the player, and strafe right around the player
    public void Strafe()
    {
        aiPath.enableRotation = false;
        if (!aiPath.pathPending && aiPath.reachedEndOfPath || !ReadyToStateChange())
        {
            // Choose a random destination point `stayDistance` distance away from the target.
            aiPath.destination = RandomPointOnEdge(target.transform, stayDistance);
        }
        Vector2 dir = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        transform.up = dir;
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

    private void OnDrawGizmos()
    {
        // Draws the circle around which the enemy strafes
        Gizmos.color = Color.red;
        try { Gizmos.DrawWireSphere(target.transform.position, stayDistance); } catch { }

        // The range beyond which the enemy pursues the player
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, stayDistance + chaseDistance);

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); // The sight range of the enemy
        if (target != null) { Gizmos.DrawLine(transform.position, target.transform.position); } // A direct line to the target
    }
}
