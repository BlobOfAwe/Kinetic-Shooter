// ## - JV
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityWell : Projectile
{
    // Added by Nathaniel Klassen
    [SerializeField]
    private LayerMask shootableLayer;
    [SerializeField] private float attractionForce;
    [SerializeField] private float radius;
    [SerializeField] private float dropoff = 2f; // A value of 1 indicates linear drop-off. The greater the value, the slower the initial dropoff
    private Collider2D[] affectedColliders;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        rb.velocity = transform.up * speed * speedMultiplier;
    }

    private void FixedUpdate()
    {
        affectedColliders = Physics2D.OverlapCircleAll(transform.position, radius, shootableLayer);

        foreach (Collider2D collider in affectedColliders)
        {
            Enemy target = collider.gameObject.GetComponent<Enemy>();
            if ( target != null)
            {
                Vector2 knockbackDir = (transform.position - collider.transform.position).normalized;
                Rigidbody2D targetRB = target.gameObject.GetComponent<Rigidbody2D>();
                targetRB.velocity = Vector2.zero;
                targetRB.AddForce(knockbackDir * attractionForce * (1 - (Mathf.Pow(Vector2.Distance(transform.position, target.transform.position) / radius, dropoff))), ForceMode2D.Force);
            }
        }
    }


}
