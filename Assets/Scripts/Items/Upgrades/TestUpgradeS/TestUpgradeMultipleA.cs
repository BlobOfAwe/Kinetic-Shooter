// ## - NK
using UnityEngine;

public class TestUpgradeMultipleA : Upgrade
{
    [SerializeField]
    private float speedMultiplier = 1f;

    [SerializeField]
    private float attackToAdd = 0f;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.speedMultiplier += speedMultiplier * quantity;
        player.totalAttack += attackToAdd * quantity;
        Debug.Log("Speed multiplier + " + (speedMultiplier * quantity));
        Debug.Log("Added to attack by " + (attackToAdd * quantity));
    }
}
