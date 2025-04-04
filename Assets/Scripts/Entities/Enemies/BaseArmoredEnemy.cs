using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmoredEnemy : Enemy
{
    [SerializeField]
    private Animator animator;
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // If the enemy has a target, Pursue
            else
            { state = 1; }
        }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                Wander();
                animator.SetBool("isMoving", true);
                //animator.SetBool("isAttacking", false);
                break;
            case 1: // Pursuit
                Pursue();
                animator.SetBool("isMoving", true);
                if (primary.available && distanceToTarget < (primary.range + 1)) { UseAbility(primary); }
                //animator.SetBool("isAttacking", true);
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                break;
        }
    }
}
