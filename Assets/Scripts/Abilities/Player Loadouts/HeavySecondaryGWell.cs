using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySecondaryGWell : Ability
{
    [SerializeField] GameObject bulletPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;
    [SerializeField] private float recoil = 1;
    private PlayerBehaviour player;

    // Added multipliers for bullet speed and knockback to be manipulated with upgrades. - NK
    public float bulletSpeedMultiplier = 1f;
    public float bulletKnockbackMultiplier = 1f;


    private GameObject[] bullets;
    private Rigidbody2D rb;

    // Populate the array bullets with instances of bulletPrefab
    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        if (player.secondary == this)
        {
            bullets = new GameObject[maxBullets];
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Instantiate(bulletPrefab);
                bullets[i].GetComponent<Projectile>().shooter = player.firePoint.gameObject; // Changed to set bullets' shooters to firePoint instead of this gameObject. - NK
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.heavySecondary, this.transform.position);

        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {  // If the bullet is not active (being fired)
                bullet.transform.position = player.firePoint.position; // Set the bullet to firePoint's position - changed from transform.position - NK
                bullet.transform.eulerAngles = player.firePoint.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                rb.AddForce(-player.firePoint.up * recoil, ForceMode2D.Impulse); // Add any knockback to the object
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
}
