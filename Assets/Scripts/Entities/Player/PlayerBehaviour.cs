// ## - NK
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;

public class PlayerBehaviour : Entity
{
    [SerializeField]
    private bool primaryAutofire = true;

    [SerializeField]
    private bool secondaryAutofire = true;

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

    [SerializeField]
    private Transform aimTransform;

    [SerializeField]
    private HPBarSystem hpBar;


    [SerializeField]
    private int gameOverScene = 0;

    private bool isFiringPrimary = false;

    private bool isFiringSecondary = false;

    private Vector2 moveDir;

    private Vector2 cursorPos;

    private Vector2 aimPos;

    //audio variable for player movement
    private EventInstance playerMovementSound;


    //creates an FMOD events instance for player movement
    private void Start()
    {
        health = maxHealth;
        playerMovementSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerMovementSound);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        cursorPos = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();

        // MOVED CODE FROM UpdateSound() TO HERE. - NK
        //detects if player is moving and plays audio
        if (context.started)
        {
            PLAYBACK_STATE playbackState;
            playerMovementSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerMovementSound.start();
            }
        }
        //stops hover sound
        if (context.canceled)
        {
            playerMovementSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        hpBar.TakeDamage(amount);
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        hpBar.HealHp(amount);
    }

    public void OnUsePrimary(InputAction.CallbackContext context)
    {
        if (primary != null)
        {
            if (primaryAutofire)
            {
                if (context.started)
                {
                    isFiringPrimary = true;
                }
                if (context.canceled)
                {
                    isFiringPrimary = false;
                }
            }
            else if (context.performed)
            {
                UseAbility(primary);
            }
        }
    }

    public void OnUseSecondary(InputAction.CallbackContext context)
    {
        if (secondary != null)
        {
            if (secondaryAutofire)
            {
                if (context.started)
                {
                    isFiringSecondary = true;
                }
                if (context.canceled)
                {
                    isFiringSecondary = false;
                }
            }
            else if (context.performed)
            {
                UseAbility(secondary);
            }
        }
    }

    public void OnUseUtility(InputAction.CallbackContext context)
    {
        if (utility != null)
        {
            if (context.performed)
            {
                UseAbility(utility);
            }
        }
    }

    public void OnUseAdditional(InputAction.CallbackContext context)
    {
        if (additional != null)
        {
            if (context.performed)
            {
                UseAbility(additional);
            }
        }
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
        if (isFiringPrimary)
        {
            UseAbility(primary);
        }
        if (isFiringSecondary)
        {
            UseAbility(secondary);
        }
    }

    private void FixedUpdate()
    {
        if (canMoveManually && rb.velocity.magnitude < moveSpeed)
        {
            rb.AddForce(moveDir * moveAcceleration);
        }
        aimTransform.up = aimPos - (Vector2)transform.position;

        //UpdateSound();
    }

    // COMMENTED OUT THIS CODE BECAUSE IT SHOULD BE DONE IN OnMove(). - NK
    //detects if player is moving using WASD and plays audio
    /*private void UpdateSound()
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
    }*/

    protected override void Death()
    {
        SceneManager.LoadScene(gameOverScene);
    }
}
