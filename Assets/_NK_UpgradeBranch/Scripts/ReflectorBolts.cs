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

    [SerializeField]
    private Vector2 reflectorOffset = Vector2.zero;

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
        if (Random.Range(0f, 1f) <= reflectorChance)
        {
            if (bullet != null)
            {
                GameObject reflector = Instantiate(reflectorPrefab, bullet.transform, false);
                reflector.transform.Translate(reflectorOffset);
                reflector.GetComponent<Reflector>().reflects = quantity;
            }
        }
    }
}
