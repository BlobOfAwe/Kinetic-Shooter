using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;

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

    private Vector2 aimPos;

    //audio variable for player movement
    private EventInstance playerMovementSound;

    //creates an FMOD events instance for player movement
    private void Start()
    {
        playerMovementSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerMovementSound);
    }

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
        aimPos = mainCam.ScreenToWorldPoint(cursorPos);
        if (aimReticle != null)
        {
            aimReticle.transform.position = aimPos;
        }
    }

    private void FixedUpdate()
    {
        if (canMoveManually && rb.velocity.magnitude < moveSpeed)
        {
            rb.AddForce(moveDir * moveAcceleration);
        }
        transform.up = aimPos - (Vector2)transform.position;

        UpdateSound();
    }

    //detects if player is moving using WASD and plays audio
    private void UpdateSound()
    {
        //plays hover sound if player presses "W"
        if (Input.GetKey(KeyCode.W))
        {
            PLAYBACK_STATE playbackState;
            playerMovementSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerMovementSound.start();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        //plays hover sound if player presses "A"
        {
            PLAYBACK_STATE playbackState;
            playerMovementSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerMovementSound.start();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        //plays hover sound if player presses "S"
        {
            PLAYBACK_STATE playbackState;
            playerMovementSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerMovementSound.start();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        //plays hover sound if player presses "D"
        {
            PLAYBACK_STATE playbackState;
            playerMovementSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerMovementSound.start();
            }
        }
        else
        //stops hover sound
        {
            playerMovementSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}

