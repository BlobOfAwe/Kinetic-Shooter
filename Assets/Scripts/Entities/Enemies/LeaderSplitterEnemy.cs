using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSplitterEnemy : Enemy
{
    [SerializeField] private LeaderSplitterEnemy[] childSplots;
    [SerializeField] private float splotRepelForce; // How much are the splots repelled from each other after spawning
    [SerializeField] private float iSecondsAfterSplit;

    new private void Start()
    {
        base.Start();

        LeaderSplitterEnemy[] children = GetComponentsInChildren<LeaderSplitterEnemy>();
        foreach (var child in children) 
        {
            if (child != this)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public override void Death()
    {
        if (childSplots != null)
        {
            foreach (var splot in childSplots)
            {
                splot.transform.parent = null;
                splot.gameObject.SetActive(true);
                
                ApplyBuff(splot);

                Vector2 randomDirection = new Vector2(Random.Range(0, 1), Random.Range(0, 1)).normalized;
                splot.GetComponent<Rigidbody2D>().AddForce(randomDirection * splotRepelForce, ForceMode2D.Impulse);
            }
        }
        base.Death();
    }
    
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // If the enemy is far enough away, Pursue
            else if (distanceToTarget > stayDistance)
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
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                break;
        }
    }

    // If this entity collides with something while it is lunging, attempt to damage it
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hostile & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<Entity>().Damage(totalAttack);
        }
    }

    public void ApplyBuff(Enemy buffTarget)
    {
        Buff buffConstructor = ScriptableObject.CreateInstance<Buff>(); // Create a new buff object

        buffConstructor.buffType = Buff.buffCategory.DEFENSE_BUFF;

        buffConstructor.value = 999999;
        buffConstructor.modification = Buff.modificationType.Additive;

        buffConstructor.duration = iSecondsAfterSplit;

        buffTarget.ApplyBuff(buffConstructor);
        FindObjectOfType<BuffUIManager>().AddBuff(buffConstructor);//Added by Z.S
    }

}
