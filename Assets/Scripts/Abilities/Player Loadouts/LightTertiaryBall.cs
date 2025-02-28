using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class LightTertiaryBall : Ability
{
    [Header("Withdrawn Stats")]
    [SerializeField] private float duration;
    [SerializeField] PhysicsMaterial2D bouncyMaterial;

    [Header("Walls")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float wallSpace = 2;

    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;

    private bool curled;

    private PlayerBehaviour player;
    private Rigidbody2D rb;
    private Collider2D col;

    // FOR WHITEBOX USE ONLY
    private Color baseColor;
    private SpriteRenderer sprite;
    private GameObject[] bullets;


    // Populate the array bullets with instances of bulletPrefab
    private void Start()
    {
        player = GetComponent<PlayerBehaviour>();

        // Instantiate walls
        if (player.utility == this)
        {
            bullets = new GameObject[maxBullets];
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Instantiate(bulletPrefab);
                bullets[i].SetActive(false);
            }

            // Initialize Variables
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
            baseColor = sprite.color;
            curled = false;
        }
    }

    public override void OnActivate()
    {
        if (!curled)
        {
            curled = true;
            col.sharedMaterial = bouncyMaterial;
            StartCoroutine(Curl());
            AudioManager.instance.PlayOneShot(FMODEvents.instance.lightAbility, this.transform.position);
        }
        else
        {
            // Check for the first available inactive bullet, and activate it from this object's position
            foreach (GameObject bullet in bullets)
            {
                if (!bullet.activeSelf)
                {  // If the bullet is not active (being fired)
                    bullet.transform.position = (Vector2)transform.position + rb.velocity.normalized * wallSpace; // Set the bullet to firePoint's position - changed from transform.position - NK
                    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
                    bullet.transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * angle; // Set the bullet's rotation to firePoint's rotation - changed from transform.eulerAngles - NK
                    bullet.GetComponent<Portawall>().timeRemaining = bullet.GetComponent<Portawall>().despawnTime; // Reset the bullet's despawn timer. - NK
                    bullet.SetActive(true); return;
                } // Set the bullet to active and return
            }
        }
    }

    private IEnumerator Curl()
    {
        yield return new WaitForSeconds(duration);
        curled = false;
        col.sharedMaterial = null;
        StartCoroutine(BeginCooldown());
    }
}
