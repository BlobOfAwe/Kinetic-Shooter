using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disgorge : Ability
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int amount;
    [SerializeField]
    private Animator bossSpawnAnimator;
    [SerializeField]
    private Transform spawnPoint;

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        for (int i = 0; i < amount; i++)
        {
            bossSpawnAnimator.SetTrigger("isSpawning");
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoint.position, Quaternion.identity);
        }
    }
}
