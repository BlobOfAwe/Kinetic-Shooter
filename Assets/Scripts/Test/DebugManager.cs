// ## - JV
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// This code was taken in its entirety (with some minor alterations) from the following source:
/// CITATION: VanderMeulen, James (2024). GAME 380. Trinity Western University
/// With reference to the following source:
/// CITATION: Stufco. (2023). How to Make a Runtime Debug Console in Unity. Youtube. https://www.youtube.com/watch?v=ySzxcS2_feg
/// </summary>

public class DebugManager : MonoBehaviour
{
    private string[] logs = new string[9];
    [SerializeField] float updateTime = 0.1f;
    private float updateCounter = 0f;
    [SerializeField] InputActionReference debugToggle;
    [SerializeField] Text logText;
    [SerializeField] Text fps;


    private void Update()
    {
        if (debugToggle.action.triggered) { ToggleDebug(); }

        // Update the FPS every updateTime seconds, based on the framerate on that Frame
        updateCounter += Time.deltaTime;
        if (updateCounter > updateTime)
        {
            fps.text = "FPS: " + Mathf.Round(1 / Time.deltaTime).ToString();
            updateCounter -= updateTime;
        }
    }
    void OnEnable() { Application.logMessageReceived += ConsoleLog; }
    void OnDisable() { Application.logMessageReceived -= ConsoleLog; }

    void ToggleDebug()
    {
        GameManager.debugEnabled = !GameManager.debugEnabled;

        logText.gameObject.SetActive(GameManager.debugEnabled);
        logText.transform.parent.gameObject.SetActive(GameManager.debugEnabled);
        fps.gameObject.SetActive(GameManager.debugEnabled);
    }

    void ConsoleLog(string logString, string stackTrace, LogType type)
    {
        // If the last unit in the array has already been written to...
        if (logs[logs.Length - 1] != null)
        {
            // Move every value in the array up by one index, 
            for (int i = 0; i < logs.Length - 1; i++)
            {
                logs[i] = logs[i + 1];
            }

            // replacing the last value with the logString
            logs[logs.Length - 1] = logString;
        }
        
        // If there is at least one empty value in the array...
        else
        {
            // Add the logstring to the first null index
            for (int i = 0; i < logs.Length; i++)
            {
                {
                    if (logs[i] == null) { logs[i] = logString; break; }
                }
            }
        }

        // Update the Debugger text
        string textBodyString = "";
        foreach (string log in logs) { if (log != null) { textBodyString += "> " + log + "\n"; } }
        logText.text = textBodyString;
    }

    
}
