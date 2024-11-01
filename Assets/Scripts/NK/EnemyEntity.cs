using UnityEngine;

// IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!IMPORTANT!!
// ----------------------------------------------------------------------------------------------------
// <NOTE> : As Class<Enemy> now inherits directly from Entity.cs, this script is now obsolete.
// ----------------------------------------------------------------------------------------------------

// This can probably be renamed to "Enemy" and Enemy could be renamed to "EnemyBehaviour".
public class EnemyEntity : Entity
{
    private Enemy enemyBehaviour;

    private void Awake()
    {
        enemyBehaviour = GetComponent<Enemy>();
    }

    protected override void Death()
    {
        // JV - base.Death() can no longer be called as it is now an abstract function. To reenable this code, change Entity.Death to a virtual function
        //base.Death();
        Destroy(gameObject);
    }
}
