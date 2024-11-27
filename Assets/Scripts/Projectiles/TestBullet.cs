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
    public float knockbackMultiplier; // Added so that upgrades can affect bullet knockback. - NK
    private float knockback;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!shooter) { Debug.LogError("Bullet initialized without reference to shooter"); }
    }

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
            bool hitDamageable;
            // transform.position = Vector2.zero; // Moved - NK
            // if statement should check against damageable objects.
            try 
            { 
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback * knockbackMultiplier, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * shooter.GetComponentInParent<Entity>().totalAttack);
                hitDamageable = true;
            } 
            catch
            {
                Debug.LogError("TODO: Colliding against non-damagable objects.");
                hitDamageable = false;
            }
            FindObjectOfType<PlayerBehaviour>().ProjectileDestroyEffect(this, hitDamageable); // Instead of disabling the object, first apply effects based on upgrades. - NK
            transform.position = Vector2.zero; // Moved here in case upgrades need position of bullet when destroyed. - NK
            //gameObject.SetActive(false); // This is now done in PlayerBehaviour.ProjectileDestroyEffect() - NK
        }
    }
}
