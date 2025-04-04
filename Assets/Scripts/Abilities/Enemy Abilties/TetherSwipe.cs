using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSwipe : Ability
{
    private BoxCollider2D hitbox;
    private SpriteRenderer hitboxSprite;
    private BuffUIManager buffUI;
    private Animator animator;
    public float knockback;
    public float activeTime;
    [SerializeField] private float onHitSpeedMod;
    [SerializeField] private float speedDebuffMod;
    [SerializeField] private float debuffDuration;

    [SerializeField] float sideReach = 2f;

    [SerializeField] Sprite tempHitbox;

    // Create a gameObject as a child of this gameObject and add a BoxCollider2D trigger based on the ability's stats, then disable it
    new void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
        GameObject hitboxObj = new GameObject("TetherHitbox", typeof(BoxCollider2D), typeof(SpriteRenderer));
        hitboxSprite = hitboxObj.GetComponent<SpriteRenderer>();
        hitboxSprite.sprite = tempHitbox;
        hitboxSprite.color = Color.red;
        hitboxSprite.enabled = false;


        hitbox = hitboxObj.GetComponent<BoxCollider2D>();
        hitbox.transform.localScale = new Vector2(sideReach, range);
        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    private void Start()
    {
        hitbox.transform.parent = transform;
        hitbox.transform.rotation = transform.rotation;
        hitbox.transform.localPosition = transform.up * (0.5f + range/2);
        buffUI = FindObjectOfType<BuffUIManager>();
    }

    // Apply knockback to an object detected by the active hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            Entity target = collision.gameObject.GetComponent<PlayerBehaviour>();
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            Rigidbody2D targetRB = target.gameObject.GetComponent<Rigidbody2D>();
            targetRB.velocity = Vector2.zero;
            targetRB.AddForce(knockbackDir * knockback, ForceMode2D.Impulse);
            ApplySlow(target);
            target.Damage(thisEntity.totalAttack * damageModifier, true);
        }
    }

    // Enable the hitbox for activeTime seconds
    public override void OnActivate()
    {
        hitbox.enabled = true;
        animator.SetTrigger("isAttacking");
        //hitboxSprite.enabled = true;
        StartCoroutine(BeginCooldown());
        StartCoroutine(DisableAfterSeconds());
    }

    public override void PurgeDependantObjects()
    {
        Destroy(hitbox.gameObject);
    }

    IEnumerator DisableAfterSeconds()
    {
        yield return new WaitForSeconds(activeTime);
        hitbox.enabled = false;
        hitboxSprite.enabled = false;
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


    private void ApplySlow(Entity buffTarget)
    {
        Buff buffConstructor = ScriptableObject.CreateInstance<Buff>(); // Create a new buff object

        buffConstructor.buffType = Buff.buffCategory.SPEED_BUFF;

        buffConstructor.value = speedDebuffMod;
        buffConstructor.modification = Buff.modificationType.Multiplicative;

        buffConstructor.duration = debuffDuration;

        buffTarget.ApplyBuff(buffConstructor);

        buffUI.AddBuff(buffConstructor, GenericBuffDebuff.buffType.Debuff, null);//Added by Z.S
    }

}
