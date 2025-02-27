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
    private float extraBulletDamageMultiplier = 0.5f;

    private StandardPrimaryFire shootAbility;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        shootAbility = player.GetComponent<StandardPrimaryFire>();
        player.healthMultiplier += healthIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += knockbackIncrease * quantity;
        shootAbility.cooldownMultiplier += cooldownIncrease * quantity;
        player.defenseMultiplier = Mathf.Pow(player.defenseMultiplier + defenseIncrease, quantity);

        // Debug - remove later.
        //FindObjectOfType<StatsDisplay>().UpdateDisplay();
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
                    bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer.
                    bullet.GetComponent<Projectile>().speedMultiplier = shootAbility.bulletSpeedMultiplier;
                    bullet.GetComponent<Projectile>().knockbackMultiplier = shootAbility.bulletKnockbackMultiplier;
                    bullet.GetComponent<Projectile>().damageMultiplier = shootAbility.bulletDamageMultiplier * extraBulletDamageMultiplier;
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
