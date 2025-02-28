using UnityEngine;
/// <summary>
/// This script is used to designate each entry in the logbook -Z.S
/// </summary>
[CreateAssetMenu(fileName = "NewLogEntry", menuName = "ScriptableObjects/LogEntry")]
public class LogEntry : ScriptableObject
{
    public string entryID;
    public string entryName;
    public Sprite entryImage;
    public Sprite entryIcon;
    [TextArea] public string description;
    public bool isUnlocked;
    public string unlockCondition;
    public string unlockVariable;
    public int requiredUnlockVariableValue;
}