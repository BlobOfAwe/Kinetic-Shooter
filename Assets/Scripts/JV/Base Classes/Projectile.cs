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
    [SerializeField]
    protected float knockback = 1f;
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected Transform firePoint;
}
