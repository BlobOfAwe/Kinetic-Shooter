using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disgorge : Ability
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int amount;

    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        for (int i = 0; i < amount; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
        }
    }
}
