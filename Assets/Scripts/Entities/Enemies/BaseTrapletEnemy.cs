using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrapletEnemy : Enemy
{ 
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // If the enemy is targeting the player
            else if (target.CompareTag("Player"))
            { state = 1; }
            // If the enemy is targeting a wall
            else
            { state = 2; }

        }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                Wander();
                break;
            case 1: // Pursuit
                Pursue();
                if (primary.available && distanceToTarget < (primary.range)) { UseAbility(primary); }
                break;
            case 2: // Plant spike
                Pursue();
                if (secondary.available && distanceToTarget < (secondary.range)) { UseAbility(secondary); }
                break;
        }
    }
}