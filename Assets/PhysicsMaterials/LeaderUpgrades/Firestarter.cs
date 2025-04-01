using UnityEngine;

public class Firestarter : Upgrade
{
    [SerializeField]
    private float igniteChance = 0.2f;

    [SerializeField]
    private float tickDamage = 1f;

    [SerializeField]
    private float tickInterval = 1f;

    [SerializeField]
    private float burnTime = 1f;

    [SerializeField]
    private GameObject flame;

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (Random.Range(0f, 1f) <= igniteChance * quantity * bullet.effectModifier && target.GetComponent<Enemy>() != null)
        {
<<<<<<< Updated upstream:Assets/PhysicsMaterials/LeaderUpgrades/Firestarter.cs
            target.GetComponent<Entity>().Ignite(tickDamage, tickInterval, burnTime, flame);
=======
            target.GetComponent<Entity>().Ignite(tickDamage, tickSpeed, burnTime);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.firestarterAbility, this.transform.position);
>>>>>>> Stashed changes:Assets/_NK_UpgradeBranch/Scripts/LeaderUpgrades/Firestarter.cs
        }
    }
}
