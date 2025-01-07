using UnityEngine;

public class BladeSpinner : Upgrade
{
    [SerializeField]
    private float spawnChance = 0.2f;

    [SerializeField]
    private GameObject blade;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, bool hitDamageable, int quantity)
    {
        if (Random.Range(0f, 1f) <= spawnChance * quantity)
        {
            Instantiate(blade, bullet.transform.position, Quaternion.identity);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.swooshMelee, this.transform.position);
        }
    }
}
