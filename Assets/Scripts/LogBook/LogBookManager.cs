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

    [SerializeField] 
    private LogEntry[] allEntries;
    private int currentIndex;

    [SerializeField]
    private TMP_Text entryName;
    [SerializeField]
    private Image entryImage;
    [SerializeField]
    private TMP_Text descriptionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);    
        }
    }

    public void ShowLogBook()
    {
        currentIndex = 0;
        DisplayEntry();
    }

    private void DisplayEntry()
    {
        LogEntry currentEntry = allEntries[currentIndex];

        entryName.text = currentEntry.isUnlocked ? currentEntry.entryName : "???";

        entryImage.sprite = currentEntry.isUnlocked ? currentEntry.entryImage : Resources.Load<Sprite>("LockedSprite");

        descriptionText.text = currentEntry.isUnlocked ? currentEntry.description : $"Unlock by {currentEntry.unlockCondition}";
    }

    public void NextEntry()
    {
        currentIndex = (currentIndex + 1) % allEntries.Length;
        DisplayEntry();
    }

    public void PreviousEntry()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = allEntries.Length - 1;
        DisplayEntry();
    }

    public void UnlockEntry(string entryID)
    {
        var entry = System.Array.Find(allEntries, e => e.name == entryID);
        if (entry != null) entry.isUnlocked = true;
    }

}