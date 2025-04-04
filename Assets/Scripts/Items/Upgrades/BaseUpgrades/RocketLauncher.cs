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


    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        //player.attackMultiplier += damageIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        shootAbility.bulletSpeedMultiplier = Mathf.Pow(shootAbility.bulletSpeedMultiplier + bulletSpeedIncrease, quantity);
        // AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketEquipAbility, this.transform.position);
    }

    public override void FireUpgradeEffect(int quantity, TestBullet bullet)
    {
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketLaunchAbility, this.transform.position);
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        //Debug.Log(bullet.transform.position);
        Instantiate(explosion, bullet.transform.position, Quaternion.identity).GetComponent<RocketExplosion>().SetDamage(player.totalAttack * bullet.damageMultiplier * quantity * damagePercent);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketImpactAbility, this.transform.position);
    }
}
