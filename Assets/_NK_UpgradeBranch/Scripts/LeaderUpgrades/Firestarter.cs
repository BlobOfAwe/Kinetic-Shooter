using UnityEngine;

public class Firestarter : Upgrade
{
    [SerializeField]
    private float igniteChance = 0.2f;

    [SerializeField]
    private float tickDamage = 1f;

    [SerializeField]
    private float tickSpeed = 1f;

    [SerializeField]
    private float burnTime = 1f;

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (Random.Range(0f, 1f) <= igniteChance && target.GetComponent<Enemy>() != null)
        {
            target.GetComponent<Entity>().Ignite(tickDamage, tickSpeed, burnTime);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.firestarterAbility, this.transform.position);
        }
    }
}
