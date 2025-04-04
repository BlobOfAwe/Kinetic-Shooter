using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : Ability
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int amount;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private Animator bossGunAnimator;

    new private void Awake()
    {
        base.Awake();
        bullets = new GameObject[amount*3]; // Create enough bullets for 3 shots to be active at a time
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].GetComponent<Projectile>().shooter = gameObject;
            bullets[i].SetActive(false);
        }
    }

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        
        // For each bullet required for the shot
        for (int i = 0; i < amount; i++)
        {
            // Check each bullet in the list
            for (int j = 0; j < bullets.Length; j++)
            {
                // If the bullet is inactive, activate it for the shot
                if (!bullets[j].activeSelf)
                {
                    //bossGunAnimator.SetTrigger("isShooting");
                    bullets[j].transform.position = transform.position;
                    bullets[j].transform.eulerAngles = transform.eulerAngles + (Vector3.forward * (360 / amount) * i);
                    bullets[j].GetComponent<Projectile>().timeRemaining = bullets[i].GetComponent<Projectile>().despawnTime; // Reset the bullet's despawn timer. - NK
                    bullets[j].SetActive(true);
                    break;
                }
            }
        }
    }

    public override void PurgeDependantObjects()
    {
        foreach (GameObject bullet in bullets) { Destroy(bullet); }
    }
}
