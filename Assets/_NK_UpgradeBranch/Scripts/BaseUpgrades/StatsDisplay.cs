using TMPro;
using UnityEngine;

// This script is for debugging.
public class StatsDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerBehaviour playerBehaviour;

    [SerializeField]
    private StandardPrimaryFire shootAbility;

    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDisplay()
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }
        textMesh.text = "Attack: " + playerBehaviour.totalAttack + " * " + playerBehaviour.attackMultiplier + " = " + (playerBehaviour.totalAttack * playerBehaviour.attackMultiplier) + "\n" +
                "Defense: " + playerBehaviour.totalDefense + " * " + playerBehaviour.defenseMultiplier + " = " + (playerBehaviour.totalDefense * playerBehaviour.defenseMultiplier) + "\n" +
                "Speed: " + playerBehaviour.totalSpeed + " * " + playerBehaviour.speedMultiplier + " = " + (playerBehaviour.totalSpeed * playerBehaviour.speedMultiplier) + "\n" +
                "Max HP: " + playerBehaviour.maxHealth + " * " + playerBehaviour.healthMultiplier + " = " + (playerBehaviour.maxHealth * playerBehaviour.healthMultiplier) + "\n" +
                "Recover: " + playerBehaviour.totalRecovery + " * " + playerBehaviour.recoveryMultiplier + " = " + (playerBehaviour.totalRecovery * playerBehaviour.recoveryMultiplier) + "\n\n" +
                "Weapon:\n" +
                "Cooldown: *" + shootAbility.cooldownMultiplier + "\n" +
                //"Recoil: " + (shootAbility.recoil / shootAbility.recoilMultiplier) + " * " + shootAbility.recoilMultiplier + " = " + shootAbility.recoil + "\n" +
                "Bullet Speed: *" + shootAbility.bulletSpeedMultiplier + "\n" +
                "Bullet Knockback: *" + shootAbility.bulletKnockbackMultiplier;
    }
}
