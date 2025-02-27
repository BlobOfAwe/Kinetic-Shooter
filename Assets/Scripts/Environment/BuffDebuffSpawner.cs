// ## - JV
using FMODUnity;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enemy spawner is modeled after the Combat Director spawn system from Risk of Rain 2
/// https://riskofrain2.fandom.com/wiki/Directors#The_Combat_Directors
/// </summary>
public class BuffDebuffSpawner : MonoBehaviour
{
    public bool active;

    [SerializeField] private float addCreditsPerSecond;
    [SerializeField] private float maxSpawnTimeInterval;
    [SerializeField] private Item[] buffPrefabs;
    [SerializeField] private float timeToNextSpawn;
    [SerializeField] private float credits;
    [SerializeField] private Item selectedBuff;
    [SerializeField] private bool lastSpawnSuccess;
    [SerializeField] private float costToSpawn; // All generic buffs and debuffs have the same spawn cost

    [SerializeField] private float spawnRange;

    [SerializeField] Vector2 spawnPosition;
    [SerializeField] private PlayerBehaviour player;


    //audio emitter variable
    protected StudioEventEmitter emitter;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerBehaviour>();
    }

    private void Update()
    {
        if (player == null) { player = FindAnyObjectByType<PlayerBehaviour>(); }
        
        if (active)
        {
            if (timeToNextSpawn <= 0)
            {
                selectedBuff = buffPrefabs[Random.Range(0, buffPrefabs.Length)];

                lastSpawnSuccess = AttemptSpawn(selectedBuff);

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
    private bool AttemptSpawn(Item buff)
    {
        // If the enemy is too expensive, fail the spawn
        if (costToSpawn > credits) { 
            //Debug.Log("Failed to spawn buff " + buff + " of cost " + costToSpawn); 
            return false; }

        // Otherwise, spawn the enemy and subtract the cost
        credits -= costToSpawn;
        Spawn(buff);
        return true;
    }
    public void Spawn(Item buffPrefab)
    {
        spawnPosition.x = player.transform.position.x + Random.Range(-spawnRange, spawnRange);
        spawnPosition.y = player.transform.position.y + Random.Range(-spawnRange, spawnRange);

        Item buff = Instantiate(buffPrefab, spawnPosition, Quaternion.identity);

        GraphNode node = AstarPath.active.GetNearest(buff.transform.position, NNConstraint.Default).node;
        buff.gameObject.transform.position = (Vector3)node.position;
        AstarPath.active.Scan();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(player.transform.position, Vector2.one * spawnRange * 2);
    }
}
