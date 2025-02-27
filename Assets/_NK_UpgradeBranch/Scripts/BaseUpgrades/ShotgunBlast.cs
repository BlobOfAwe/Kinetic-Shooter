using UnityEngine;

public class ShotgunBlast : Upgrade
{
    [SerializeField]
    private float knockbackIncrease = 0.05f;

    [SerializeField]
    private float attackIncrease = -0.05f;

    [SerializeField]
    private float cooldownIncrease = 0.05f;

    [SerializeField]
    private float offsetAngle = 10f;

    private StandardPrimaryFire shootAbility;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        shootAbility = player.GetComponent<StandardPrimaryFire>();
        shootAbility.bulletKnockbackMultiplier += knockbackIncrease * quantity;
        player.attackMultiplier = Mathf.Pow(player.attackMultiplier + attackIncrease, quantity);
        shootAbility.cooldownMultiplier += cooldownIncrease * quantity;

        // Debug - remove later.
        //FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void FireUpgradeEffect(int quantity, TestBullet b)
    {
        float offset = offsetAngle;
        for (int i = 0; i < quantity; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                foreach (GameObject bullet in shootAbility.bullets)
                {
                    // Copied and slightly modified from StandardPrimaryFire.cs
                    if (!bullet.activeSelf && bullet != b.gameObject)
                    {  // If the bullet is not active (being fired)
                        bullet.transform.position = player.firePoint.position; // Set the bullet to firePoint's position - changed from transform.position
                        bullet.transform.eulerAngles = new Vector3(0f, 0f, player.firePoint.eulerAngles.z + offset); // Set the bullet's rotation to firePoint's rotation offset by angleOffset.
                        bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer.
                        bullet.GetComponent<Projectile>().speedMultiplier = shootAbility.bulletSpeedMultiplier;
                        bullet.GetComponent<Projectile>().knockbackMultiplier = shootAbility.bulletKnockbackMultiplier;
                        bullet.GetComponent<Projectile>().damageMultiplier = shootAbility.bulletDamageMultiplier;
                        bullet.SetActive(true);
                        offset = -offset;
                        break;
                    } // Set the bullet to active and break
                }
            }
            offset += offsetAngle;
        }
        Debug.Log("Fired " + ((quantity * 2) + 1) + " projectiles.");
    }
}
