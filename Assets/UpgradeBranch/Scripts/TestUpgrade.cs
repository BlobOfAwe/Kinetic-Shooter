// ## - NK
using UnityEngine;

public class TestUpgrade : Upgrade
{
    [SerializeField]
    private float speedMultiplier = 1f;

    public override void ApplyUpgrade(int quantity)
    {
        player.speedMultiplier = speedMultiplier * quantity;
        Debug.Log("Speed multiplied by " + speedMultiplier);
    }
}
