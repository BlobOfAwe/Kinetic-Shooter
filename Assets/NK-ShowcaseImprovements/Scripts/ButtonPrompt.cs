using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    [SerializeField]
    private bool isUI = true;
    [SerializeField]
    private Sprite keyboardSprite;
    [SerializeField]
    private Sprite gamepadSprite;

    private Image image;
    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;

    private void Awake()
    {
        if (isUI)
        {
            image = GetComponent<Image>();
        } else
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (FindObjectOfType<InventoryCursor>() != null && FindObjectOfType<InventoryCursor>().controlCursor)
        {
            if (isUI)
            {
                image.sprite = gamepadSprite;
            }
            else
            {
                spriteRenderer.sprite = gamepadSprite;
            }
        } else if (playerInput != null)
        {
            if (playerInput.currentControlScheme == "Keyboard+Mouse")
            {
                if (isUI)
                {
                    image.sprite = keyboardSprite;
                } else
                {
                    spriteRenderer.sprite = gamepadSprite;
                }
            } else if (playerInput.currentControlScheme == "Gamepad")
            {
                if (isUI)
                {
                    image.sprite = gamepadSprite;
                } else
                {
                    spriteRenderer.sprite = gamepadSprite;
                }
            }
        }
    }
}
