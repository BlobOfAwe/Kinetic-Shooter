using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMOD.Studio;
public class BaseTwinsEnemy : Enemy
{
    [SerializeField] private BaseTwinsEnemy twin;
    [SerializeField] public bool eldest; // The "eldest" twin controls the lightning connection, only one twin should ever be eldest
    [SerializeField] private BoxCollider2D connector;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float orphanedBuffValue;

    //audio emitter variable
    protected StudioEventEmitter emitter;
    //protected BaseTwinsEnemy lightingTwinsEnemy;

    protected new void Start()
    {

        //creates an audio emitter and plays event
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.lightningTwins, this.gameObject);
        emitter.Play();
        //lightingTwinsEnemy = FindObjectOfType<BaseTwinsEnemy>();

        base.Start();

        if (eldest)
        {
            var tempObj = transform.parent;
            twin.transform.parent = null;
            connector.transform.parent = null;
            transform.parent = null;
            Destroy(tempObj.gameObject);

            if (twin.eldest) { Debug.LogError("A pair of lightning twins cannot both be eldest"); }
        }
    }

    // DerivativeUpdate is called once per frame as a part of the abstract Enemy class' Update()
    public override void DerivativeUpdate()
    {
        if (ReadyToStateChange())
        {
            // If the enemy has no target, Wander
            if (target == null)
            { state = 0; }
            // If the enemy is already strafing and the target has not gone far enough to be chased, Strafe, 
            else if (state == 2 && distanceToTarget < stayDistance + chaseDistance)
            { state = 2; }
            // If the enemy is far enough away, Pursue
            else if (distanceToTarget > stayDistance)
            { state = 1; }
            // If the enemy is close to its target, Attack
            else if (distanceToTarget < stayDistance)
            { state = 2; }

        }

        switch (state)
        {
            case 0: // Wandering
                SearchForTarget();
                Wander();
                break;
            case 1: // Pursuit
                Pursue();
                RefreshTarget(); // Periodically update to see if target is within range. Lose interest if not
                break;
            case 2: // Attack
                Strafe();
                if (twin == null && primary.available && distanceToTarget < (primary.range + 1)) { UseAbility(primary); }
                break;
        }
    }

    new private void LateUpdate()
    {
        base.LateUpdate();
        if (eldest && twin != null)
        {
            // Calculate the distance and direction to the younger twin
            float twinDistance = Vector2.Distance(transform.position, twin.transform.position);
            Vector2 twinDir = new Vector2(twin.transform.position.x - transform.position.x, twin.transform.position.y - transform.position.y);
            
            // Size and rotate the connector object to fill the space between twins.
            connector.size = new Vector2(twinDistance, connector.size.y);
            connector.transform.position = (Vector2) transform.position + (twinDir / 2);
            connector.transform.right = twinDir;

            ParticleSystem.ShapeModule psShape = particles.shape;
            psShape.scale = connector.size;
        }
    }

    public override void Death()
    {
        if (twin != null)
        {
            // Apply a speed buff to the remaining twin
            Buff orphanBuff = new Buff();
            orphanBuff.buffType = Buff.buffCategory.SPEED_BUFF;
            orphanBuff.modification = Buff.modificationType.Additive;
            orphanBuff.value = orphanedBuffValue;
            twin.ApplyBuff(orphanBuff);

            twin.twin = null;
            twin.connector = null;
            //stops audio emitter
            emitter.Stop();
            Destroy(connector.gameObject);
        }
        
        base.Death();
    }
}
