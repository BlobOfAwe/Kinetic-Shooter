using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSecondarySwipe : Ability
{
    private BoxCollider2D hitbox;
    private SpriteRenderer hitboxSprite;
    private PlayerBehaviour player;
    [SerializeField]
    private Animator swipeAnim;

    public float knockback;
    [SerializeField] float staggerTime = 3f;
    [SerializeField] float sideReach = 2f;
    public float activeTime;

    [SerializeField] Sprite tempHitbox;

    // Create a gameObject as a child of this gameObject and add a BoxCollider2D trigger based on the ability's stats, then disable it
    new void Awake()
    {
        base.Awake();
        GameObject hitboxObj = new GameObject("SecondarySwipeHitbox", typeof(BoxCollider2D), typeof(SpriteRenderer));
        hitboxSprite = hitboxObj.GetComponent<SpriteRenderer>();
        hitboxSprite.sprite = tempHitbox;
        hitboxSprite.color = Color.red;
        hitboxSprite.enabled = false;


        hitbox = hitboxObj.GetComponent<BoxCollider2D>();
        hitbox.transform.localScale = new Vector2(sideReach, range);
        hitbox.offset = Vector2.up * (range / 2 - 1);
        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        hitbox.transform.parent = transform;
        hitbox.transform.rotation = player.firePoint.rotation;
        hitbox.transform.position = player.firePoint.position;
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
            target.Damage(thisEntity.totalAttack * damageModifier);
        }
    }

    // Enable the hitbox for activeTime seconds
    public override void OnActivate()
    {
        hitbox.transform.rotation = player.firePoint.rotation;
        hitbox.transform.position = player.firePoint.position;

        //hitbox.enabled = true;
        //hitboxSprite.enabled = true;
        swipeAnim.SetTrigger("isSwiping");
        StartCoroutine(BeginCooldown());
        StartCoroutine(DisableAfterSeconds());
        AudioManager.instance.PlayOneShot(FMODEvents.instance.standardSecondary, this.transform.position);
    }

    IEnumerator DisableAfterSeconds()
    {
        yield return new WaitForSeconds(activeTime);
        hitbox.enabled = false;
        hitboxSprite.enabled=false;
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
