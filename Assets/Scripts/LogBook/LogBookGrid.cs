using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogBookGrid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] 
    private Image iconImage;
    [SerializeField] 
    private GameObject lockedOverlay;

    public string EntryID { get; private set; }

    public void Initialize(Sprite icon, string name, string entryID, bool isUnlocked)
    {
        EntryID = entryID;
        UpdateAppearance(icon, name, isUnlocked);

        GetComponent<Button>().onClick.AddListener(() => {
            LogBookManager.Instance.ShowEntryDetails(EntryID);
        });
    }

    public void UpdateAppearance(Sprite icon, string name, bool isUnlocked)
    {
        iconImage.sprite = icon;
        lockedOverlay.SetActive(!isUnlocked);
    }
}