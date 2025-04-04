using UnityEngine;

public class LifeInsurance : Upgrade
{
    [SerializeField]
    private float defenseIncrease = 0.05f;

    [SerializeField]
    private float healthIncrease = 0.05f;

    [SerializeField]
    private float bulletSpeedIncrease = 0.05f;

    [SerializeField]
    private float bulletKnockbackIncrease = -0.05f;

    [SerializeField]
    private float manualMoveIncrease = -0.05f;

    [SerializeField]
    private float healAmount = 0.05f;



    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.defenseMultiplier += defenseIncrease * quantity;
        player.healthMultiplier += healthIncrease * quantity;
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease * quantity;
        //shootAbility.bulletKnockbackMultiplier = Mathf.Pow(shootAbility.bulletKnockbackMultiplier + bulletKnockbackIncrease, quantity);
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        //player.speedMultiplier = Mathf.Pow(player.speedMultiplier + manualMoveIncrease, quantity);
        player.speedMultiplier += manualMoveIncrease * quantity;
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (target.GetComponent<Entity>() != null)
        {
            player.Heal((healAmount * quantity) * ((bullet.damageMultiplier * player.totalAttack) * (100 / (100 + target.GetComponent<Entity>().totalDefense))));
            AudioManager.instance.PlayOneShot(FMODEvents.instance.lifeInsuranceAbility, this.transform.position);
        }
    }
}
