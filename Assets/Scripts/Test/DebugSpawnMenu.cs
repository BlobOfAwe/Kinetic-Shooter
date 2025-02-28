using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSpawnMenu : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Item[] items;
    private GameObject[] combinedList;
    [SerializeField] private Transform buttonHolder;
    [SerializeField] private Button buttonPrefab;
    private PlayerBehaviour player;

    private void Awake()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Button button = Instantiate(buttonPrefab, buttonHolder);
            button.GetComponent<Image>().sprite = enemies[i].sprite.sprite;
            GameObject enemy = enemies[i].gameObject;
            button.onClick.AddListener(() => { Spawn(enemy); });
        }
        for (int i = 0; i < items.Length; i++)
        {
            Button button = Instantiate(buttonPrefab, buttonHolder);
            button.GetComponent<Image>().sprite = items[i].sprite;
            GameObject item = items[i].gameObject;
            button.onClick.AddListener(() => { Spawn(item); });
        }
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerBehaviour>();
    }

    public void Spawn(GameObject obj)
    {
        Instantiate(obj, player.transform.position + Vector3.up * 3, Quaternion.identity);
    }
}
