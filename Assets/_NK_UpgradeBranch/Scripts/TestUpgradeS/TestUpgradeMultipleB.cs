// ## - NK
using UnityEngine;

public class TestUpgradeMultipleB : Upgrade
{
    [SerializeField]
    private float speedToAdd = 0f;

    [SerializeField]
    private float attackMultiplier = 1f;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.totalSpeed += speedToAdd * quantity;
        player.attackMultiplier += attackMultiplier * quantity;
        Debug.Log("Added to speed by " + (speedToAdd * quantity));
        Debug.Log("Attack multiplier + " + (attackMultiplier * quantity));
    }
}
