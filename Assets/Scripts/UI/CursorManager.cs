using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/// <summary>
/// Script handles dynamic cursor changing between Menus and gameplay
/// </summary>
public class CursorManager : MonoBehaviour
{
    public Texture2D reticleTexture;
    public Texture2D uiCursorTexture;
    public Vector2 reticleHotSpot = Vector2.zero;
    public Vector2 uiHotSpot = Vector2.zero;
    [SerializeField]
    private int menuScene;
    [HideInInspector]
    public bool isEndScreen = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // Commented out because cursor management should be handled differently. - NK
        /*if (EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.visible = true;
            Cursor.SetCursor(reticleTexture, reticleHotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.visible = true;
            Cursor.SetCursor(uiCursorTexture, uiHotSpot, CursorMode.Auto);
        }*/
        if (GameManager.paused || isEndScreen || SceneManager.GetActiveScene().buildIndex == menuScene)
        {
            Cursor.SetCursor(uiCursorTexture, uiHotSpot, CursorMode.Auto);
        } else
        {
            Cursor.SetCursor(reticleTexture, reticleHotSpot, CursorMode.Auto);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isEndScreen = false;
    }
}