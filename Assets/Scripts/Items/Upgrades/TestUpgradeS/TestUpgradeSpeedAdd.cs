// ## - NK
using UnityEngine;

public class TestUpgradeSpeedAdd : Upgrade
{
    [SerializeField]
    private float speedToAdd = 0f;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.totalSpeed += speedToAdd * quantity;
        Debug.Log("Added to speed by " + (speedToAdd * quantity));
    }
}
