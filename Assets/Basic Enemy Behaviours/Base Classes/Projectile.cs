using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public abstract class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 1f;
    public float knockback = 1f;
    public Rigidbody2D rb;

}
