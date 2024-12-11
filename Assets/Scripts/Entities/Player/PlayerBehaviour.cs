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

    public enum StatType { attack, defense, speed, hp, recover }

    private List<Upgrade> attackUpgrades;
    private List<Upgrade> defenseUpgrades;
    private List<Upgrade> speedUpgrades;
    private List<Upgrade> hpUpgrades;
    private List<Upgrade> recoverUpgrades;

    [SerializeField]
    private Animator playerGunAnimator;

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
            float manualMoveX = Mathf.Abs(rb.velocity.x) < totalSpeed * manualMoveModifier ? totalSpeed * manualMoveModifier * moveDir.x : 0;
            float manualMoveY = Mathf.Abs(rb.velocity.y) < totalSpeed * manualMoveModifier ? totalSpeed * manualMoveModifier * moveDir.y : 0;
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
        cursorPos = context.ReadValue<Vector2>();
        aimPos = mainCam.ScreenToWorldPoint(cursorPos);

        aimTransform.localPosition = (aimPos - (Vector2)transform.position).normalized;
        aimTransform.up = aimPos - (Vector2)transform.position;
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
        if (!isInvincible)
        {
            hpBar.TakeDamage(amount);
        }
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        hpBar.HealHp(amount);
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

    /*public void UpgradeStats(StatType statType, float value, bool multiply)
    {
        switch (statType)
        {
            case StatType.attack:
                if (multiply)
                {
                    attackStat *= value;
                } else
                {
                    attackStat += value;
                }
                Debug.Log("Attack increased to " + attackStat);
                break;
            case StatType.defense:
                if (multiply)
                {
                    defenseStat *= value;
                } else
                {
                    defenseStat += value;
                }
                Debug.Log("Defense increased to " + defenseStat);
                break;
            case StatType.speed:
                if (multiply)
                {
                    speedStat *= value;
                } else
                {
                    speedStat += value;
                }
                Debug.Log("Speed increased to " + speedStat);
                break;
            case StatType.hp:
                if (multiply)
                {
                    hpStat *= value;
                } else
                {
                    hpStat += value;
                }
                Debug.Log("HP increased to " + hpStat);
                break;
            case StatType.recover:
                if (multiply)
                {
                    recoverStat *= value;
                } else
                {
                    recoverStat += value;
                }
                Debug.Log("Recover increased to " + recoverStat);
                break;
            default:
                Debug.LogError("Not a valid stat type.");
                break;
        }
        UpdateStats();
    }*/

    public void OnUsePrimary(InputAction.CallbackContext context)
    {
        if (primary != null)
        {
            if (primaryAutofire)
            {
                if (context.started)
                {
                    playerGunAnimator.SetTrigger("isShooting");
                    isFiringPrimary = true;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.shotgunGun, this.transform.position);
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
        SceneManager.LoadScene(gameOverScene);
        //StartCoroutine(HandleDeath());
    }
    //Added by ZS, to play the death animation and add a delay before switching scenes to the gameover menu
   // private IEnumerator HandleDeath()
   // {
   //     yield return new WaitForSeconds(deathDelay);

   // }
}

