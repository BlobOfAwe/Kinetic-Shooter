using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBullet : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = Vector2.zero;
        try { collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse); } catch { }
        gameObject.SetActive(false);
    }
}
