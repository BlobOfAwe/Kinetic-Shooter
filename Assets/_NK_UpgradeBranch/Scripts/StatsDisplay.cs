using TMPro;
using UnityEngine;

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
        textMesh.text = "Attack: " + (playerBehaviour.totalAttack / playerBehaviour.attackMultiplier) + " * " + playerBehaviour.attackMultiplier + " = " + playerBehaviour.totalAttack + "\n" +
                "Defense: " + (playerBehaviour.totalDefense / playerBehaviour.defenseMultiplier) + " * " + playerBehaviour.defenseMultiplier + " = " + playerBehaviour.totalDefense + "\n" +
                "Speed: " + (playerBehaviour.totalSpeed / playerBehaviour.speedMultiplier) + " * " + playerBehaviour.speedMultiplier + " = " + playerBehaviour.totalSpeed + "\n" +
                "Max HP: " + (playerBehaviour.maxHealth / playerBehaviour.healthMultiplier) + " * " + playerBehaviour.healthMultiplier + " = " + playerBehaviour.maxHealth + "\n" +
                "Recover: " + (playerBehaviour.totalRecovery / playerBehaviour.recoveryMultiplier) + " * " + playerBehaviour.recoveryMultiplier + " = " + playerBehaviour.totalRecovery + "\n\n" +
                "Weapon:\n" +
                "Cooldown: *" + shootAbility.cooldownMultiplier + "\n" +
                //"Recoil: " + (shootAbility.recoil / shootAbility.recoilMultiplier) + " * " + shootAbility.recoilMultiplier + " = " + shootAbility.recoil + "\n" +
                "Bullet Speed: *" + shootAbility.bulletSpeedMultiplier + "\n" +
                "Bullet Knockback: *" + shootAbility.bulletKnockbackMultiplier;
    }
}
