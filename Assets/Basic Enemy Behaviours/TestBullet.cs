using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBullet : Projectile
{
    public GameObject shooter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!shooter) { Debug.LogError("Bullet initialized without reference to shooter"); }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed;
    }

    // When the bullet collides with something, disable it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != shooter)
        {
            transform.position = Vector2.zero;
            try { collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse); } catch { }
            gameObject.SetActive(false);
        }
    }
}
