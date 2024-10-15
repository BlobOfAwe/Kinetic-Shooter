using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ReadyToStateChange()){
            if (target == null) { state = 0; } // If the enemy has no target, Wander
            else if (Vector2.Distance(target.transform.position, transform.position) > stayDistance) { state = 1; } // If the enemy has a target far enough away, Pursue
            else if (Vector2.Distance(target.transform.position, transform.position) < stayDistance) { state = 2; } // If the enemy is close to its target, Attack
        }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget(); 
                break;
            case 1: // Pursuit
                RefreshTarget();
                Pursue(); 
                break;
            case 2: // Attack
                Strafe();
                if (primary.available) { primary.OnActivate(); }
                break;
        }
    }

    
}
