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

    private StandardPrimaryFire shootAbility;

    protected override void Awake()
    {
        base.Awake();
        shootAbility = player.GetComponent<StandardPrimaryFire>();

        // Debug - remove later.
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ApplyUpgrade(int quantity)
    {
        shootAbility.cooldownMultiplier = Mathf.Pow(shootAbility.cooldownMultiplier + cooldownIncrease, quantity);
        shootAbility.bulletKnockbackMultiplier += bulletKnockbackIncrease * quantity;
        shootAbility.bulletSpeedMultiplier = Mathf.Pow(shootAbility.bulletSpeedMultiplier + bulletSpeedIncrease, quantity);

        // Debug - remove later.
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        //Instantiate(piercingBullet, bullet.transform.position, Quaternion.identity);
        if (target.GetComponent<Entity>() != null)
        {
            if (!bullet.isPiercing)
            {
                bullet.isPiercing = true;
                bullet.hits = quantity + 1;
                Debug.Log("hits: " + bullet.hits);
            }
        }
    }
}
