using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disgorge : Ability
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private int amount;
    [SerializeField]
    private Animator bossSpawnAnimator;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField] float localSpawnCredits = 25f;
    private float credits;

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        credits = localSpawnCredits;
        bossSpawnAnimator.SetTrigger("isSpawning");
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Enemy enemy = enemies[Random.Range(0, enemies.Length)];
        if (enemy.spawnCost > credits) { return; }
        else
        {
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
            credits -= enemy.spawnCost;
            SpawnEnemies();
        }
    }
}
