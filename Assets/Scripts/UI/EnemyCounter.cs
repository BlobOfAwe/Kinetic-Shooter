// ## - ZS
using Pathfinding;
using TMPro;
using UnityEngine;

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

    private bool bossIsSpawned = false; // Added by NK.

    //Audio variables

    [SerializeField] private string parameterNameIntensity;
    [SerializeField] private float parameterValueIntensity;

    [SerializeField] private string parameterNameStage;
    [SerializeField] private float parameterValueStage;

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
        if (remainingEnemies == 18)
        {
            parameterValueIntensity = 1;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
        else if (remainingEnemies == 15)
        {
            parameterValueIntensity = 2;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
        else if (remainingEnemies == 13)
        {
            parameterValueIntensity = 3;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
        else if (remainingEnemies == 11)
        {
            parameterValueIntensity = 4;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
        else if (remainingEnemies == 10)
        {
            parameterValueStage = 1;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
        }
        else if (remainingEnemies == 6)
        {
            parameterValueStage = 1;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
            parameterValueIntensity = 5;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
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
            if (!beacon.active) { beacon.Activate(); }// Added by Nathaniel Klassen.
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
            parameterValueStage = 2;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
            parameterValueIntensity = 5;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);

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