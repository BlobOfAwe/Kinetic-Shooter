using UnityEngine;

// This can probably be renamed to "Enemy" and Enemy could be renamed to "EnemyBehaviour".
public class EnemyEntity : Entity
{
    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
}
