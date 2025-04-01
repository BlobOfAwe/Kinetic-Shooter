// ## - JV
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBouncerEnemy : Enemy
{
    [SerializeField]
    private Animator bossAnimator;

    //audio variable for boss movement
    private EventInstance bouncerMovementSound;

    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()

    public void Start()
    {
        bouncerMovementSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.bouncerMovement);
    }

    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // Otherwise, pursue the target
            else { state = 1; }

        }

        if (primary.available && !utility.inUse) { UseAbility(primary); }
        if (secondary.available && !utility.inUse) { UseAbility(secondary); }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                bossAnimator.SetBool("isMoving", true);
                PLAYBACK_STATE playbackState;
                // bouncer movement audio - GZ
                bouncerMovementSound.getPlaybackState(out playbackState);
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    bouncerMovementSound.start();
                }
                Wander();
                break;
            case 1: // Attack
                FaceTarget();
                if (utility.available) { UseAbility(utility); }
                // bouncer movement audio - GZ
                bouncerMovementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
        }
    }
}
