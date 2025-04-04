// ## - JV
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSeigeBruteEnemy : Enemy
{
    
    private Animator animator;
    [SerializeField]
    private float accelerateOnDamageTaken = 0.1f;
    [SerializeField]
    private float degradeSpeedRate = 0.1f;
    private Buff speedBuff;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
        speedBuff = ScriptableObject.CreateInstance<Buff>();
        speedBuff.buffType = Buff.buffCategory.SPEED_BUFF;
        speedBuff.modification = Buff.modificationType.Additive;
        speedBuff.value = 0;
        ApplyBuff(speedBuff);
    }
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            if (state != 2)
            {
                // If the enemy has no target, Wander
                if (target == null)
                { state = 0; }
                // Otherwise, pursue the target
                else { state = 1; }
            }

        }
        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                animator.SetBool("isMoving", true);
                Wander();
                break;
            case 1: // Attack
                if (primary.available) { UseAbility(primary); } // Rockfall Ability
                else if (secondary.available && !primary.inUse) { UseAbility(secondary); } // Fastball Ability
                AudioManager.instance.PlayOneShot(FMODEvents.instance.bruteShoot, this.transform.position);
                if (utility.available) { UseAbility(utility); } // Disgorge Basic Enemies
                // Additional Ability is a passive that applies speed buffs when the boss is shot

                Pursue();
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not

                break;
            case 2: // Hold Stil
                FaceTarget();
                RefreshTarget();
                break;
        }
        speedBuffs.Remove(speedBuff);
        speedBuff.value -= degradeSpeedRate * Time.deltaTime;
        ApplyBuff(speedBuff);
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        speedBuffs.Remove(speedBuff);
        speedBuff.value += accelerateOnDamageTaken;
        ApplyBuff(speedBuff);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bruteDeath, this.transform.position);
    }
    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hostile & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<Entity>().Damage(totalAttack, true);
        }
    }
    public override void Death()
    {
        animator.SetTrigger("isDead");
        StartCoroutine(DeathSequence());
    }
    private IEnumerator DeathSequence()
    {
        float deathAnimationLength = 0.8f;
        yield return new WaitForSeconds(deathAnimationLength);
        base.Death();
    }
}
