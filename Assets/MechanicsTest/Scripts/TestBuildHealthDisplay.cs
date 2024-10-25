using TMPro;
using UnityEngine;

public class TestBuildHealthDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerDamage playerDamage;

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Health: " + playerDamage.health;
    }
}
