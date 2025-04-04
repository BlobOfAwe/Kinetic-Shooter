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
    public bool canMoveManually = false;

    // Temporarily serialized for testing.
    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    public Transform aimTransform;
    [SerializeField]
    public Transform firePoint;

    // When the player is mirrored, the shoulder's position should be positive/negative this value to mirror around the y axis
    private float offsetShoulderFromCenterX;
    // The default position of the firepoint relative to the shoulder. This value is modified by offsetShoulderFromCenterX when the player is mirrored
    private float firePointLocalPosDefaultX; 

    [SerializeField]
    private float rotationOffset = 0f;
    [SerializeField]
    private Transform gunHolder;
    [SerializeField]
    private Transform headHolder;

    private HPBarSystem hpBar;
    //Added by ZS to reference the animators and set the timer for the death delay
    
    public Animator playerAnimator;
    // Added by ZS to reference the GameOver Panels
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject winPanel;

    private PauseMenu pauseMenu;

    private InventoryManager inventory;

    private LineRenderer aimLine;

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
    private float manualMoveModifier = 0.1f;

    [SerializeField]
    private float aimLineLength = 1f;

    private TestBullet lastBullet;

    // Changed to public so it can be accessed by cushion upgrade.
    [HideInInspector]
    public bool isFiringPrimary = false;

    [HideInInspector]
    public bool isFiringSecondary = false;

    private bool isGameEnd = false;

    private Vector2 moveDir;

    private Vector2 cursorPos;

    private Vector2 aimPos;

    [SerializeField]
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField]
    private SpriteRenderer gunSpriteRenderer;
    [SerializeField]
    private SpriteRenderer playerHeadSpriteRenderer;
    //audio variable for player movement
    private EventInstance playerMovementSound;

    private float audioTimer = 0.5f;
    //ending parameter
    [SerializeField] private string parameterNameEnding;
    [SerializeField] private float parameterValueEnding;

    [SerializeField]
    private LoadoutManager.Loadout loadout;
    
    public ShootAbility primaryShootAbility; // A more specific reference to Entity.primary used by upgrades

    protected override void Awake()
    {
        base.Awake();
        if (GameManager.playerLoadout != null)
        {
            loadout = GameManager.playerLoadout;
        }

        // Assigns the player.ability to be the component based on the specified ability type
        primary = (Ability)GetComponent(loadout.primaryAbility.GetType());
        primaryShootAbility = (ShootAbility)primary;
        secondary = (Ability)GetComponent(loadout.secondaryAbility.GetType());
        utility = (Ability)GetComponent(loadout.utilityAbility.GetType());

        attackStat = loadout.attackStat;
        defenseStat = loadout.defenseStat;
        hpStat = loadout.hpStat;
        recoverStat = loadout.recoverStat;
        speedStat = loadout.speedStat;

        health = maxHealth;
    }
    //creates an FMOD events instance for player movement
    private void Start()
    {
        offsetShoulderFromCenterX = aimTransform.localPosition.x;
        firePointLocalPosDefaultX = firePoint.localPosition.x;
        hpBar = FindAnyObjectByType<HPBarSystem>();
        mainCam = Camera.main;
        playerMovementSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.basicMovement);
        playerAnimator.SetTrigger("isTeleportingIn");
        playerGunAnimator.SetTrigger("isTeleportingIn");

        pauseMenu = FindObjectOfType<PauseMenu>();
        inventory = FindObjectOfType<InventoryManager>();

        aimLine = GetComponentInChildren<LineRenderer>();
    }

    new private void Update()
    {
        base.Update();

        if (!isGameEnd)
        {
            if (isFiringPrimary)
            {
                UseAbility(primary);

            }
            if (isFiringSecondary)
            {
                UseAbility(secondary);
            }
            aimLine.SetPosition(0, firePoint.position);
            aimLine.SetPosition(1, firePoint.position + aimTransform.up * aimLineLength);
        } else
        {
            aimLine.enabled = false;
        }

        // audio timer
        if (audioTimer > 0)
        {
            //Subtract elapsed time every frame
            audioTimer -= Time.deltaTime;
        }

        // Obsolete. Invincibility is now handled differently.
        /*if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (isInvincible)
            {
                isInvincible = false;
            } else
            {
                isInvincible = true;
            }
            Debug.Log("Invincibility = " + isInvincible);

        }*/
    }

    private void FixedUpdate()
    {
        //Added by ZS, checks if the player is idle, and if they are, plays the idle animation.
       // Vector2 velocity = rb.velocity;
       // bool isIdle = Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.y) < 0.1f;
       // playerAnimator.SetBool("isIdle", isIdle);

        if (canMoveManually && !isGameEnd)
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

        if (capSpeedToTotalSpeed && rb.velocity.magnitude > totalSpeed) { rb.velocity = rb.velocity.normalized * totalSpeed; }
        
        //UpdateSound();
    }

    /// <summary>
    /// If the player is moving slower than the lower bound;
    /// Add to the velocity until they hit that lower bound;
    /// </summary>

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (cushion > 0f && collision.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("Damaged " + collision.gameObject.name + " with shield for " + (totalAttack * cushion) + " damage.");
            collision.gameObject.GetComponent<Enemy>().Damage(totalAttack * cushion, true);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!GameManager.paused && !isGameEnd)
        {
            PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();

            Vector2 direction = aimTransform.up;

            if (playerInput.currentControlScheme == "Keyboard+Mouse")
            {
                Vector2 cursorPos = context.ReadValue<Vector2>();
                Vector2 aimPos = Vector2.zero;

                if (mainCam != null)
                {
                    aimPos = mainCam.ScreenToWorldPoint(cursorPos);
                }
                else { Debug.LogWarning("No Main Camera detected"); }

                // Created a local variable to reference the transform position instead of typing it manually. Z.S
                direction = aimPos - (Vector2)aimTransform.position;
            } else if (playerInput.currentControlScheme == "Gamepad")
            {
                if (Mathf.Abs(context.ReadValue<Vector2>().x) > 0f && Mathf.Abs(context.ReadValue<Vector2>().y) > 0f)
                {
                    direction = context.ReadValue<Vector2>();
                }
            }

            // aimTransform.localPosition = direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimTransform.up = direction;
            gunHolder.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationOffset));
            headHolder.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Flips the player and gun sprite to make the direction of view
            if (direction.x < 0)
            {
                playerSpriteRenderer.flipX = false;
                playerHeadSpriteRenderer.flipX = true;
                playerHeadSpriteRenderer.flipY = true;
                //gunSpriteRenderer.flipX = true; // DEFUNCT This value should never not be true
                gunSpriteRenderer.flipY = true;

                //gunHolder.localPosition = new Vector2( 0.153f, -0.16f); 
                aimTransform.localPosition = new Vector2(offsetShoulderFromCenterX, aimTransform.localPosition.y);
                firePoint.localPosition = new Vector2(firePointLocalPosDefaultX, firePoint.localPosition.y);
            }
            else if (direction.x > 0)
            {
                playerSpriteRenderer.flipX = true;
                playerHeadSpriteRenderer.flipX = true;
                playerHeadSpriteRenderer.flipY = false;
                //gunSpriteRenderer.flipX = true; // DEFUNCT This value should never not be true
                gunSpriteRenderer.flipY = false;
                //gunHolder.localPosition = new Vector2(-0.153f, -0.16f);
                aimTransform.localPosition = new Vector2(-offsetShoulderFromCenterX, aimTransform.localPosition.y);
                firePoint.localPosition = new Vector2(-firePointLocalPosDefaultX, firePoint.localPosition.y);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isGameEnd)
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
    }

    public void OnHunker(InputAction.CallbackContext context)
    {
        if (!isGameEnd)
        {
            //Debug.Log("Hunkered");
            if (context.performed)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                canMoveManually = false;
                playerAnimator.SetBool("isHunkered", true);
            }
            else
            {
                rb.isKinematic = false;
                canMoveManually = true;
                playerAnimator.SetBool("isHunkered", false);
            }
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        playerAnimator.SetTrigger("isHurt");
        //hpBar.TakeDamage(amount);
        if (audioTimer <= 0)
        {
            //plays damage sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.damageRecieved, this.transform.position);
            audioTimer = 0.5f;
        }
        
        /*if (!isInvincible)
        {
            //hpBar.TakeDamage(amount);
        }*/
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        //hpBar.HealHp(amount);
    }

    public void SetLastBullet(TestBullet bullet)
    {
        lastBullet = bullet;
    }

    public void ProjectileDestroyEffect(TestBullet bullet, GameObject target)
    {
        if (inventoryManager != null)
        {
            foreach (InventorySlot slot in inventoryManager.inventory)
            {
                if (slot.item != null)
                {
                    if (slot.item.GetComponent<Upgrade>() != null)
                    {
                        slot.item.GetComponent<Upgrade>().ProjectileUpgradeEffect(bullet, target, slot.quantity);
                    }
                }
            }
        }
        if (bullet.isFromBurst)
        {
            bullet.isFromBurst = false;
        } else
        {
            if (target.GetComponent<Entity>() != null)
            {
                bullet.hits -= 1;
                //Debug.Log("hits: " + bullet.hits);
            }
            else
            {
                bullet.hits = 1;
                //Debug.Log("Hit a wall. Hits reset to " + bullet.hits);
                bullet.isPiercing = false;
                bullet.isBursting = false;
                bullet.gameObject.SetActive(false);
            }
            if (bullet.hits <= 0)
            {
                bullet.hits = 1;
                //Debug.Log("Out of hits. Hits reset to " + bullet.hits);
                bullet.isPiercing = false;
                bullet.isBursting = false;
                bullet.gameObject.SetActive(false);
            }
        }
    }

    public void ProjectileFireEffect(TestBullet bullet)
    {
        if (inventoryManager != null)
        {
            foreach (InventorySlot slot in inventoryManager.inventory)
            {
                if (slot.item != null)
                {
                    if (slot.item.GetComponent<Upgrade>() != null)
                    {
                        slot.item.GetComponent<Upgrade>().FireUpgradeEffect(slot.quantity, bullet);
                    }
                }
            }
        }
    }

    public void ProjectileKillEffect(Enemy target)
    {
        if (inventoryManager != null)
        {
            foreach (InventorySlot slot in inventoryManager.inventory)
            {
                if (slot.item != null)
                {
                    if (slot.item.GetComponent<Upgrade>() != null)
                    {
                        slot.item.GetComponent<Upgrade>().KillUpgradeEffect(target, slot.quantity);
                    }
                }
            }
        }
    }

    public void OnUsePrimary(InputAction.CallbackContext context)
    {
        if (primary != null && !isGameEnd)
        {
            if (primaryAutofire)
            {
                if (context.started)
                {
                    // Removed audio and animator triggers because this should be happening when the ability activates rather than every time the fire button is pressed.
                    //Added the animator trigger back in because it was interacting weirdly with the ability activation causing the animation to play twice for a single instance of primary activation. Z.S
                    playerGunAnimator.SetTrigger("isShooting");
                    
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
        if (secondary != null && !isGameEnd)
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
        if (utility != null && !isGameEnd)
        {
            if (context.performed)
            {
                UseAbility(utility);
            }
        }
    }

    // Not used
    /*public void OnUseAdditional(InputAction.CallbackContext context)
    {
        if (additional != null && !isGameEnd)
        {
            if (context.performed)
            {
                UseAbility(additional);
            }
        }
    }*/

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.paused)
            {
                pauseMenu.ResumeGame();
            } else
            {
                pauseMenu.PauseGame();
            }
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventory.ToggleSidebar();
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

    public override void Death()
    {
        if (!isGameEnd)
        {
            isGameEnd = true;
            totalSpeed = 0f;
            playerAnimator.SetTrigger("isDead");
            //SceneManager.LoadScene(gameOverScene);

            StartCoroutine(HandleDeath());
        }
    }
    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathDelay);
        GameOverPanel();
    }
    //Added by ZS to display the Gameover/Win screens as a panel rather than seperate scenes.
    public void GameOverPanel()
    {
        //Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void TeleportAnim()
    {
        if (!isGameEnd)
        {
            isGameEnd = true;
            totalSpeed = 0f;
            playerAnimator.SetTrigger("isTeleporting");
            playerGunAnimator.SetTrigger("isTeleporting");
        }
    }
    //Added by ZS, to play the death animation and add a delay before switching scenes to the gameover menu


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

}

