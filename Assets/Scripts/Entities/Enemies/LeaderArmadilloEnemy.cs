using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderArmadilloEnemy : Enemy
{

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
                break;
            case 1: // Pursuit
                Pursue();
                if (primary.available) { UseAbility(primary); }
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hostile & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<Entity>().Damage(totalAttack, true);
        }
    }
}
