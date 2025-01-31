using UnityEngine;

public class ReflectorBolts : Upgrade
{
    [SerializeField]
    private float defenseIncrease = 0.05f;

    [SerializeField]
    private float healthIncrease = 0.05f;

    [SerializeField]
    private float damageIncrease = -0.05f;

    [SerializeField]
    private float reflectorChance = 0.5f;

    [SerializeField]
    private GameObject reflectorPrefab;

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
        player.defenseMultiplier += defenseIncrease * quantity;
        player.healthMultiplier += healthIncrease * quantity;
        player.attackMultiplier = Mathf.Pow(shootAbility.bulletDamageMultiplier + damageIncrease, quantity);

        // Debug - remove later.
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void FireUpgradeEffect(int quantity, TestBullet bullet)
    {
        Debug.Log("THIS TEXT SHOULD ALWAYS APPEAR WHEN FIRING AFTER OBTAINING REFLECTOR BOLTS!");
        if (Random.Range(0f, 1f) <= reflectorChance)
        {
            Debug.Log("This bullet is a reflector.");
            if (bullet != null)
            {
                Instantiate(reflectorPrefab, bullet.transform, false);
            } else
            {
                Debug.LogWarning("No last fired bullet.");
            }
        } else
        {
            Debug.Log("This bullet is not a reflector.");
        }
    }
}
