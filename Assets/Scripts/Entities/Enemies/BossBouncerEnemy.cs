// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBouncerEnemy : Enemy
{
    [SerializeField]
    private Animator bossAnimator;
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
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
                bossAnimator.SetBool("isMoving", true);
                Wander();
                break;
            case 1: // Attack
                FaceTarget();
                if (utility.available) { UseAbility(utility); }
                break;
        }
    }
}
