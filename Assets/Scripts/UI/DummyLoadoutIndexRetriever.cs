//ZS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLoadoutIndexRetriever : MonoBehaviour
{

    void Start()
    {
        int selectedLoadoutIndex = PlayerPrefs.GetInt("SelectedLoadoutIndex", 0);
        // using debuglog to test the presistence
        Debug.Log($"Saved Loadout Index From previous scene is: {selectedLoadoutIndex}");
    }
}
