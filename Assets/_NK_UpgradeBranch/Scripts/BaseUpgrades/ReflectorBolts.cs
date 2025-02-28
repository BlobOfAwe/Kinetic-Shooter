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


    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        player.defenseMultiplier += defenseIncrease * quantity;
        player.healthMultiplier += healthIncrease * quantity;
        //player.attackMultiplier = Mathf.Pow(shootAbility.bulletDamageMultiplier + damageIncrease, quantity);
        player.attackMultiplier += damageIncrease * quantity;
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
