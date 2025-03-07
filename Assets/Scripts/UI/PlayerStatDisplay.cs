using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerBehaviour player;
    [SerializeField]
    private TMP_Text maxHealth;
    [SerializeField]
    private TMP_Text maxAttack;
    [SerializeField]
    private TMP_Text maxDefense;
    [SerializeField]
    private TMP_Text maxSpeed;
    [SerializeField]
    private TMP_Text maxRecovery;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (Transform child in transform)
            {
                bool currentState = child.gameObject.activeSelf;
                child.gameObject.SetActive(!currentState);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        maxHealth.text = "Max Health: " + player.maxHealth.ToString();
        maxAttack.text = "Total Attack: " + player.totalAttack.ToString();
        maxDefense.text = "Total Defense: " + player.totalDefense.ToString();
        maxSpeed.text = "Total Speed: " + player.totalSpeed.ToString();
        maxRecovery.text = "Total Recovery: " + player.totalRecovery.ToString();
    }
}
