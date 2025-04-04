using FMODUnity;
using UnityEngine;

public class SpinningBlade : MonoBehaviour
{
    [SerializeField]
    private float damage = 0f;

    [SerializeField]
    private float damageCooldown = 1f;

    [SerializeField]
    private float despawnTime = 1f;

    private float timeToDamage;

    private float timeToDespawn;

    private void Awake()
    {
        timeToDespawn = despawnTime;
        timeToDamage = damageCooldown;
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        timeToDamage -= Time.deltaTime;
        if (timeToDamage <= 0f)
        {
            if (collision.gameObject.GetComponent<Enemy>() != null)
            {
                //Debug.Log("HIT " + collision.gameObject.name + " FOR " + damage + " DAMAGE!");
                collision.gameObject.GetComponent<Enemy>().Damage(damage);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.bladeSpinner, this.transform.position);
            }
            timeToDamage = damageCooldown;
        }
    }

    private void Update()
    {
        timeToDespawn -= Time.deltaTime;
        if (timeToDespawn <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
