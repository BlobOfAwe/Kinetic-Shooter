using UnityEngine;

public class AdditionalArms : Upgrade
{
    [SerializeField]
    private float healthIncrease = 0.05f;

    [SerializeField]
    private float knockbackIncrease = 0.05f;

    [SerializeField]
    private float cooldownIncrease = 0.05f;

    [SerializeField]
    private float defenseIncrease = -0.05f;

    [SerializeField]
    private float offsetAmount = 1f;

    [SerializeField]
    private float extraBulletDamageMultiplier = 0.2f;


    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.healthMultiplier += healthIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += knockbackIncrease * quantity;
        shootAbility.cooldownMultiplier += cooldownIncrease * quantity;
        //player.defenseMultiplier = Mathf.Pow(player.defenseMultiplier + defenseIncrease, quantity);
        player.defenseMultiplier += defenseIncrease * quantity;
    }

    public override void FireUpgradeEffect(int quantity, TestBullet b)
    {
        float offset = offsetAmount;
        for (int i = 0; i < quantity; i++)
        {
            // Copied and slightly modified from StandardPrimaryFire.cs
            foreach (GameObject bullet in shootAbility.bullets)
            {
                if (!bullet.activeSelf && bullet != b.gameObject)
                {  // If the bullet is not active (being fired)
                    bullet.transform.position = player.firePoint.position; // Set the bullet to firePoint's position.
                    bullet.transform.eulerAngles = player.firePoint.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles
                    bullet.transform.Translate(Vector2.right * offset); // Offset the bullet's position.
                    Projectile bulletProj = bullet.GetComponent<Projectile>();
                    bulletProj.timeRemaining = bulletProj.despawnTime; // Reset the bullet's despawn timer.
                    bulletProj.speedMultiplier = shootAbility.bulletSpeedMultiplier;
                    bulletProj.knockbackMultiplier = shootAbility.bulletKnockbackMultiplier;
                    bulletProj.damageMultiplier = shootAbility.bulletDamageMultiplier * extraBulletDamageMultiplier;
                    bulletProj.effectModifier = shootAbility.damageModifier * extraBulletDamageMultiplier;
                    bullet.SetActive(true);
                    if (offset < 0f)
                    {
                        offset -= offsetAmount;
                    }
                    offset = -offset;
                    break;
                } // Set the bullet to active and break
            }
        }
    }
}
