using UnityEngine;

public class SlowShots : Upgrade
{
    [SerializeField]
    private float enemySlow = -1f;

    [SerializeField]
    private float duration = 1f;

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private float explosionDamage = 10f;

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (target.GetComponent<Enemy>() != null)
        {
            Buff buffConstructor = ScriptableObject.CreateInstance<Buff>();

            buffConstructor.buffType = Buff.buffCategory.SPEED_BUFF;
            buffConstructor.value = enemySlow;
            buffConstructor.modification = Buff.modificationType.Additive;
            buffConstructor.duration = duration;

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy.totalSpeed + enemySlow <= 0f)
            {
                buffConstructor.value = -enemy.totalSpeed;
            }
            enemy.ApplyBuff(buffConstructor);
            Debug.Log(target.name + " slowed to " + enemy.totalSpeed);
        }
    }

    public override void KillUpgradeEffect(Enemy target, int quantity)
    {
        if (target.totalSpeed <= 0f)
        {
            Debug.Log(target.name + " exploded!");
            Instantiate(explosion, target.transform.position, Quaternion.identity).GetComponent<RocketExplosion>().SetDamage(explosionDamage);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketImpactAbility, target.transform.position);
        } else
        {
            Debug.Log(target.name + " was killed with the projectile.");
        }
    }
}
