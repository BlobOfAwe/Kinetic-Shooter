// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteShot : Ability
{
    [SerializeField] GameObject bulletPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 2;
    public float recoil = 1; // Changed to public so it can be accessed by upgrades. - NK
    public float recoilMultiplier = 1f; // Added this so that recoil can be multiplied by upgrades. - NK

    // Added multipliers for bullet speed and knockback to be manipulated with upgrades. - NK
    public float bulletSpeedMultiplier = 1f;
    public float bulletKnockbackMultiplier = 1f;

    [SerializeField] float windup = 3;

    [SerializeField] private Transform firePoint;
    private GameObject[] bullets;
    private Rigidbody2D rb;
    private Buff speedDebuff;
    private LineRenderer lineRender;
    private Enemy thisEnemy;
    private Animator animator;

    // Populate the array bullets with instances of bulletPrefab
    new private void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
        bullets = new GameObject[maxBullets];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].GetComponent<Projectile>().shooter = firePoint.gameObject; // Changed to set bullets' shooters to firePoint instead of this gameObject. - NK
            bullets[i].SetActive(false);
        }

        // Construct the speed debuff to immobilize the player while curled
        speedDebuff = ScriptableObject.CreateInstance<Buff>();
        speedDebuff.buffType = Buff.buffCategory.SPEED_BUFF;
        speedDebuff.modification = Buff.modificationType.Multiplicative;
        speedDebuff.value = 0;
    }

    private void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        thisEnemy = (Enemy)thisEntity;
        
        if (lineRender == null) { Debug.LogError("No LineRenderer attached to " + gameObject.name); }
        if (rb == null) { Debug.LogError("No Rigidbody attatched to " + gameObject.name + ". Knockback and other physics cannot be applied."); }
    }

    protected override void Update()
    {
        if (inUse)
        {            
            lineRender.SetPosition(0, firePoint.position);
            lineRender.SetPosition(1, firePoint.position + firePoint.up * range);
        }
    }

    // Shoot a bullet from the gameObject's position
    public override void OnActivate()
    {
        thisEnemy.state = 2;
        Vector2 dir = new Vector2(thisEnemy.target.transform.position.x - transform.position.x, thisEnemy.target.transform.position.y - transform.position.y);
        thisEnemy.transform.up = dir;
        inUse = true;
        available = false;
        thisEntity.ApplyBuff(speedDebuff);
        StartCoroutine(TelegraphShot());
    }

    private IEnumerator TelegraphShot()
    {
        lineRender.enabled = true;
        yield return new WaitForSeconds(windup);
        inUse = false;
        lineRender.enabled = false;
        Shoot();
        thisEntity.speedBuffs.Remove(speedDebuff);
        thisEnemy.state = 0;
        StartCoroutine(BeginCooldown());
    }

    private void Shoot()
    {
        animator.SetTrigger("isAttacking");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bruteShoot, this.transform.position);

        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {  // If the bullet is not active (being fired)
                bullet.transform.position = firePoint.position; // Set the bullet to firePoint's position - changed from transform.position - NK
                bullet.transform.eulerAngles = firePoint.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                rb.AddForce(-firePoint.up * recoil * recoilMultiplier, ForceMode2D.Impulse); // Add any knockback to the object
                bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
                bullet.GetComponent<Projectile>().speedMultiplier = bulletSpeedMultiplier;
                bullet.GetComponent<Projectile>().knockbackMultiplier = bulletKnockbackMultiplier;
                bullet.SetActive(true); return;
            } // Set the bullet to active and return
        }

        // If no inactive bullets were found, throw an error
        // Changed this from an error to a message because this can happen if the max number of bullets are fired at once, which isn't a problem. - NK
        Debug.Log("No instantiated bullets available to be fired from object: " + gameObject.name);
    }

    public override void PurgeDependantObjects()
    {
        foreach (GameObject bullet in bullets) { Destroy(bullet); }
    }
}
