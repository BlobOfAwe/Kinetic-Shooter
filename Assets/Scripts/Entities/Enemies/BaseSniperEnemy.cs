using UnityEngine;

public class BaseSniperEnemy : Enemy
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
            else if (distanceToTarget > stayDistance + chaseDistance)
            { state = 1; }
            // If the enemy is too close to its target, retreat
            else if (distanceToTarget < stayDistance)
            { state = 3; }
            // If the enemy is between the pursuit and stay distances, Attack
            else if (distanceToTarget > stayDistance && distanceToTarget < stayDistance + chaseDistance)
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
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                break;
            case 2: // Attack
                FaceTarget();
                if (primary.available && distanceToTarget < (primary.range + 1)) { UseAbility(primary); }
                break;
            case 3: // Retreat
                KeepBack();
                break;
        }

        Debug.DrawRay(transform.position, transform.up);
    }
}