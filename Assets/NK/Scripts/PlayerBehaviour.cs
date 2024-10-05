using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public float recoilForce = 1f;

    public bool canMoveManually = false;

    public float moveSpeed = 1f;

    public float moveAcceleration = 1f;

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private GameObject aimReticle;

    private Rigidbody2D rb;

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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Shoot!");
            rb.AddForce(-transform.up * recoilForce, ForceMode2D.Impulse);
        }
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
