using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField]
    private float damage = 0f;

    [SerializeField]
    private float despawnTime = 1f;

    private float timeRemaining;

    private void Awake()
    {
        timeRemaining = despawnTime;
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
        }
    }

    private void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
