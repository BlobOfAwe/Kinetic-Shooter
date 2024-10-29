using TMPro;
using UnityEngine;

public class TestBuildHealthDisplay : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Health: " + player.health;
    }
}
