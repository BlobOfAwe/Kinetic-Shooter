using UnityEngine;

public class Electromagnetism : Upgrade
{
    [SerializeField]
    private float attackIncrease = 0.05f;

    [SerializeField]
    private float manualMoveIncrease = 0.05f;

    [SerializeField]
    private float bulletKnockbackIncrease = -0.05f;

    private StandardPrimaryFire shootAbility;

    protected override void Awake()
    {
        base.Awake();
        shootAbility = player.GetComponent<StandardPrimaryFire>();

        // Debug - remove later.
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ApplyUpgrade(int quantity)
    {
        player.attackMultiplier += attackIncrease * quantity;
        player.speedMultiplier += manualMoveIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier = Mathf.Pow(shootAbility.bulletKnockbackMultiplier + bulletKnockbackIncrease, quantity);

        // Debug - remove later.
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }
}
