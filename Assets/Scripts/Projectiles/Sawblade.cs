using UnityEngine;

public class Sawblade : MonoBehaviour
{
    // Added by Nathaniel Klassen
    [SerializeField]
    private LayerMask shootableLayer;
    private Rigidbody2D anchor;
    [SerializeField] private float speed;
    [SerializeField] private float damageMultiplier;
    private Entity parentEntity;
    private LineRenderer lineRenderer;

    private void Start()
    {
        anchor = transform.parent.GetComponent<Rigidbody2D>();
        parentEntity = GetComponentInParent<Entity>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        anchor.transform.localPosition = Vector3.zero;
        anchor.angularVelocity = speed;
        lineRenderer.SetPosition(0, anchor.transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            // transform.position = Vector2.zero; // Moved - NK
            // if statement should check against damageable objects.

            if (collision.gameObject.GetComponent<Entity>())
            {
                collision.gameObject.GetComponent<Entity>().Damage(damageMultiplier * parentEntity.totalAttack);
            }
        }
    }
}
