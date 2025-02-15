using UnityEngine;

public class Electromagnetism : Upgrade
{
    [SerializeField]
    private float arcDamagePercent = 1f;

    [SerializeField]
    private float attackIncrease = 0.05f;

    [SerializeField]
    private float manualMoveIncrease = 0.05f;

    [SerializeField]
    private float bulletKnockbackIncrease = -0.05f;

    [SerializeField]
    private float maxArcRadius = 1f;

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
        player.attackMultiplier += attackIncrease * quantity;
        player.speedMultiplier += manualMoveIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier = Mathf.Pow(shootAbility.bulletKnockbackMultiplier + bulletKnockbackIncrease, quantity);

        // Debug - remove later.
        FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (target.GetComponent<Enemy>() != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(target.transform.position, maxArcRadius);
            Enemy closestEnemy = null;
            float closestDistance = float.PositiveInfinity;
            for (int i = 0; i < colliders.Length; i++)
            {
                if ((colliders[i].GetComponent<Enemy>() != null) && (colliders[i].gameObject != target))
                {
                    if (Vector2.Distance(colliders[i].ClosestPoint(target.transform.position), target.transform.position) < closestDistance)
                    {
                        closestEnemy = colliders[i].GetComponent<Enemy>();
                        closestDistance = Vector2.Distance(colliders[i].ClosestPoint(target.transform.position), target.transform.position);
                    }
                }
            }
            Color arcZoneColor = Color.red;
            if (closestEnemy != null)
            {
                closestEnemy.Damage(player.totalAttack * bullet.damageMultiplier * quantity * arcDamagePercent);
                Debug.Log("Dealt " + (player.totalAttack * bullet.damageMultiplier * quantity * arcDamagePercent) + " damage to " + closestEnemy.name);
                Debug.DrawLine(target.transform.position, closestEnemy.transform.position, Color.yellow, 1f);
                arcZoneColor = Color.green;
            } else
            {
                Debug.Log("No nearby enemies");
                arcZoneColor = Color.red;
            }

            Debug.DrawRay(target.transform.position, Vector2.up * maxArcRadius, arcZoneColor, 1f);
            Debug.DrawRay(target.transform.position, Vector2.down * maxArcRadius, arcZoneColor, 1f);
            Debug.DrawRay(target.transform.position, Vector2.left * maxArcRadius, arcZoneColor, 1f);
            Debug.DrawRay(target.transform.position, Vector2.right * maxArcRadius, arcZoneColor, 1f);
        }
    }
}
