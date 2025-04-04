using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class StandardPrimaryFire : ShootAbility
{
    [SerializeField] GameObject bulletPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;
    [SerializeField] private float recoil = 1;
    private PlayerBehaviour player;


    private Rigidbody2D rb;

    // Populate the array bullets with instances of bulletPrefab
    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        Debug.Log("Player's primary ability is " + player.primary.GetType());

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
        StartCoroutine(BeginCooldown());

        // Modified code to animate gun and play fire sound here and only here. - NK
        player.playerGunAnimator.SetTrigger("isShooting");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.standardPrimary, this.transform.position);

        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {  // If the bullet is not active (being fired)
                bullet.transform.position = (Vector2)player.firePoint.position; // Added an offset to make the bullets spawn closer to the gun - Z.S // Set the bullet to firePoint's position - changed from transform.position - NK
                bullet.transform.eulerAngles = player.firePoint.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                rb.AddForce(-player.firePoint.up * recoil, ForceMode2D.Impulse); // Add any knockback to the object
                Projectile bulletProj = bullet.GetComponent<Projectile>();
                bulletProj.timeRemaining = bulletProj.despawnTime; // Reset the bullet's despawn timer. - NK
                bulletProj.speedMultiplier = bulletSpeedMultiplier;
                bulletProj.knockbackMultiplier = bulletKnockbackMultiplier;
                bulletProj.damageMultiplier = bulletDamageMultiplier * damageModifier;
                bulletProj.effectModifier = damageModifier;
                player.ProjectileFireEffect(bullet.GetComponent<TestBullet>());
                bullet.SetActive(true); return;
            } // Set the bullet to active and return
        }

        // If no inactive bullets were found, throw an error
        // Changed this from an error to a message because this can happen if the max number of bullets are fired at once, which isn't a problem. - NK
        Debug.LogWarning("No instantiated bullets available to be fired from object: " + gameObject.name);
    }
}
