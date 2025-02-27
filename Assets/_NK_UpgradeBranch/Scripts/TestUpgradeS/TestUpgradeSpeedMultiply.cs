// ## - NK
using UnityEngine;

public class TestUpgradeSpeedMultiply : Upgrade
{
    [SerializeField]
    private float speedMultiplier = 1f;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.speedMultiplier += speedMultiplier * quantity;
        Debug.Log("Speed multiplier + " + (speedMultiplier * quantity));
    }
}
