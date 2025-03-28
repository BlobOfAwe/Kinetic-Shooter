// ## - NK
using UnityEngine;

public class TestUpgradeAttackMultiply : Upgrade
{
    [SerializeField]
    private float attackMultiplier = 1f;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.attackMultiplier += attackMultiplier * quantity;
        Debug.Log("Attack multiplier + " + (attackMultiplier * quantity));
    }
}
