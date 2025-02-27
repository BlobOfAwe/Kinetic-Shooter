using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTertiaryBall : Ability
{
    [Header("Withdrawn Stats")]
    [SerializeField] private float duration;
    private Buff armorBuff;
    private Buff speedDebuff;
    private bool curled;

    [Header("Lunge Stats")]
    public LayerMask canDamage = 128; // Objects on these layers are valid attack targets
    [SerializeField] private float startup; // How long does the enemy indicate before it lunges?
    [SerializeField] private float startupSpeed; // While starting up, the entity will back up. How fast does it do this?
    [SerializeField] private float lungeForce; // How far does the entity lunge?
    [SerializeField] private float endtime; // After lunging, how long (in seconds) before the entity can move again

    private BoxCollider2D hitbox;
    private bool lunging;
    private PlayerBehaviour player;
    private Rigidbody2D rb;
    private Coroutine ballCurlCoroutine;

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;

    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        // Initialize Variables
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        baseColor = sprite.color;
        curled = false;

        // Construct the armor buff to make the player invincible while curled
        armorBuff = ScriptableObject.CreateInstance<Buff>();
        armorBuff.buffType = Buff.buffCategory.DEFENSE_BUFF;
        armorBuff.modification = Buff.modificationType.Additive;
        armorBuff.value = 999999999;

        // Construct the speed debuff to immobilize the player while curled
        speedDebuff = ScriptableObject.CreateInstance<Buff>();
        speedDebuff.buffType = Buff.buffCategory.SPEED_BUFF;
        speedDebuff.modification = Buff.modificationType.Multiplicative;
        speedDebuff.value = 0;
    }

    public override void OnActivate()
    {
        if (!curled)
        {
            rb.isKinematic = true;
            curled = true;
            player.canMoveManually = false;
            player.ApplyBuff(armorBuff);
            player.ApplyBuff(speedDebuff);
            player.primary.StopAllCoroutines();
            player.primary.available = false;
            player.secondary.StopAllCoroutines();
            player.secondary.available = false;
            sprite.color = Color.blue;

            ballCurlCoroutine = StartCoroutine(BallCurl());
        }

        else
        {
            StopCoroutine(ballCurlCoroutine);
            rb.isKinematic = false;
            curled = false;
            player.speedBuffs.Remove(speedDebuff);
            player.UpdateStats();
            player.canMoveManually = true;
            StartCoroutine(LungeForward());
        }
    }

    private IEnumerator LungeForward()
    {
        sprite.color = Color.red;
        rb.velocity = -player.aimTransform.up * startupSpeed;
        yield return new WaitForSeconds(startup);

        // Reset velocity and apply a powerful force forward to lunge
        rb.velocity = Vector2.zero;
        lunging = true;
        rb.AddForce(player.aimTransform.up * lungeForce, ForceMode2D.Impulse);

        // Wait for endTime seconds before ending the lunge
        yield return new WaitForSeconds(endtime * (100 / (100 + thisEntity.totalSpeed)));
        lunging = false;
        player.defenseBuffs.Remove(armorBuff);
        player.speedBuffs.Remove(speedDebuff);
        player.UpdateStats();

        player.primary.available = true;
        player.secondary.available = true;

        StartCoroutine(BeginCooldown());

        sprite.color = baseColor;
    }
    private IEnumerator BallCurl()
    {
        yield return new WaitForSeconds(duration);
        rb.isKinematic = false;
        player.defenseBuffs.Remove(armorBuff);
        curled = false;
        player.speedBuffs.Remove(speedDebuff);
        player.UpdateStats();
        player.canMoveManually = true;
        sprite.color = baseColor;
        StartCoroutine(BeginCooldown());
        player.primary.available = true;
        player.secondary.available = true;
    }

    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lunging)
        {
            if ((canDamage & (1 << collision.gameObject.layer)) != 0)
            {
                collision.gameObject.GetComponent<Entity>().Damage(thisEntity.totalAttack * damageModifier); // 10 should be replaced by the appropriate damage calculation
            }
        }
    }
}
