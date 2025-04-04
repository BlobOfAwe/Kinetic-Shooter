// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCry : Ability
{
    private Collider2D[] entities;
    [SerializeField] private List<ParticleSystem> buffIndicators;
    [SerializeField] private ParticleSystem warCryEffect;
    [SerializeField] private int initialIndicators = 10;
    [SerializeField] private LayerMask affectLayers;
    [SerializeField] private GameObject buffIndicatorPrefab;
    [SerializeField] private float buffDuration;
    [SerializeField] private Animator animator;

    new private void Awake()
    {
        base.Awake();
        
        // Initialize a predetermined number of indicators. This reduces the potential load for instantiating indicators at runtime
        for (int i = 0; i < initialIndicators; i++)
        {
            buffIndicators.Add(Instantiate(buffIndicatorPrefab).GetComponent<ParticleSystem>());
            buffIndicators[i].gameObject.SetActive(false);
        }
    }

    // When the ability is activated
    public override void OnActivate()
    {
        StartCoroutine(BeginCooldown());
        
        // Choose a random buff type
        WarCallBuff.buffType buffType;
        var rand = Random.Range(0, 3);
        Debug.Log(rand);

        switch (rand)
        {
            case 0:
                buffType = WarCallBuff.buffType.Damage; break;
            case 1:
                buffType = WarCallBuff.buffType.Heal; break;
            case 2:
                buffType = WarCallBuff.buffType.Speed; break;
            default:
                buffType = WarCallBuff.buffType.Damage; Debug.LogError("Invalid WarCall Type");  break; 
        }
        switch (buffType)
        {
            case WarCallBuff.buffType.Damage:
                animator.SetBool("isDamage", true);
                animator.SetBool("isHealth", false);
                animator.SetBool("isSpeed", false);
                break;
            case WarCallBuff.buffType.Heal:
                animator.SetBool("isHealth", true);
                animator.SetBool("isDamage", false);
                animator.SetBool("isSpeed", false);
                break;
            case WarCallBuff.buffType.Speed:
                animator.SetBool("isSpeed", true);
                animator.SetBool("isDamage", false);
                animator.SetBool("isHealth", false);
                break;
        }
        // Play the particle system effect for the war call
        warCryEffect.Stop();
        warCryEffect.Play();

        // Check for all valid targets within range
        entities = Physics2D.OverlapCircleAll(transform.position, range, affectLayers);

        // If there are more entities than buff indicators, create new buff indicators until there are enough
        if (entities.Length > buffIndicators.Count)
        {
            for (int i = 0; i < entities.Length - buffIndicators.Count; i++)
            {
                buffIndicators.Add(Instantiate(buffIndicatorPrefab).GetComponent<ParticleSystem>());
            }
        }

        // For each entity targeted
        for (int i = 0; i < entities.Length; i++)
        {
            // If the associated slot in buffIndicators is empty, populate it with a new object.
            // This can happen if an enemy is destroyed while the buff is attatched to it
            if (buffIndicators[i] == null) { buffIndicators[i] = Instantiate(buffIndicatorPrefab).GetComponent<ParticleSystem>(); }

            // Assign the associated buff indicator as a child of the target entity
            buffIndicators[i].transform.position = entities[i].transform.position;
            buffIndicators[i].transform.parent = entities[i].transform;

            // Activate the buff indicator and apply the buff
            buffIndicators[i].gameObject.SetActive(true);
            buffIndicators[i].GetComponent<WarCallBuff>().buff = buffType;
            buffIndicators[i].GetComponent<WarCallBuff>().ApplyBuff(buffDuration);
        }
    }

    public override void PurgeDependantObjects()
    {
        foreach (ParticleSystem indicator in buffIndicators) 
        { 
            if (indicator != null)
                Destroy(indicator.gameObject); 
        }
    }
}
