using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKamikazeEnemy : Enemy
{
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // If the enemy is far enough away, Pursue
            else
            { state = 1; }

        }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                Wander();
                break;
            case 1: // Pursuit
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                Pursue();
                if (primary.available && distanceToTarget < (primary.range)) { UseAbility(primary); }
                break;
        }
    }
}
