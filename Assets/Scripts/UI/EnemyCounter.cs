// ## - ZS
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    // Changed public variables that don't need to be public but need to be set in the inspector to private and added [SerializeField].
    [SerializeField]
    private int totalEnemies = 10;
    private float remainingEnemies;
    [SerializeField]
    private int bossSpawningCredits; // How many credits are given to the boss spawner
    [SerializeField]
    private float remainingBosses;
    [SerializeField]
    private Enemy[] bossEnemies; // Added by NK.
    [SerializeField]
    private float bossXOffsetMin; // Added by NK.
    [SerializeField]
    private float bossXOffsetMax; // Added by NK.
    [SerializeField]
    private float bossYOffsetMin; // Added by NK.
    [SerializeField]
    private float bossYOffsetMax; // Added by NK.
    [SerializeField]
    private Beacon beacon; // Added by NK.
    [SerializeField]
    private TMP_Text enemyCounterText;
    [SerializeField]
    private GameObject beaconIndicator;
    private bool bossIsSpawned = false; // Added by NK.

    // audio parameter controller script
    [SerializeField] AudioParameterController parameterController;

    //private bool increaseAudioIntensity = false; // Grey
    //private bool increaseAudioStage = false; // Grey

    void Start()
    {
        totalEnemies = Mathf.RoundToInt(totalEnemies * (1 + (GameManager.difficultyCoefficient * 0.5f))); // Scale the number of enemies based on the difficulty
        remainingEnemies = totalEnemies;
        bossSpawningCredits = GameManager.bossSpawnCreditsForLevel[GameManager.currentLevel];
        UpdateEnemyCounter();
    }

    // Debug
    void Update()
    {
        //Audio Intensity
        if (remainingEnemies == 18 || remainingEnemies == 17 || remainingEnemies == 16)
        {
            parameterController.IntensityOne();
        }
        else if (remainingEnemies == 15 || remainingEnemies == 14 || remainingEnemies == 13)
        {
            parameterController.IntensityTwo();
            parameterController.StageOne();
        }
        else if (remainingEnemies == 10 || remainingEnemies == 9 || remainingEnemies == 8)
        {
            parameterController.IntensityThree();
            parameterController.StageTwo();
        }
        else if (remainingEnemies == 5 || remainingEnemies == 4 || remainingEnemies == 3)
        {
            parameterController.StageThree();
        }
        else if (remainingEnemies == 0)
        {
            parameterController.StageFour();
        }

    }

    public void EnemyDefeated(Enemy enemy)
    {
        remainingEnemies -= enemy.enemyCounterValue;
        remainingEnemies = Mathf.Max(remainingEnemies, 0);
        UpdateEnemyCounter();
        if (remainingEnemies == 0)
        {
            //Debug.Log("An Elite Enemy Has Spawned");
            beaconIndicator.GetComponent<Image>().enabled = true;
            if (beacon != null && !beacon.active) { beacon.Activate(); }// Added by Nathaniel Klassen.
            
        }
    }

    public void BossDefeated(Enemy enemy)
    {
        remainingBosses -= enemy.enemyCounterValue;
        remainingBosses = Mathf.Max(remainingBosses, 0);
        UpdateEnemyCounter();

        if (remainingBosses <= 0)
        {
            FindObjectOfType<Forcefield>().Deactivate();
            FindObjectOfType<Beacon>().levelIsFinished = true; // temporary
        }
    }

    // Method created by Nathaniel Klassen.
    public void SpawnBoss()
    {
        remainingBosses = 0;
        if (!bossIsSpawned)
        {
            for (int i = 0; i < bossEnemies.Length;)
            {
                bool success = AttemptSpawn(bossEnemies[i]);
                if (!success) { i++; }
                else { remainingBosses++; Debug.Log("Added boss to total of " + remainingBosses); }
            }

            bossIsSpawned = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bossEnemyAppear, this.transform.position);
            UpdateEnemyCounter();
        }
    }

    // Returns true if the spawn succeeded, false if it failed
    private bool AttemptSpawn(Enemy enemy)
    {
        // If the enemy is too expensive, fail the spawn
        if (enemy.spawnCost > bossSpawningCredits)
        {
            //Debug.Log("Failed to spawn enemy " + enemy + " of cost " + enemy.spawnCost); 
            return false;
        }

        // Otherwise, spawn the enemy and subtract the cost
        bossSpawningCredits -= enemy.spawnCost;
        Spawn(enemy);
        return true;
    }
    public void Spawn(Enemy enemyPrefab)
    {
        Vector2 bossOffset;
        bossOffset.x = Random.Range(bossXOffsetMin, bossXOffsetMax);
        bossOffset.y = Random.Range(bossYOffsetMin, bossYOffsetMax);
        GraphNode node = AstarPath.active.GetNearest((Vector2)beacon.transform.position + bossOffset, NNConstraint.Default).node;
        Enemy enemy = Instantiate(enemyPrefab, (Vector3)node.position, Quaternion.identity);
        enemy.gameObject.transform.position = (Vector3)node.position;
        AstarPath.active.Scan();
    }

    private void UpdateEnemyCounter()
    {
        if (!bossIsSpawned)
            enemyCounterText.text = "Enemies Left: " + Mathf.Ceil(remainingEnemies);
        else
            enemyCounterText.text = "Bosses Left: " + remainingBosses;
    }
}