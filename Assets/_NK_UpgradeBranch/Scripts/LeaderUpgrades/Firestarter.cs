using UnityEngine;

public class Firestarter : Upgrade
{
    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (target.GetComponent<Enemy>() != null)
        {
            Debug.Log("Lit " + target.name + " on fire!");
        }
    }
}
