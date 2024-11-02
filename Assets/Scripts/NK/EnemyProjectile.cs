using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField]
    private LayerMask playerLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //if (!shooter) { Debug.LogError("Bullet initialized without reference to shooter"); }
    }

    void Update()
    {
        rb.velocity = transform.up * speed;
    }

    // When the bullet collides with something, disable it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
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
