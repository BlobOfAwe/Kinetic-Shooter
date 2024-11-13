// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderWarCallerEnemy : Enemy
{
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // If the enemy has a target, keep back from it
            else
            { state = 1; }

        }

        switch (state)
        {
            case 0: // Wandering
                Wander();
                SearchForTarget();
                if (primary.available && distanceToTarget < (primary.range + 1)) { UseAbility(primary); }
                break;
            case 1: // Keep Back
                KeepBack();
                RefreshTarget();
                if (primary.available && distanceToTarget < (primary.range + 1)) { UseAbility(primary); }
                break;
        }
    }
}
