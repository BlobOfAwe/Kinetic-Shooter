// ## - JV
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBullet : Projectile
{
    // Added by Nathaniel Klassen
    [SerializeField]
    private LayerMask shootableLayer;
    private float knockback;
    [HideInInspector]
    public bool isPiercing = false;
    [HideInInspector]
    public int hits = 1;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        rb.velocity = transform.up * speed * speedMultiplier;
    }

    // When the bullet collides with something, disable it
    // Changed so that bullet is only disabled if it collides with something that matches a specific layer. - NK
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            //bool hitDamageable; // We are now checking if an object has an Entity component to determine if an object is damageable, rather than using a bool. - NK
            // transform.position = Vector2.zero; // Moved - NK
            // if statement should check against damageable objects.

            if (collision.gameObject.GetComponent<Rigidbody2D>())
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse);
            }

            if (collision.gameObject.GetComponent<Entity>())
            {
                //hitDamageable = true;
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * shooterEntity.totalAttack);
            }

            /*else 
            {
                hitDamageable = false;
            }*/

            //gameObject.SetActive(false); // Why was this put here? - NK

            FindObjectOfType<PlayerBehaviour>().ProjectileDestroyEffect(this, collision.gameObject); // Instead of disabling the object, first apply effects based on upgrades. - NK
            if (hits <= 0)
            {
                transform.position = Vector2.zero; // Moved here in case upgrades need position of bullet when destroyed. - NK
            }
            //gameObject.SetActive(false); // This is now done in PlayerBehaviour.ProjectileDestroyEffect() - NK
        }
    }
}
