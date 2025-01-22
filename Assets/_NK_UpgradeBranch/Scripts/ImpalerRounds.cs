using UnityEngine;

public class ImpalerRounds : Upgrade
{
    [SerializeField]
    private float cooldownIncrease = -0.05f;

    [SerializeField]
    private float bulletKnockbackIncrease = 0.05f;

    [SerializeField]
    private float bulletSpeedIncrease = -0.05f;

    private StandardPrimaryFire shootAbility;

    protected override void Awake()
    {
        base.Awake();
        shootAbility = player.GetComponent<StandardPrimaryFire>();
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ApplyUpgrade(int quantity)
    {
        shootAbility.cooldownMultiplier += cooldownIncrease * Mathf.Pow(shootAbility.bulletSpeedMultiplier, quantity);
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease * Mathf.Pow(shootAbility.bulletSpeedMultiplier, quantity);
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }
}
