// ## - NK
using UnityEngine;

public class TestUpgrade : Upgrade
{
    [SerializeField]
    private float cooldownIncrease = 0f;

    [SerializeField]
    private float recoilIncrease = 0f;

    [SerializeField]
    private float bulletSpeedIncrease = 0f;

    [SerializeField]
    private float bulletKnockbackIncrease = 0f;

    private TestShootBullet shootAbility; // TestShootBullet will be replaced with whatever is the final basic bullet shooting ability.

    protected override void Awake()
    {
        base.Awake();
        shootAbility = player.GetComponent<TestShootBullet>();
    }

    public override void ApplyUpgrade(int quantity)
    {
        shootAbility.cooldownMultiplier += cooldownIncrease;
        shootAbility.recoilMultiplier += recoilIncrease;
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease;
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease;
    }
}
