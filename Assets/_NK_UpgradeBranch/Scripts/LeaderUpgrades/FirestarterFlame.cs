using UnityEngine;

public class FirestarterFlame : MonoBehaviour
{
    [SerializeField]
    private float damage = 1f;

    [SerializeField]
    private float interval = 1f;

    [SerializeField]
    private float burnTime = 1f;

    [SerializeField]
    private GameObject flame;

    [SerializeField]
    private string playerTag = "Player";

    private void Update()
    {
        if (transform.parent.GetComponent<Entity>() != null)
        {
            if (!transform.parent.GetComponent<Entity>().isOnFire)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Entity>() != null)
        {
            if (!collision.CompareTag(playerTag))
            {
                collision.GetComponent<Entity>().Ignite(damage, interval, burnTime, flame);
            }
        }
    }
}
