using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardTertiaryBoomDash : Ability
{
    [SerializeField] private LayerMask enemyLayer = 128; // 128 represents the bitmask 1000000, referencing the 7th layer "Enemy".
    [SerializeField] private float explosionDropoff;
    [SerializeField] private float explosionKnockback;
    [SerializeField] private float dashForce;
    private PlayerBehaviour player;

    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    public override void OnActivate()
    {

        StartCoroutine(BeginCooldown());

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        foreach (var obj in enemies)
        {
            Enemy targetEntity = obj.gameObject.GetComponent<Enemy>();
            if (targetEntity != null && obj.gameObject != gameObject)
            {
                float distanceDropoffValue = 1 - (Mathf.Pow(Vector2.Distance(transform.position, targetEntity.transform.position) / range, explosionDropoff));

                Vector2 explosionKnockbackVector = (targetEntity.transform.position - transform.position).normalized * explosionKnockback * distanceDropoffValue;
                targetEntity.gameObject.GetComponent<Rigidbody2D>().AddForce(explosionKnockbackVector);

                float explosionDamage = (thisEntity.totalAttack * damageModifier) * distanceDropoffValue;
                Debug.Log("explosionDamage for entity " + targetEntity.gameObject.name + " at " + (Vector2.Distance(transform.position, targetEntity.transform.position) / range) * 100 + "% of range equals " + explosionDamage);
                targetEntity.Damage(explosionDamage);
            }
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(player.aimTransform.up * dashForce, ForceMode2D.Impulse);
    }
}
