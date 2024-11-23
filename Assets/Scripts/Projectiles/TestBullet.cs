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
    public float knockback; // Changed to public so that this value can be changed by abilities. - NK

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
        rb.velocity = transform.up * speed;
    }

    // When the bullet collides with something, disable it
    // Changed so that bullet is only disabled if it collides with something that matches a specific layer. - NK
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            transform.position = Vector2.zero;
            // if statement should check against damageable objects.
            try 
            { 
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * shooter.GetComponentInParent<Entity>().totalAttack);
            } 
            catch
            {
                Debug.LogError("TODO: Colliding against non-damagable objects.");
            }
            gameObject.SetActive(false);
        }
    }
}
