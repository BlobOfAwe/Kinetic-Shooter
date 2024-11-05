// ## - NK
using TMPro;
using UnityEngine;

public class TestBuildHealthDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerBehaviour player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerBehaviour>();
    }

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Health: " + player.health;
    }
}
