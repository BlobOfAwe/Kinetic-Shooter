using UnityEngine;

public class BladeSpinner : Upgrade
{
    [SerializeField]
    private float spawnChance = 0.2f;

    [SerializeField]
    private float bladeDamageMod = 0.2f;

    [SerializeField]
    private GameObject blade;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (Random.Range(0f, 1f) <= spawnChance * quantity * bullet.effectModifier)
        {
            Instantiate(blade, bullet.transform.position, Quaternion.identity).GetComponent<SpinningBlade>().SetDamage(player.totalAttack * bullet.damageMultiplier * bladeDamageMod);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.swooshMelee, this.transform.position);
        }
    }
}
