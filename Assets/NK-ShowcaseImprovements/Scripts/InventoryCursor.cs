using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryCursor : MonoBehaviour
{
    [SerializeField]
    private Vector2 defaultPos = Vector2.zero;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float deadzone = 0f;
    [HideInInspector]
    public bool controlCursor = false;
    private Vector2 moveDir;
    private Vector2 position;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        Debug.Log("position = " + position / screenResolution);
        if (controlCursor)
        {
            position += moveDir * speed * Screen.height * Time.unscaledDeltaTime;
            Mouse.current.WarpCursorPosition(position);
            if (position.x < 0)
            {
                position.x = 0;
            }
            if (position.x > Screen.width)
            {
                position.x = Screen.width;
            }
            if (position.y < 0)
            {
                position.y = 0;
            }
            if (position.y > Screen.height)
            {
                position.y = Screen.height;
            }
        }
    }

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        if (controlCursor)
        {
            if (context.ReadValue<Vector2>() != Vector2.zero)
            {
                moveDir = context.ReadValue<Vector2>();
            }
            else if (Mathf.Abs(moveDir.x) <= deadzone && Mathf.Abs(moveDir.y) <= deadzone)
            {
                moveDir = Vector2.zero;
            }
        }
    }

    public void SetCursorMode(bool isInventory)
    {
        if (isInventory)
        {
            if (playerInput.currentControlScheme == "Gamepad")
            {
                Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
                controlCursor = true;
                position = defaultPos * screenResolution;
                Debug.Log("position set to " + defaultPos + " * " + screenResolution + " = " + position);
                Debug.Log((defaultPos * screenResolution) + " = " + position);
                moveDir = Vector2.zero;
            } else
            {
                controlCursor = false;
            }
        } else
        {
            controlCursor = false;
        }
    }
}
