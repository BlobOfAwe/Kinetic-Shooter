using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTracker : MonoBehaviour
{
    public static BossTracker Instance;
    [SerializeField]
    private List<Enemy> activeBosses = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddBoss(Enemy boss)
    {
        if (!activeBosses.Contains(boss) && boss.isBoss)
        {
            activeBosses.Add(boss);
            Enemy[] childBosses = boss.GetComponentsInChildren<Enemy>(true);
            foreach (Enemy child in childBosses)
            {
                if (child != boss && child.isBoss && !activeBosses.Contains(child))
                {
                    activeBosses.Add(child);
                }
            }
        }
    }
    public void RemoveBoss(Enemy boss)
    {
        if (activeBosses.Contains(boss) && boss.isBoss)
        {
            activeBosses.Remove(boss);
        }
    }

    public float GetTotalCurrentHealth()
    {
        float total = 0f;
        foreach (Enemy boss in activeBosses)
        {
            if (boss != null)
            {
                total += Mathf.Max(boss.health, 0f);
            }
        }
        return Mathf.Max(total, 0f);
    }

    public float GetTotalMaxHealth()
    {
        float total = 0f;
        foreach (Enemy boss in activeBosses)
        {
            if (boss != null)
            {
                total += boss.maxHealth;
            }
        }
        return total;
    }

    public bool HasActiveBosses()
    {
        foreach (Enemy boss in activeBosses)
        {
            if (boss != null)
            {
                return true;
            }
        }
        return false;
    }
}