using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recharge : Ability
{
    private Buff armorBuff;
    private Buff speedDebuff;
    [SerializeField] float duration;
    private Animator animator;

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;

    new private void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        baseColor = sprite.color;

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

        thisEntity.ApplyBuff(armorBuff);

    }

    public override void OnActivate()
    {
        available = false;
        

        StartCoroutine(VulnerableRecharge());
    }

    public IEnumerator VulnerableRecharge()
    {
        thisEntity.defenseBuffs.Remove(armorBuff);
        thisEntity.ApplyBuff(speedDebuff);
        // sprite.color = Color.red;
        animator.SetBool("isDefending", true);
        yield return new WaitForSeconds(duration);
        animator.SetBool("isDefending", false);
        thisEntity.speedBuffs.Remove(speedDebuff);
        thisEntity.ApplyBuff(armorBuff);

        sprite.color = baseColor;

        StartCoroutine(BeginCooldown());
    }
}
