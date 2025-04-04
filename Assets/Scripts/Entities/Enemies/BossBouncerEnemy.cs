// ## - JV
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBouncerEnemy : Enemy
{

    private Animator animator;

        //audio emitter variable
    protected StudioEventEmitter emitter;
    
    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }



    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()

    protected override void Start()
    {
        base.Start();
        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.bouncerMovement, this.gameObject);
        emitter.Play();
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
                animator.SetBool("isMoving", true);
                PLAYBACK_STATE playbackState;
                Wander();
                break;
            case 1: // Attack
                FaceTarget();
                if (utility.available) { UseAbility(utility); }
                //AudioManager.instance.PlayOneShot(FMODEvents.instance.bouncerLaunch, this.transform.position);
                break;
        }
    }
    public override void Death()
    {
        animator.SetTrigger("isDead");
        StartCoroutine(DeathSequence());
    }
    private IEnumerator DeathSequence()
    {
        float deathAnimationLength = 0.8f;
        yield return new WaitForSeconds(deathAnimationLength);
        base.Death();
    }
}
