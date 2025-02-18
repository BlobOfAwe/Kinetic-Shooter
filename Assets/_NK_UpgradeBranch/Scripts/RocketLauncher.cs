// ## - NK
using UnityEngine;

public class RocketLauncher : Upgrade
{
    // This upgrade no longer increases base attack power.
    //[SerializeField]
    //private float damageIncrease = 0.1f;

    [SerializeField]
    private float damagePercent = 1f;

    [SerializeField]
    private float bulletKnockbackIncrease = 0.05f;

    [SerializeField]
    private float bulletSpeedIncrease = -0.1f;

    [SerializeField]
    private GameObject explosion;

    private StandardPrimaryFire shootAbility; // TestShootBullet will be replaced with whatever is the final basic bullet shooting ability.

    protected override void Awake()
    {
        base.Awake();
        shootAbility = player.GetComponent<StandardPrimaryFire>();
    }

    public override void ApplyUpgrade(int quantity)
    {
        //player.attackMultiplier += damageIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease * Mathf.Pow(shootAbility.bulletSpeedMultiplier,quantity);
        // AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketEquipAbility, this.transform.position);
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, bool hitDamageable, int quantity)
    {
        Debug.Log(bullet.transform.position);
        Instantiate(explosion, bullet.transform.position, Quaternion.identity).GetComponent<RocketExplosion>().SetDamage(player.totalAttack * bullet.damageMultiplier * quantity * damagePercent);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketImpactAbility, this.transform.position);
    }
}
