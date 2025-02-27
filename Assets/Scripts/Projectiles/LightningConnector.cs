using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class LightningConnector : Projectile
{
    [SerializeField]
    private LayerMask shootableLayer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.gameObject.GetComponent<Entity>())
            {
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * shooterEntity.totalAttack * Time.deltaTime);
            }
        }
    }
}
