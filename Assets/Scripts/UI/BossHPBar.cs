using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Image healthFill;
    public TextMeshProUGUI healthText;
    public GameObject healthBarContainer;

    private void Update()
    {
        if (BossTracker.Instance.HasActiveBosses())
        {
            healthBarContainer.SetActive(true);
            float currentHealth = BossTracker.Instance.GetTotalCurrentHealth();
            float maxHealth = BossTracker.Instance.GetTotalMaxHealth();

            if (maxHealth > 0)
            {
                healthFill.fillAmount = currentHealth / maxHealth;
                healthText.text = $"{Mathf.RoundToInt((currentHealth / maxHealth) * 100)}%";
            }
            else
            {
                healthFill.fillAmount = 0;
                healthText.text = "0%";
            }
        }
        else
        {
            healthBarContainer.SetActive(false);
        }
    }
}