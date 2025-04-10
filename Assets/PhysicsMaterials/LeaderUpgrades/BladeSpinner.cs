using FMODUnity;
using UnityEngine;

public class BladeSpinner : Upgrade
{
    [SerializeField]
    private float spawnChance = 0.2f;

    [SerializeField]
    private float bladeDamageMod = 0.2f;

    [SerializeField]
    private GameObject blade;

    //audio emitter variable
    //protected StudioEventEmitter emitter;

    /*public void Start()
    {
        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.itemApproach, this.gameObject);
        emitter.Play();
    }*/

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
    }

    public override void ProjectileUpgradeEffect(TestBullet bullet, GameObject target, int quantity)
    {
        if (Random.Range(0f, 1f) <= spawnChance * quantity * bullet.effectModifier)
        {
            Instantiate(blade, bullet.transform.position, Quaternion.identity).GetComponent<SpinningBlade>().SetDamage(player.totalAttack * bullet.damageMultiplier * bladeDamageMod);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bladeSpinner, this.transform.position);
        }
    }
}
