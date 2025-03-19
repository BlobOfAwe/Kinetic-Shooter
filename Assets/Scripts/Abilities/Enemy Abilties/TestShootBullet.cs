// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootBullet : Ability
{
    [SerializeField] GameObject bulletPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;
    public float recoil = 1; // Changed to public so it can be accessed by upgrades. - NK
    public float recoilMultiplier = 1f; // Added this so that recoil can be multiplied by upgrades. - NK

    // Added multipliers for bullet speed and knockback to be manipulated with upgrades. - NK
    public float bulletSpeedMultiplier = 1f;
    public float bulletKnockbackMultiplier = 1f;

    // Added firePoint to shoot from instead of this gameObject. - NK
    [SerializeField]
    private Transform firePoint;
    private GameObject[] bullets;
    private Rigidbody2D rb;

    // Populate the array bullets with instances of bulletPrefab
    new private void Awake()
    {
        base.Awake();
        bullets = new GameObject[maxBullets];
        for (int i = 0; i < bullets.Length; i++) 
        { 
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].GetComponent<Projectile>().shooter = firePoint.gameObject; // Changed to set bullets' shooters to firePoint instead of this gameObject. - NK
            bullets[i].SetActive(false); 
        }
    }

    private void Start()
    {
        try { rb = GetComponent<Rigidbody2D>(); }
        catch { Debug.LogError("No Rigidbody attatched to " + gameObject.name + ". Knockback and other physics cannot be applied."); }
    }

    // Shoot a bullet from the gameObject's position
    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());

        AudioManager.instance.PlayOneShot(FMODEvents.instance.turretShotAbility, this.transform.position);

        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf) {  // If the bullet is not active (being fired) 
                bullet.transform.position = firePoint.position; // Set the bullet to firePoint's position - changed from transform.position - NK
                bullet.transform.eulerAngles = firePoint.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                rb.AddForce(-firePoint.up * recoil * recoilMultiplier, ForceMode2D.Impulse); // Add any knockback to the object
                bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
                bullet.GetComponent<Projectile>().speedMultiplier = bulletSpeedMultiplier;
                bullet.GetComponent<Projectile>().knockbackMultiplier = bulletKnockbackMultiplier;
                bullet.SetActive(true); return; } // Set the bullet to active and return
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
