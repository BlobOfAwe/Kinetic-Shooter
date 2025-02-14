using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSecondaryMines : Ability
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float duration = 5;
    [SerializeField] float minSpeed = 2;

    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;

    private PlayerBehaviour player;
    private GameObject[] bullets;
    private Rigidbody2D rb;

    // Populate the array bullets with instances of bulletPrefab
    private new void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerBehaviour>();

        if (player.secondary == this)
        {
            bullets = new GameObject[maxBullets];
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Instantiate(bulletPrefab);
                bullets[i].GetComponent<Projectile>().shooter = player.gameObject; // Changed to set bullets' shooters to firePoint instead of this gameObject. - NK
                bullets[i].SetActive(false);
            }
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
        StartCoroutine(Drop() );
        
    }

    private IEnumerator Drop()
    {
        float durationTimer = duration;
        while (durationTimer > 0)
        {
            if (rb.velocity.magnitude > minSpeed)
            {
                yield return new WaitForSeconds(0.5f);
                durationTimer -= 0.5f;
                DropMine();
            }
            else { yield return new WaitForEndOfFrame(); }
        }
    }

    private void DropMine()
    {
        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {  // If the bullet is not active (being fired)
                bullet.transform.position = player.transform.position; // Set the bullet to firePoint's position - changed from transform.position - NK
                bullet.transform.eulerAngles = player.transform.eulerAngles; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
                bullet.SetActive(true); return;
            } // Set the bullet to active and return
        }

        // If no inactive bullets were found, throw an error
        // Changed this from an error to a message because this can happen if the max number of bullets are fired at once, which isn't a problem. - NK
        Debug.Log("No instantiated bullets available to be fired from object: " + gameObject.name);
    }
}
