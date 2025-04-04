// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBouncerEnemy : Enemy
{

    private Animator animator;
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    new private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // Otherwise, pursue the target
            else { state = 1; }

        }

        if (primary.available && !utility.inUse) { UseAbility(primary); }
        if (secondary.available && !utility.inUse) { UseAbility(secondary); }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                animator.SetBool("isMoving", true);
                Wander();
                break;
            case 1: // Attack
                FaceTarget();
                if (utility.available) { UseAbility(utility); }
                break;
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
