// ## - NK
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using System.Collections.Generic;

public class PlayerBehaviour : Entity
{
    [SerializeField]
    private bool primaryAutofire = true;

    [SerializeField]
    private bool secondaryAutofire = true;

    [SerializeField]
    private bool canMoveManually = false;

    [SerializeField]
    private float moveAcceleration = 1f;

    private Camera mainCam;

    [SerializeField]
    public Transform aimTransform;

    private HPBarSystem hpBar;
    //Added by ZS to reference the animators and set the timer for the death delay
    [SerializeField]
    private Animator playerAnimator;
    // Added by ZS to reference the GameOver Panels
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject winPanel;
    public enum StatType { attack, defense, speed, hp, recover }

    private List<Upgrade> attackUpgrades;
    private List<Upgrade> defenseUpgrades;
    private List<Upgrade> speedUpgrades;
    private List<Upgrade> hpUpgrades;
    private List<Upgrade> recoverUpgrades;

    public Animator playerGunAnimator; // Changed to public so that the animator can be referenced by abilities.

    [SerializeField]
    private float deathDelay = 0.5f;

    [SerializeField]
    private int gameOverScene = 3;

    [SerializeField]
    private float manualMoveModifier = 0.1f;

    private bool isFiringPrimary = false;

    private bool isFiringSecondary = false;

    private Vector2 moveDir;

    private Vector2 cursorPos;

    private Vector2 aimPos;

    //audio variable for player movement
    private EventInstance playerMovementSound;

    private float audioTimer = 0.5f;
    //ending parameter
    [SerializeField] private string parameterNameEnding;
    [SerializeField] private float parameterValueEnding;

    //creates an FMOD events instance for player movement
    private void Start()
    {
        health = maxHealth;
        hpBar = FindAnyObjectByType<HPBarSystem>();
        mainCam = Camera.main;
        playerMovementSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.basicMovement);

        // Assigns the player.ability to be the component based on the specified ability type
        primary = (Ability)GetComponent(GameManager.playerLoadout.primaryAbility.GetType());
        secondary = (Ability)GetComponent(GameManager.playerLoadout.secondaryAbility.GetType());
        utility = (Ability)GetComponent(GameManager.playerLoadout.utilityAbility.GetType());

    }

    new private void Update()
    {
        base.Update();

        if (isFiringPrimary)
        {
            UseAbility(primary);
        }
        if (isFiringSecondary)
        {
            UseAbility(secondary);
        }


        // audio timer
        if (audioTimer > 0)
        {
            //Subtract elapsed time every frame
            audioTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (isInvincible)
            {
                isInvincible = false;
            } else
            {
                isInvincible = true;
            }
            Debug.Log("Invincibility = " + isInvincible);

        }
    }

    private void FixedUpdate()
    {
        //Added by ZS, checks if the player is idle, and if they are, plays the idle animation.
        Vector2 velocity = rb.velocity;
        bool isIdle = Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.y) < 0.1f;
        playerAnimator.SetBool("isIdle", isIdle);

        if (canMoveManually)
        {
            // If (negative Input)
            // If (Velocity > the negative-maximum for base movement speed)
            // Add movement speed
            // else if (positive Input)
            // If (Velocity < the positive-maximum for base movement speed)
            float manualMoveX;
            float manualMoveY;

            if (moveDir.x < 0 && rb.velocity.x > -totalSpeed * manualMoveModifier) { manualMoveX = totalSpeed * manualMoveModifier * moveDir.x; }
            else if (moveDir.x > 0 && rb.velocity.x < totalSpeed * manualMoveModifier) { manualMoveX = totalSpeed * manualMoveModifier * moveDir.x; }
            else { manualMoveX = 0; }

            if (moveDir.y < 0 && rb.velocity.y > -totalSpeed * manualMoveModifier) { manualMoveY = totalSpeed * manualMoveModifier * moveDir.y; }
            else if (moveDir.y > 0 && rb.velocity.y < totalSpeed * manualMoveModifier) { manualMoveY = totalSpeed * manualMoveModifier * moveDir.y; }
            else { manualMoveY = 0; }

            rb.velocity += new Vector2(manualMoveX, manualMoveY); 
        }

        if (rb.velocity.magnitude > totalSpeed) { rb.velocity = rb.velocity.normalized * totalSpeed; }
        //UpdateSound();
    }

    /// <summary>
    /// If the player is moving slower than the lower bound;
    /// Add to the velocity until they hit that lower bound;
    /// </summary>

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!GameManager.paused)
        {
            cursorPos = context.ReadValue<Vector2>();
            aimPos = mainCam.ScreenToWorldPoint(cursorPos);

            aimTransform.localPosition = (aimPos - (Vector2)transform.position).normalized;
            aimTransform.up = aimPos - (Vector2)transform.position;
        }
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
            playerMovementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void OnHunker(InputAction.CallbackContext context)
    {
        Debug.Log("Hunkered");
        if (context.performed)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            canMoveManually = false;
        }
        else
        {
            rb.isKinematic = false;
            canMoveManually = true;
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        //hpBar.TakeDamage(amount);
        if (audioTimer <= 0)
        {
            //plays damage sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.damageRecieved, this.transform.position);
            audioTimer = 0.5f;
        }
        if (!isInvincible)
        {
            //hpBar.TakeDamage(amount);
        }
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        //hpBar.HealHp(amount);
    }

    public void ProjectileDestroyEffect(TestBullet bullet, bool hitDamageable)
    {
        if (inventoryManager != null)
        {
            foreach (InventorySlot slot in inventoryManager.inventory)
            {
                if (slot.item != null)
                {
                    if (slot.item.GetComponent<Upgrade>() != null)
                    {
                        slot.item.GetComponent<Upgrade>().ProjectileUpgradeEffect(bullet, hitDamageable, slot.quantity);
                    }
                }
            }
        }
        bullet.gameObject.SetActive(false);
    }

    public void OnUsePrimary(InputAction.CallbackContext context)
    {
        if (primary != null)
        {
            if (primaryAutofire)
            {
                if (context.started)
                {
                    // Removed audio and animator triggers because this should be happening when the ability activates rather than every time the fire button is pressed.

                    //playerGunAnimator.SetTrigger("isShooting");
                    isFiringPrimary = true;
                    //AudioManager.instance.PlayOneShot(FMODEvents.instance.impalerGun, this.transform.position);
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
        Quit();
    }

    public void Quit() { Application.Quit(); }



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

    public override void Death()
    {
        // playerAnimator.SetTrigger("isDead");
        //SceneManager.LoadScene(gameOverScene);
        GameOverPanel();
        //StartCoroutine(HandleDeath());
    }
    //Added by ZS to display the Gameover/Win screens as a panel rather than seperate scenes.
    public void GameOverPanel()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    //Added by ZS, to play the death animation and add a delay before switching scenes to the gameover menu
    // private IEnumerator HandleDeath()
    // {
    //     yield return new WaitForSeconds(deathDelay);

    // }
}

