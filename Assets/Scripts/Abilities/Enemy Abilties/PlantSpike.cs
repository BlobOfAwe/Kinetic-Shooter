using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class PlantSpike : Ability
{
    [SerializeField] GameObject bulletPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;
    [SerializeField] private float offsetFromWall; // Offset the spike from the wall by this amount
    [SerializeField] LayerMask walls = 64;
    private GameObject[] bullets;
    private Enemy thisEnemy; // An Enemy class specific version of base.thisEntity.

    new private void Awake()
    {
        base.Awake();
        bullets = new GameObject[maxBullets];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].GetComponent<Projectile>().shooter = gameObject;
            bullets[i].SetActive(false);
        }
        thisEnemy = GetComponent<Enemy>();
    }

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        // Circlecast forward out to Range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, thisEnemy.target.position, range * 2, walls);

        // Check the Normal of the wall
        if (hit)
        {
            Debug.Log("Plant spike at normal " + hit.normal);
            // Spawn a spike projectile on that wall
            // Check for the first available inactive bullet, and activate it from this object's position
            foreach (GameObject bullet in bullets)
            {
                if (!bullet.activeSelf)
                {
                    bullet.transform.position = hit.point; // Set the bullet to firePoint's position - changed from transform.position - NK
                    Vector2 dir = new Vector2(hit.normal.x, hit.normal.y);
                    bullet.transform.up = dir;
                    bullet.transform.position += bullet.transform.up * offsetFromWall;
                    bullet.GetComponent<Projectile>().timeRemaining = bullet.GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
                    bullet.SetActive(true);
                    break;
                } // Set the bullet to active and return
            }
        }

        thisEnemy.target = null;
        thisEnemy.state = 0; // Manually set the enemy state to override the StateChange Cooldown
        Debug.Log("Set enemy state " + thisEnemy.state);
    }
}