using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Script handles dynamic cursor changing between Menus and gameplay
/// </summary>
public class CursorManager : MonoBehaviour
{
    public Texture2D reticleTexture;
    public Texture2D uiCursorTexture;
    public Vector2 reticleHotSpot = Vector2.zero;
    public Vector2 uiHotSpot = Vector2.zero;

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.visible = true;
            Cursor.SetCursor(uiCursorTexture, uiHotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.visible = true;
            Cursor.SetCursor(reticleTexture, reticleHotSpot, CursorMode.Auto);
        }
    }
}