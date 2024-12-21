// ## - NK
using UnityEngine;

public class TestUpgradeAttackAdd : Upgrade
{
    [SerializeField]
    private float attackToAdd = 0f;

    public override void ApplyUpgrade(int quantity)
    {
        player.totalAttack += attackToAdd * quantity;
        Debug.Log("Added to attack by " + (attackToAdd * quantity));
    }
}
