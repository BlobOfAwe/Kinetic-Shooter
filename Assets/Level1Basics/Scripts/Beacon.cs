using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField]
    private Forcefield forcefield;
    public bool active;

    public void Activate()
    {
        active = true;

        // Stop the enemy spawner and find and destroy all loaded enemies - JV
        FindAnyObjectByType<EnemySpawner>().active = false;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies) { enemy.gameObject.SetActive(false); }
        
        Debug.Log("Beacon activated!");
        forcefield.gameObject.SetActive(true);
        
    }
}
