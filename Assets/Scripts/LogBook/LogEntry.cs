using UnityEngine;
/// <summary>
/// This script is used to designate each entry in the logbook -Z.S
/// </summary>
[CreateAssetMenu(fileName = "NewLogEntry", menuName = "ScriptableObjects/LogEntry")]
public class LogEntry : ScriptableObject
{
    public string entryName;
    public Sprite entryImage;
    [TextArea] public string description;
    public bool isUnlocked;
    public string unlockCondition;
}