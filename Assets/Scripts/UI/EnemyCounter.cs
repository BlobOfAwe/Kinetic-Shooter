// ## - ZS
using Pathfinding;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    // Changed public variables that don't need to be public but need to be set in the inspector to private and added [SerializeField].
    [SerializeField]
    private int totalEnemies = 10;
    private int remainingEnemies;
    [SerializeField]
    private GameObject bossEnemy; // Added by NK.
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
        UpdateEnemyCounter();
    }

    // Debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            EnemyDefeated();
        }
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

    public void EnemyDefeated()
    {
        remainingEnemies -= 1;
        remainingEnemies = Mathf.Max(remainingEnemies, 0);
        UpdateEnemyCounter();
        if (remainingEnemies <= 0)
        {
            Debug.Log("An Elite Enemy Has Spawned");
            if (!beacon.active) { beacon.Activate(); }// Added by Nathaniel Klassen.
        }
    }

    // Method created by Nathaniel Klassen.
    public void SpawnBoss()
    {
        if (!bossIsSpawned)
        {
            Vector2 bossOffset;
            bossOffset.x = Random.Range(bossXOffsetMin, bossXOffsetMax);
            bossOffset.y = Random.Range(bossYOffsetMin, bossYOffsetMax);
            GraphNode node = AstarPath.active.GetNearest((Vector2)beacon.transform.position + bossOffset, NNConstraint.Default).node;
            Instantiate(bossEnemy, (Vector3)node.position, Quaternion.identity);
            bossIsSpawned = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bossEnemyAppear, this.transform.position);
            parameterValueStage = 2;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
            parameterValueIntensity = 5;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);

        }
    }

    private void UpdateEnemyCounter()
    {
        enemyCounterText.text = "Enemies Left: " + remainingEnemies;
    }
}