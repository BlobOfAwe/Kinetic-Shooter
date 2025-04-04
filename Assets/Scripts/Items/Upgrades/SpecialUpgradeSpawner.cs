using UnityEngine;

public class SpecialUpgradeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] leaderUpgrades;

    [SerializeField]
    private GameObject[] bossUpgrades;

    public void SpawnLeaderUpgrade(Vector2 position)
    {
        Instantiate(leaderUpgrades[Random.Range(0, leaderUpgrades.Length)], position, Quaternion.identity);
    }

    public void SpawnBossUpgrade(Vector2 position)
    {
        SpawnBossUpgrade(position, Random.Range(0, bossUpgrades.Length));
    }

    public void SpawnBossUpgrade(Vector2 position, int upgradeToSpawn)
    {
        // TODO - Figure out a way to detect which boss upgrades are already collected and prevent duplicates.
        //        For now, each boss can spawn a specific upgrade.
        Instantiate(bossUpgrades[upgradeToSpawn], position, Quaternion.identity);
    }
}
