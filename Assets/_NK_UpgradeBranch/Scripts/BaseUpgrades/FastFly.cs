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

    private StandardPrimaryFire shootAbility;

    protected override void Awake()
    {
        base.Awake();
        shootAbility = player.GetComponent<StandardPrimaryFire>();

        // Debug - remove later.
        //FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ApplyUpgrade(int quantity)
    {
        shootAbility.cooldownMultiplier = Mathf.Pow(shootAbility.cooldownMultiplier + cooldownIncrease, quantity);
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease * quantity;
        player.speedMultiplier += manualMoveIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        player.attackMultiplier = Mathf.Pow(player.attackMultiplier + attackIncrease, quantity);
        player.defenseMultiplier = Mathf.Pow(player.defenseMultiplier + defenseIncrease, quantity);

        // Debug - remove later.
        //FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }
}
