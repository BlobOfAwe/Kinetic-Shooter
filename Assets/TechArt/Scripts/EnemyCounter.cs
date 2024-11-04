// ## - ZS
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    public int totalEnemies = 10;
    private int remainingEnemies;
    public TMP_Text enemyCounterText;

    void Start()
    {
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
    }

    public void EnemyDefeated()
    {
        remainingEnemies = remainingEnemies - 1;
        remainingEnemies = Mathf.Max(remainingEnemies, 0);
        UpdateEnemyCounter();

        if (remainingEnemies <= 0)
        {
            Debug.Log("An Elite Enemy Has Spawned"); //insert Elite enemy spawn
        }
    }

    private void UpdateEnemyCounter()
    {
        enemyCounterText.text = "Enemies Left: " + remainingEnemies;
    }
}