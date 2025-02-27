using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This script manages the logbook entries and the navigation between the different entries whilst handling the locked and unlocked logic and updating them in the UI accordingly.
/// </summary>
public class LogBookManager : MonoBehaviour
{

    public static LogBookManager Instance;

    [Header("Entries")]
    [SerializeField]
    private LogEntry[] allEntries;

    [Header("Ui Entries Button")]
    [SerializeField]
    private LogBookGrid[] gridButtons;

    [Header("Detail View")]
    [SerializeField]
    private GameObject detailPanel;
    [SerializeField]
    private TMP_Text entryName;
    [SerializeField]
    private Image entryImage;
    [SerializeField]
    private Sprite lockedIcon;
    [SerializeField]
    private Sprite lockedImage;
    [SerializeField]
    private TMP_Text entryDescription;

    private Dictionary<string, LogEntry> entryDictionary = new Dictionary<string, LogEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeDictionary();
            InitializeManualGrid();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionary()
    {
        foreach (var entry in allEntries)
        {
            entryDictionary.Add(entry.entryID, entry);
        }
    }
    public void InitializeManualGrid()
    {
        for (int i = 0; i < gridButtons.Length; i++)
        {
            if (i < allEntries.Length)
            {
                LogEntry entry = allEntries[i];
                gridButtons[i].Initialize(
                    entry.isUnlocked ? entry.entryIcon : lockedIcon,
                    entry.isUnlocked ? entry.entryName : "???",
                    entry.entryID,
                    entry.isUnlocked
                );
            }
            else
            {
                gridButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowEntryDetails(string entryID)
    {
        if (entryDictionary.TryGetValue(entryID, out LogEntry entry))
        {
            detailPanel.SetActive(true);
            entryName.text = entry.isUnlocked ? entry.entryName : "???";
            entryImage.sprite = entry.isUnlocked ? entry.entryImage : lockedImage;
            entryDescription.text = entry.isUnlocked ?
                entry.description :
                $"Unlock by {entry.unlockCondition}";
        }
    }

    public void UnlockEntry(string entryID)
    {
        if (entryDictionary.TryGetValue(entryID, out LogEntry entry))
        {
            entry.isUnlocked = true;
            RefreshGridItem(entryID);
        }
    }

    private void RefreshGridItem(string entryID)
    {
        foreach (var gridItem in gridButtons)
        {
            if (gridItem.EntryID == entryID)
            {
                LogEntry entry = entryDictionary[entryID];
                gridItem.UpdateAppearance(
                    entry.entryIcon,
                    entry.entryName,
                    entry.isUnlocked
                );
                break;
            }
        }
    }
}