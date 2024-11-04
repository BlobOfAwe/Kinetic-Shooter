// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange()){
            // If the enemy has no target, Wander
            if (target == null) 
            { state = 0; }
            // If the enemy is already strafing and the target has not gone far enough to be chased, Strafe, 
            else if (state == 2 && distanceToTarget < stayDistance + chaseDistance) 
            { state = 2; }
            // If the enemy is far enough away, Pursue
            else if (distanceToTarget > stayDistance) 
            { state = 1; }
            // If the enemy is close to its target, Attack
            else if (distanceToTarget < stayDistance) 
            { state = 2; } 
            
        }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget(); 
                break;
            case 1: // Pursuit
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                Pursue();
                break;
            case 2: // Attack
                Strafe();
                if (secondary.available && distanceToTarget < (secondary.range + 1)) { UseAbility(secondary); }
                else if (primary.available && distanceToTarget < primary.range) { UseAbility(primary); };
                break;
        }
    }

    
}
