// ## - NK
using UnityEngine;

// This is a temporary script to generate a map automatically when the level is loaded.
public class AutoGenerate : MonoBehaviour
{
    private void Start()
    {
        GetComponent<MapGenerator>().Generate();
    }
}
