using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootBullet : Ability
{
    [SerializeField] GameObject bulletPrefab;
    private GameObject[] bullets;

    // Populate the array bullets with instances of bulletPrefab
    private void Awake()
    {
        bullets = new GameObject[9];
        for (int i = 0; i < bullets.Length; i++) 
        { 
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].GetComponent<TestBullet>().shooter = gameObject;
            bullets[i].SetActive(false); 
        }
    }

    // Shoot a bullet from the gameObject's position
    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());

        // Check for the first available inactive bullet, and activate it from this object's position
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf) {  // If the bullet is not active (being fired)
                bullet.transform.position = transform.position; // Set the bullet to my position
                bullet.transform.eulerAngles = transform.eulerAngles; // Set the bullet's rotation to my rotation
                bullet.SetActive(true); return; } // Set the bullet to active and return
        }

        // If no inactive bullets were found, throw an error
        Debug.LogError("No instantiated bullets available to be fired from object: " + gameObject.name);
    }
}
