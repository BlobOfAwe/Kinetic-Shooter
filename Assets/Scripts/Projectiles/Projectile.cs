// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    protected float damage = 1f;
    public float knockback = 1f;
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected Transform firePoint;

    // Everything below added by Nathaniel Klassen
    public float despawnTime = 0f;
    [HideInInspector]
    public float timeRemaining;

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