// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockfall : Ability
{
    private Animator animator;
    [SerializeField] FallingRock rockPrefab;
    // Added maxBullets instead of the max number of bullets being hard-coded. - NK
    [SerializeField]
    private int maxBullets = 10;
    [SerializeField]
    private int minRocks = 5;

    private FallingRock[] bullets;
    private Enemy thisEnemy; // An Enemy class specific version of base.thisEntity.

    // Populate the array bullets with instances of bulletPrefab
    new private void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
        bullets = new FallingRock[maxBullets];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(rockPrefab);
            bullets[i].gameObject.SetActive(false);
            bullets[i].shooter = gameObject;
            bullets[i]._INIT();
        }
        thisEnemy = GetComponent<Enemy>();
    }

    // Shoot a bullet from the gameObject's position
    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        animator.SetTrigger("isAttacking");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.turretShotAbility, this.transform.position);

        int rocks = Random.Range(minRocks, maxBullets);
        for (int i = 0; i < rocks; i++)
        {
            if (!bullets[i].gameObject.activeSelf)
            {
                Debug.Log("Revealing Rock " + i + " of " + rocks);
                FallingRock bullet = bullets[i];
                Vector2 rockfallLocalPos = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
                bullet.transform.position = (Vector2)thisEnemy.target.transform.position + rockfallLocalPos; // Set the bullet to a random location within range
                bullet.gameObject.SetActive(true);
                bullet.Fall();
            }
        }

        // If no inactive bullets were found, throw an error
        // Changed this from an error to a message because this can happen if the max number of bullets are fired at once, which isn't a problem. - NK
        Debug.Log("No instantiated bullets available to be fired from object: " + gameObject.name);
    }

    public override void PurgeDependantObjects()
    {
        foreach (FallingRock bullet in bullets) { Destroy(bullet.gameObject); }
    }
}
