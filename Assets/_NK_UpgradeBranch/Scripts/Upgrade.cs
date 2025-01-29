// ## - NK
using UnityEngine;

public abstract class Upgrade : Item
{
    protected PlayerBehaviour player;

    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerBehaviour>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        player.UpdateStats();
    }

    public virtual void ApplyUpgrade(int quantity)
    {
        Debug.Log("Upgrade applied.");
    }

    public virtual void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        Debug.Log("The projectile was destroyed.");
    }

    public virtual void FireUpgradeEffect(int quantity)
    {
        Debug.Log("The projectile was fired.");
    }
}
