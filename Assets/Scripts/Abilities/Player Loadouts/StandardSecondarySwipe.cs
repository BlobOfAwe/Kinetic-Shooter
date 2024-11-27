using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSecondarySwipe : Ability
{
    private BoxCollider2D hitbox;
    private PlayerBehaviour player;

    public float knockback;
    [SerializeField] float staggerTime = 3f;
    [SerializeField] float sideReach = 2f;
    public float activeTime;

    // Create a gameObject as a child of this gameObject and add a BoxCollider2D trigger based on the ability's stats, then disable it
    void Awake()
    {
        GameObject hitboxObj = new GameObject("SecondarySwipeHitbox", typeof(BoxCollider2D));

        hitbox = hitboxObj.GetComponent<BoxCollider2D>();
        hitbox.size = new Vector2(sideReach, range);
        hitbox.offset = Vector2.up * (range / 2 - 1);
        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        hitbox.transform.parent = transform;
        hitbox.transform.rotation = player.aimTransform.rotation;
        hitbox.transform.position = player.aimTransform.position;
    }

    // Apply knockback to an object detected by the active hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy target = collision.gameObject.GetComponent<Enemy>();
            StartCoroutine(target.Stagger(staggerTime));
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            target.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDir * knockback, ForceMode2D.Impulse);
            target.Damage(entity.totalAttack * damageModifier);
        }
    }

    // Enable the hitbox for activeTime seconds
    public override void OnActivate()
    {
        hitbox.transform.rotation = player.aimTransform.rotation;
        hitbox.transform.position = player.aimTransform.position;

        hitbox.enabled = true;
        StartCoroutine(BeginCooldown());
        StartCoroutine(DisableAfterSeconds());
    }

    IEnumerator DisableAfterSeconds()
    {
        yield return new WaitForSeconds(activeTime);
        hitbox.enabled = false;
    }

    new private void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        if (hitbox && hitbox.enabled)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector2.zero + hitbox.offset, hitbox.size);
        }
    }
}
