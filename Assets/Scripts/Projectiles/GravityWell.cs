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
    private float knockback;
    [SerializeField] private float radius;
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
            if (collider.gameObject.GetComponent<Enemy>() != null)
            {
                Entity target = collider.gameObject.GetComponent<PlayerBehaviour>();
                Vector2 knockbackDir = (collider.transform.position - transform.position).normalized;
                Rigidbody2D targetRB = target.gameObject.GetComponent<Rigidbody2D>();
                targetRB.velocity = Vector2.zero;
                targetRB.AddForce(knockbackDir * knockback, ForceMode2D.Impulse);
            }
        }
    }


}
