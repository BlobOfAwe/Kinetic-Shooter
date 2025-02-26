// ## - JV
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This enemy spawner is modeled after the Combat Director spawn system from Risk of Rain 2
/// https://riskofrain2.fandom.com/wiki/Directors#The_Combat_Directors
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public bool active;

    [SerializeField] private float addCreditsPerSecond;
    [SerializeField] private float maxSpawnTimeInterval;
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private Enemy[] leaderPrefabs;
    [SerializeField] private float timeToNextSpawn;
    [SerializeField] private float credits;
    [SerializeField] private Enemy selectedEnemy;
    [SerializeField] private bool lastSpawnSuccess;
    [SerializeField] private float maxEnemiesPerSpawn;

    [SerializeField] private float maxSpawnDistance;
    [SerializeField] private float minSpawnDistance;

    [SerializeField] Vector2 spawnPosition;
    [SerializeField] private PlayerBehaviour player;

    private int[] randFlip = {-1,1}; // Used by UnityEngine.Random to choose either 1 or -1
    private void Start()
    {
        addCreditsPerSecond = addCreditsPerSecond * (1 + GameManager.difficultyCoefficient);

        player = FindAnyObjectByType<PlayerBehaviour>(FindObjectsInactive.Exclude);
    }

    private void Update()
    {
        if (active)
        {
            if (timeToNextSpawn <= 0)
            {
                if (!lastSpawnSuccess) 
                { 
                    selectedEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; 
                    if (credits > selectedEnemy.spawnCost * maxEnemiesPerSpawn) 
                    { 
                        selectedEnemy = leaderPrefabs[Random.Range(0, leaderPrefabs.Length)];    
                    }
                }

                lastSpawnSuccess = AttemptSpawn(selectedEnemy);

                // If the spawn was a success, try to spawn another enemy of the same type within the next second. Otherwise, start the process over
                timeToNextSpawn = lastSpawnSuccess ? Random.Range(0f, 1f) : Random.Range(0f, maxSpawnTimeInterval);
            }

            // Otherwise, decrease the timer and add credits
            else
            {
                timeToNextSpawn -= Time.deltaTime;
                credits += addCreditsPerSecond * Time.deltaTime;
            }
        }
    }

    // Returns true if the spawn succeeded, false if it failed
    private bool AttemptSpawn(Enemy enemy)
    {
        // If the enemy is too expensive, fail the spawn
        if (enemy.spawnCost > credits) { 
            //Debug.Log("Failed to spawn enemy " + enemy + " of cost " + enemy.spawnCost); 
            return false; }

        // Otherwise, spawn the enemy and subtract the cost
        credits -= enemy.spawnCost;
        Spawn(enemy);
        return true;
    }
    public void Spawn(Enemy enemyPrefab)
    {
        
        spawnPosition.x = player.transform.position.x + (Random.Range(minSpawnDistance, maxSpawnDistance) * randFlip[Random.Range(0,randFlip.Length)]);
        spawnPosition.y = player.transform.position.y + (Random.Range(minSpawnDistance, maxSpawnDistance) * randFlip[Random.Range(0, randFlip.Length)]);

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        GraphNode node = AstarPath.active.GetNearest(enemy.transform.position, NNConstraint.Default).node;
        enemy.gameObject.transform.position = (Vector3)node.position;
        AstarPath.active.Scan();
    }
}
