using UnityEngine;

public class Reflector : MonoBehaviour
{
    [SerializeField]
    private LayerMask solidLayer;

    [SerializeField]
    private string enemyBulletTag = "";

    public int reflects = 1;

    private ShootAbility shootAbility;

    private void Awake()
    {
        shootAbility = FindObjectOfType<PlayerBehaviour>().gameObject.GetComponent<ShootAbility>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyBulletTag))
        {
            Projectile enemyBullet = collision.GetComponent<Projectile>();
            foreach (GameObject bullet in shootAbility.bullets)
            {
                if (!bullet.activeSelf)
                {
                    bullet.transform.position = collision.transform.position;
                    bullet.transform.eulerAngles = collision.transform.eulerAngles + Vector3.forward * 180f;
                    bullet.GetComponent<Projectile>().timeRemaining = enemyBullet.despawnTime;
                    bullet.GetComponent<Projectile>().speedMultiplier = enemyBullet.speedMultiplier;
                    bullet.GetComponent<Projectile>().knockbackMultiplier = enemyBullet.knockbackMultiplier;
                    bullet.GetComponent<Projectile>().damageMultiplier = enemyBullet.damageMultiplier;
                    bullet.SetActive(true);
                    break;
                }
            }
            collision.gameObject.SetActive(false);
            reflects -= 1;
            if (reflects <= 0)
            {
                Destroy(gameObject);
            }
        } else if ((solidLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
