using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyPrimaryFire : Ability
{
    [SerializeField] GameObject bulletPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 30;
    [SerializeField] private float recoil = 1;
    private PlayerBehaviour player;

    // Added multipliers for bullet speed and knockback to be manipulated with upgrades. - NK
    public float bulletSpeedMultiplier = 1f;
    public float bulletKnockbackMultiplier = 1f;

    [SerializeField]
    private float burstSpacing = 0.2f; // Spacing between bullets in a burst

    [Header("Bullet 1")]
    [SerializeField] private float damageModOne;
    [SerializeField] private float recoilModOne;
    [SerializeField] private float scaleModOne;

    [Header("Bullet 2")]
    [SerializeField] private float damageModTwo;
    [SerializeField] private float recoilModTwo;
    [SerializeField] private float scaleModTwo;

    [Header("Bullet 3")]
    [SerializeField] private float damageModThree;
    [SerializeField] private float recoilModThree;
    [SerializeField] private float scaleModThree;


    private GameObject[] bullets;
    private Rigidbody2D rb;

    // Populate the array bullets with instances of bulletPrefab
    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        if (player.primary == this)
        {
            bullets = new GameObject[maxBullets];
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Instantiate(bulletPrefab);
                bullets[i].GetComponent<TestBullet>().shooter = player.firePoint.gameObject; // Changed to set bullets' shooters to firePoint instead of this gameObject. - NK
                bullets[i].SetActive(false);
            }

            try { rb = GetComponent<Rigidbody2D>(); }
            catch { Debug.LogError("No Rigidbody attatched to " + gameObject.name + ". Knockback and other physics cannot be applied."); }
        }
    }

    // Shoot a bullet from the gameObject's position
    public override void OnActivate()
    {
        available = false;
        StartCoroutine(Burst());
    }

    private IEnumerator Burst()
    {
        FireBullet(damageModOne, recoilModOne, scaleModOne);
        yield return new WaitForSeconds(burstSpacing);
        FireBullet(damageModTwo, recoilModTwo, scaleModTwo);
        yield return new WaitForSeconds(burstSpacing);
        FireBullet(damageModThree, recoilModThree, scaleModThree);
        StartCoroutine(BeginCooldown());
    }

    private void FireBullet(float damageMod, float recoilMod, float scaleMod)
    {
        // Modified code to animate gun and play fire sound here and only here. - NK
        player.playerGunAnimator.SetTrigger("isShooting");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.heavyPrimary, this.transform.position);

        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {  // If the bullet is not active (being fired)
                bullet.transform.position = player.firePoint.position; // Set the bullet to firePoint's position - changed from transform.position - NK
                bullet.transform.eulerAngles = player.firePoint.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                rb.AddForce(-player.firePoint.up * recoilMod, ForceMode2D.Impulse); // Add any knockback to the object
                bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
                bullet.GetComponent<Projectile>().speedMultiplier = bulletSpeedMultiplier;
                bullet.GetComponent<Projectile>().knockbackMultiplier = bulletKnockbackMultiplier;
                bullet.GetComponent<Projectile>().damageMultiplier *= damageMod * damageModifier; // Modify the bullet's damage by the projectile's default modifier, the modifier of the burst sequence, and the ability's damage modifier
                bullet.transform.localScale = Vector3.one * scaleMod;
                bullet.SetActive(true); return;
            } // Set the bullet to active and return
        }

        // If no inactive bullets were found, throw an error
        // Changed this from an error to a message because this can happen if the max number of bullets are fired at once, which isn't a problem. - NK
        Debug.Log("No instantiated bullets available to be fired from object: " + gameObject.name);
    }
}
