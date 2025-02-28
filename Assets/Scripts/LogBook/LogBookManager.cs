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
    private LogBookGrid[] gridButtons;
    [SerializeField]
    private LogBookGrid gridButtonPrefab;
    [SerializeField]
    private Transform gridButtonParent;

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

    private void Start()
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
        GameData saveData = DataManager.Instance.gameData;
        foreach (var entry in allEntries)
        {
            entryDictionary.Add(entry.entryID, entry);
            if ((int)saveData.GetType().GetField(entry.unlockVariable).GetValue(saveData) >= entry.requiredUnlockVariableValue)
                entry.isUnlocked = true;
            else
                entry.isUnlocked = false;
        }
    }
    public void InitializeManualGrid()
    {
        gridButtons = new LogBookGrid[allEntries.Length];
        for (int i = 0; i < gridButtons.Length; i++)
        {
            gridButtons[i] = Instantiate(gridButtonPrefab, gridButtonParent);
        }

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