// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public abstract class Projectile : MonoBehaviour
{
    public float speedMultiplier = 1f; // Added so that upgrades can affect projectile speed. - NK
    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    public float damageMultiplier = 1f; // Deals damage based on the totalAttack of the Shooter // Changed to public. - NK
    public float knockbackMultiplier; // Added so that upgrades can affect bullet knockback. - NK
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected Transform firePoint;
    protected new Collider2D collider;

    public GameObject shooter;
    protected Entity shooterEntity;

    // Everything below added by Nathaniel Klassen
    public float despawnTime = 0f;
    [HideInInspector]
    public float timeRemaining;

    protected virtual void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (!shooter) 
        { Debug.LogError("Bullet initialized without reference to shooter"); }
        else
        {
            shooterEntity = shooter.GetComponentInParent<Entity>();
            if (!shooterEntity) { Debug.LogWarning("Bullet not associated with an Entity object shooter."); }
        }
    }
    protected virtual void Update()
    {
        // Despawns the projectile automatically after a set amount of time has passed without it hitting anything.
        // If despawnTime is set to 0 or a negative number, there is no time limit.
        if (despawnTime > 0f)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
            } else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
