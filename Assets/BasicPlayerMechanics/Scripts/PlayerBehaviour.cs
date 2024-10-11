using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool canMoveManually = false;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float moveAcceleration = 1f;

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private GameObject aimReticle;

    [HideInInspector]
    public Rigidbody2D rb;

    private Vector2 moveDir;

    private Vector2 cursorPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        cursorPos = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    private void Update()
    {
        Vector2 aimPos = mainCam.ScreenToWorldPoint(cursorPos);
        if (aimReticle != null)
        {
            aimReticle.transform.position = aimPos;
        }
        transform.up = aimPos - (Vector2)transform.position;
    }

    private void FixedUpdate()
    {
        if (canMoveManually && rb.velocity.magnitude < moveSpeed)
        {
            rb.AddForce(moveDir * moveAcceleration);
        }
    }
}
