using UnityEngine;

public class SuddenBurst : Upgrade
{
    [SerializeField]
    private float burstChance = 0.2f;

    [SerializeField]
    private int fragments = 1;

    [SerializeField]
    private GameObject bulletBurst;

    private StandardPrimaryFire shootAbility;

    public override void ProjectileUpgradeEffect(TestBullet shotBullet, GameObject target, int quantity)
    {
        if (Random.Range(0f, 1f) <= burstChance * quantity && shotBullet.isBursting)
        {
            Debug.Log("Bullet with damage of " + shootAbility.bulletDamageMultiplier + " burst into " + fragments + " fragments, each one dealing " + (shootAbility.bulletDamageMultiplier / fragments) + " damage.");

            AudioManager.instance.PlayOneShot(FMODEvents.instance.shotgunGun, shotBullet.transform.position);

            for (int i = 0; i < fragments; i++)
            {
                foreach (GameObject bullet in shootAbility.bullets)
                {
                    if (!bullet.activeSelf)
                    {
                        bullet.transform.position = shotBullet.transform.position;
                        bullet.transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
                        bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime;
                        bullet.GetComponent<Projectile>().speedMultiplier = shootAbility.bulletSpeedMultiplier;
                        bullet.GetComponent<Projectile>().knockbackMultiplier = shootAbility.bulletKnockbackMultiplier;
                        bullet.GetComponent<Projectile>().damageMultiplier = shootAbility.bulletDamageMultiplier / fragments;
                        bullet.GetComponent<TestBullet>().isFromBurst = true;
                        bullet.SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    public override void FireUpgradeEffect(int quantity, TestBullet testBullet)
    {
        if (!testBullet.isBursting)
        {
            testBullet.isBursting = true;
        }
    }
}
