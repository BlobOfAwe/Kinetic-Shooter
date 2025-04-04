using UnityEngine;

public class FastFly : Upgrade
{
    [SerializeField]
    private float cooldownIncrease = -0.1f;

    [SerializeField]
    private float bulletSpeedIncrease = 0.1f;

    [SerializeField]
    private float manualMoveIncrease = 1f;

    [SerializeField]
    private float bulletKnockbackIncrease = 0.05f;

    [SerializeField]
    private float attackIncrease = -0.1f;

    [SerializeField]
    private float defenseIncrease = -0.05f;

    // audio parameter controller script
    [SerializeField] AudioParameterController parameterController;


    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        //shootAbility.cooldownMultiplier = Mathf.Pow(shootAbility.cooldownMultiplier + cooldownIncrease, quantity);
        shootAbility.cooldownMultiplier += cooldownIncrease * quantity;
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease * quantity;
        player.speedMultiplier += manualMoveIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        //player.attackMultiplier = Mathf.Pow(player.attackMultiplier + attackIncrease, quantity);
        player.attackMultiplier += attackIncrease * quantity;
        //player.defenseMultiplier = Mathf.Pow(player.defenseMultiplier + defenseIncrease, quantity);
        player.defenseMultiplier += defenseIncrease * quantity;
        parameterController.PlayerFlying();
    }
}
