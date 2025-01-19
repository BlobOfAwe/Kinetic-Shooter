using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBullet : Projectile
{
    // Added by Nathaniel Klassen
    [SerializeField]
    private LayerMask shootableLayer;
    private float knockback;
    private bool stuck;

    [SerializeField] private float stickDuration;
    private float stuckTimer;

    [SerializeField] float tickDamageMultiplier; // While stuck, deals damage based on the Attack stat multiplied by the damage modifier
    private Entity stuckTo;


    // Update is called once per frame
    protected override void Update()
    {
        if (!stuck)
        {
            base.Update();
            rb.velocity = transform.up * speed * speedMultiplier;
        }
        else
        {
            stuckTimer -= Time.deltaTime;
            
            if (stuckTo != null) 
            { 
                // This ensures that the bullet doesn't deal overflow damage from the timer going past zero, making sure it deals the same damage every time
                if (stuckTimer < 0)
                {
                    float correctedDeltaTime = Time.deltaTime + stuckTimer;
                    stuckTo.Damage(tickDamageMultiplier * shooterEntity.totalAttack * correctedDeltaTime);

                    // Once the timer expires, reset all values and disable the bullet
                    gameObject.SetActive(false);
                    transform.parent = null;
                    collider.enabled = true;
                    stuck = false;
                    stuckTo = null;
                    rb.isKinematic = false;
                }
                else
                {
                    stuckTo.Damage(tickDamageMultiplier * shooterEntity.totalAttack * Time.deltaTime);
                }
                
            }

            
            if (stuckTimer < 0) { }
        }
        
    }

    // When the bullet collides with something, disable it
    // Changed so that bullet is only disabled if it collides with something that matches a specific layer. - NK
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>())
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse);
            }

            if (collision.gameObject.GetComponent<Entity>())
            {
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * shooterEntity.totalAttack);
            }

            stuck = true;
            transform.parent = collision.transform;
            stuckTo = transform.parent.GetComponent<Entity>();
            collider.enabled = false;
            stuckTimer = stickDuration;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }
    }
}
