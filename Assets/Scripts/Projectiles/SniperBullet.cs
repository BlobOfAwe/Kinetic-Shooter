using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : Projectile
{
    // Added by Nathaniel Klassen
    [SerializeField]
    private LayerMask shootableLayer;
    private float knockback;

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
            // transform.position = Vector2.zero; // Moved - NK
            // if statement should check against damageable objects.

            if (collision.gameObject.GetComponent<Rigidbody2D>())
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse);
            }

            if (collision.gameObject.GetComponent<Entity>())
            {
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * shooterEntity.totalAttack);
            }

            gameObject.SetActive(false);

            transform.position = Vector2.zero; // Moved here in case upgrades need position of bullet when destroyed. - NK
            //gameObject.SetActive(false); // This is now done in PlayerBehaviour.ProjectileDestroyEffect() - NK
        }
    }
}
