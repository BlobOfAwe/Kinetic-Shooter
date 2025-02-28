// ## - GZ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    // Ambience Events

    [field: Header("Industrial Ambience")]
    [field: SerializeField] public EventReference industrialAmbience { get; private set; }

    [field: Header("Jungle Ambience")]
    [field: SerializeField] public EventReference jungleAmbience { get; private set; }

    [field: Header("Lava Ambience")]
    [field: SerializeField] public EventReference lavaAmbience { get; private set; }

    [field: Header("Cutscene Ambience")]
    [field: SerializeField] public EventReference cutsceneAmbience { get; private set; }

    [field: Header("Main Menu Ambience")]
    [field: SerializeField] public EventReference mainMenuAmbience { get; private set; }

    // Music Events

    [field: Header("Industrial Music")]
    [field: SerializeField] public EventReference industrialMusic { get; private set; }

    [field: Header("Jungle Music")]
    [field: SerializeField] public EventReference jungleMusic { get; private set; }

    [field: Header("Lava Music")]
    [field: SerializeField] public EventReference lavaMusic { get; private set; }

    [field: Header("Cutscene Music")]
    [field: SerializeField] public EventReference cutsceneMusic { get; private set; }

    [field: Header("Main Menu Music")]
    [field: SerializeField] public EventReference mainMenuMusic { get; private set; }

    // Player SFX: Weapon Events

    [field: Header("Shotgun SFX")]
    [field: SerializeField] public EventReference shotgunGun { get; private set; }

    [field: Header("Impaler SFX")]
    [field: SerializeField] public EventReference impalerGun { get; private set; }

    [field: Header("Extra Arms SFX")]
    [field: SerializeField] public EventReference extraArmsGun { get; private set; }

    [field: Header("Wide Shots SFX")]
    [field: SerializeField] public EventReference wideShotsGun { get; private set; }

    [field: Header("Melee SFX")]
    [field: SerializeField] public EventReference swooshMelee { get; private set; }

    // Loadout SFX

    // STANDARD

    [field: Header("Standard Primary SFX")]
    [field: SerializeField] public EventReference standardPrimary { get; private set; }

    [field: Header("Standard Secondary SFX")]
    [field: SerializeField] public EventReference standardSecondary { get; private set; }

    [field: Header("Standard Ability SFX")]
    [field: SerializeField] public EventReference standardAbility { get; private set; }

    // HEAVY

    [field: Header("Heavy Primary SFX")]
    [field: SerializeField] public EventReference heavyPrimary { get; private set; }

    [field: Header("Heavy Secondary SFX")]
    [field: SerializeField] public EventReference heavySecondary { get; private set; }

    [field: Header("Heavy Ability SFX")]
    [field: SerializeField] public EventReference heavyAbility { get; private set; }

    // LIGHT

    [field: Header("Light Primary SFX")]
    [field: SerializeField] public EventReference lightPrimary { get; private set; }

    [field: Header("Light Secondary SFX")]
    [field: SerializeField] public EventReference lightSecondary { get; private set; }

    [field: Header("Light Ability SFX")]
    [field: SerializeField] public EventReference lightAbility { get; private set; }

    // Player SFX: Movement Events

    [field: Header("Basic Movement")]
    [field: SerializeField] public EventReference basicMovement { get; private set; }

    [field: Header("Fast Flying Movement")]
    [field: SerializeField] public EventReference fastFlying { get; private set; }

    // Player SFX: Notification Events

    [field: Header("Damage Recieved")]
    [field: SerializeField] public EventReference damageRecieved { get; private set; }

    [field: Header("Player Voice Exclamation")]
    [field: SerializeField] public EventReference playerVoice { get; private set; }

    // Player SFX: Ability Events

    [field: Header("Electromagnetism")]
    [field: SerializeField] public EventReference electromagnetismAbility { get; private set; }

    [field: Header("Rocket Equip")]
    [field: SerializeField] public EventReference rocketEquipAbility { get; private set; }

    [field: Header("Rocket Launch")]
    [field: SerializeField] public EventReference rocketLaunchAbility { get; private set; }

    [field: Header("Rocket Impact")]
    [field: SerializeField] public EventReference rocketImpactAbility { get; private set; }

    [field: Header("Life Insurance")]
    [field: SerializeField] public EventReference lifeInsuranceAbility { get; private set; }

    [field: Header("Reflector Bolt")]
    [field: SerializeField] public EventReference reflectorBoltAbility { get; private set; }

    [field: Header("Firestarter")]
    [field: SerializeField] public EventReference firestarterAbility { get; private set; }

    [field: Header("Cushion Activation")]
    [field: SerializeField] public EventReference cushionActivateAbility { get; private set; }

    [field: Header("Cushion Break")]
    [field: SerializeField] public EventReference cushionBreakAbility { get; private set; }

    [field: Header("Turret Activation")]
    [field: SerializeField] public EventReference turretActivateAbility { get; private set; }

    [field: Header("Turret Shot")]
    [field: SerializeField] public EventReference turretShotAbility { get; private set; }

    [field: Header("Blasting Laser")]
    [field: SerializeField] public EventReference blastingLaserAbility { get; private set; }

    [field: Header("Grenade Deploy")]
    [field: SerializeField] public EventReference grenadeDeployAbility { get; private set; }

    [field: Header("Grenade Countdown")]
    [field: SerializeField] public EventReference grenadeCountdownAbility { get; private set; }

    [field: Header("Grenade Explosion")]
    [field: SerializeField] public EventReference grenadeExplosionAbility { get; private set; }

    [field: Header("Shield Activate")]
    [field: SerializeField] public EventReference shieldActivateAbility { get; private set; }

    [field: Header("Shield Use")]
    [field: SerializeField] public EventReference shieldUseAbility { get; private set; }

    [field: Header("Oil Spill")]
    [field: SerializeField] public EventReference oilSpillAbility { get; private set; }

    [field: Header("Cooldown Refresh")]
    [field: SerializeField] public EventReference cooldownRefresh { get; private set; }

    // Enemy SFX Events

    [field: Header("Enemy Damaged")]
    [field: SerializeField] public EventReference enemyDamaged { get; private set; }

    [field: Header("Enemy Death")]
    [field: SerializeField] public EventReference enemyDeath { get; private set; }

    [field: Header("Enemy Voice Exclamation")]
    [field: SerializeField] public EventReference enemyVoice { get; private set; }

    [field: Header("Boss Enemy Appear")]
    [field: SerializeField] public EventReference bossEnemyAppear { get; private set; }

    [field: Header("Enemy Movement")]
    [field: SerializeField] public EventReference enemyMovement { get; private set; }

    // Item SFX Events

    [field: Header("Item Pickup")]
    [field: SerializeField] public EventReference itemPickup { get; private set; }

    [field: Header("Item Approach")]
    [field: SerializeField] public EventReference itemApproach { get; private set; }

    [field: Header("Beacon")]
    [field: SerializeField] public EventReference beaconLoop { get; private set; }

    // UI SFX Events

    [field: Header("Button Select")]
    [field: SerializeField] public EventReference buttonSelect { get; private set; }

    [field: Header("Button Hover")]
    [field: SerializeField] public EventReference buttonHover { get; private set; }


    // Checks if there is more than one FMODEvents script in the scene (which is a no no)

    public static FMODEvents instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than on FMOD Events instance in the scene");
        }
        instance = this;
    }

}
