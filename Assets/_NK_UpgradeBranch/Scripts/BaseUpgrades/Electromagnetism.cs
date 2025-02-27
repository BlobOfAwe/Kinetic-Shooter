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

    [SerializeField]
    private GameObject arcLine;

    [SerializeField]
    private float arcLineTime = 1f;

    private StandardPrimaryFire shootAbility;


    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        shootAbility = player.GetComponent<StandardPrimaryFire>();
        player.attackMultiplier += attackIncrease * quantity;
        player.speedMultiplier += manualMoveIncrease * quantity;
        shootAbility.bulletKnockbackMultiplier = Mathf.Pow(shootAbility.bulletKnockbackMultiplier + bulletKnockbackIncrease, quantity);

        // Debug - remove later.
        //FindObjectOfType<StatsDisplay>().UpdateDisplay();
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (target.GetComponent<Enemy>() != null)
        {
            Arc(bullet, target, quantity, quantity);
        }
    }

    private void Arc(TestBullet bullet, GameObject source, int quantity, int arcs)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(source.transform.position, maxArcRadius);
        Enemy closestEnemy = null;
        float closestDistance = float.PositiveInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            if ((colliders[i].GetComponent<Enemy>() != null) && (colliders[i].gameObject != source))
            {
                if (Vector2.Distance(colliders[i].ClosestPoint(source.transform.position), source.transform.position) < closestDistance)
                {
                    closestEnemy = colliders[i].GetComponent<Enemy>();
                    closestDistance = Vector2.Distance(colliders[i].ClosestPoint(source.transform.position), source.transform.position);
                }
            }
        }
        Color arcZoneColor = Color.red;
        if (closestEnemy != null)
        {
            closestEnemy.Damage(player.totalAttack * bullet.damageMultiplier * quantity * arcDamagePercent);
            Instantiate(arcLine).GetComponent<ArcLine>().SetParameters(source, closestEnemy.gameObject, arcLineTime);
            //Debug.Log("Arc from " + source.name + " to " + closestEnemy.name + ", dealing " + (player.totalAttack * bullet.damageMultiplier * quantity * arcDamagePercent) + " damage.");
            Debug.DrawLine(source.transform.position, closestEnemy.transform.position, Color.yellow, arcLineTime);
            arcZoneColor = Color.green;
            arcs -= 1;
            if (arcs > 0)
            {
                Arc(bullet, closestEnemy.gameObject, quantity, arcs);
            }
        }
        else
        {
            //Debug.Log("No nearby enemies");
            arcZoneColor = Color.red;
        }

        Debug.DrawRay(source.transform.position, Vector2.up * maxArcRadius, arcZoneColor, 1f);
        Debug.DrawRay(source.transform.position, Vector2.down * maxArcRadius, arcZoneColor, 1f);
        Debug.DrawRay(source.transform.position, Vector2.left * maxArcRadius, arcZoneColor, 1f);
        Debug.DrawRay(source.transform.position, Vector2.right * maxArcRadius, arcZoneColor, 1f);
    }
}
