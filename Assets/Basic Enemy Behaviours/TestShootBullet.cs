using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootBullet : Ability
{
    [SerializeField] GameObject bulletPrefab;
    private GameObject[] bullets;

    private void Awake()
    {
        bullets = new GameObject[9];
        for (int i = 0; i < bullets.Length; i++) { bullets[i] = Instantiate(bulletPrefab); bullets[i].SetActive(false); }
    }

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());

        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf) { 
                bullet.transform.position = transform.position;
                bullet.transform.eulerAngles = transform.eulerAngles;
                bullet.SetActive(true); break; }
        }

        Debug.LogError("No instantiated bullets available to be fired from object: " + gameObject.name);
    }
}
