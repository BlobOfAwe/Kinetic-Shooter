using UnityEngine;

public class ImpalerRounds : Upgrade
{
    [SerializeField]
    private float cooldownIncrease = -0.05f;

    [SerializeField]
    private float bulletKnockbackIncrease = 0.05f;

    [SerializeField]
    private float bulletSpeedIncrease = -0.05f;

    [SerializeField]
    private GameObject piercingBullet;



    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        //shootAbility.cooldownMultiplier = Mathf.Pow(shootAbility.cooldownMultiplier + cooldownIncrease, quantity);
        shootAbility.cooldownMultiplier += cooldownIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        //shootAbility.bulletSpeedMultiplier = Mathf.Pow(shootAbility.bulletSpeedMultiplier + bulletSpeedIncrease, quantity);
        shootAbility.bulletSpeedMultiplier += bulletSpeedIncrease * quantity;
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {

        //Instantiate(piercingBullet, bullet.transform.position, Quaternion.identity);
        if (target.GetComponent<Entity>() != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.impalerGun, this.transform.position);
            if (!bullet.isPiercing)
            {
                bullet.isPiercing = true;
                bullet.hits = quantity + 1;
                //Debug.Log("hits: " + bullet.hits);
            }
        }
    }
}
