// ## - JV
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBullet : Projectile
{
    public GameObject shooter;
    // Added by Nathaniel Klassen
    [SerializeField]
    private LayerMask shootableLayer;

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
            try 
            { 
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse); 
                collision.gameObject.GetComponent<Entity>().Damage(damage);
            } 
            catch { }
            gameObject.SetActive(false);
        }
    }
}
