// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderWarCallerEnemy : Enemy
{
    protected override void Start()
    {
        Enemy[] children = GetComponentsInChildren<Enemy>();
        foreach (Enemy child in children) { child.transform.parent = null; }
        
        base.Start();
    }
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
                break;
            case 1: // Keep Back
                KeepBack();
                RefreshTarget();
                break;
        }

        if (primary.available) { UseAbility(primary); }
        if (secondary.available) { UseAbility(secondary); }
    }
}
