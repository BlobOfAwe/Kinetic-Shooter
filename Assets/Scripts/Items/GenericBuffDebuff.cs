using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBuffDebuff : Item
{
    public enum buffType { Buff, Debuff }
    private Buff.buffCategory category;
    public buffType buff;
    private PlayerBehaviour buffTarget;
    [SerializeField] private float duration;
    private BuffUIManager buffUI;

    private new void Start()
    {
        base.Start();
        category = (Buff.buffCategory)Random.Range(0, 5);
        buffUI = FindObjectOfType<BuffUIManager>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<PlayerBehaviour>()) { return; }

        buffTarget = collision.GetComponent<PlayerBehaviour>();
        ApplyBuff(duration);
        
        AudioManager.instance.PlayOneShot(FMODEvents.instance.itemPickup, this.transform.position);
       //Debug.Log("Picked up item");

        emitter.Stop();
        Destroy(gameObject);
    }

    public void ApplyBuff(float duration)
    {
        Buff buffConstructor = ScriptableObject.CreateInstance<Buff>(); // Create a new buff object

        buffConstructor.buffType = category;

        // If its a healing buff, simply heal the player and do not apply a buff effect
        if (buff == buffType.Buff && category == Buff.buffCategory.HP_BUFF)
        {
            buffTarget.Heal(buffTarget.maxHealth * 0.25f);
            return;
        }

        buffConstructor.value = buff == buffType.Buff ? 2 : 0.5f;
        buffConstructor.modification = Buff.modificationType.Multiplicative;

        // If its a defense buff, make the value additive instead of multiplicative.
        // Because defense is calculated logarithmically, halving or doubling the defense score has a negligible effect.
        // In contrast, a defense of 100 halves damage, while a -50 doubles damage
        if (category == Buff.buffCategory.DEFENSE_BUFF) 
        { 
            buffConstructor.modification = Buff.modificationType.Additive;
            buffConstructor.value = buff == buffType.Buff ? 100 : 50;
        }
        
        buffConstructor.duration = duration;
        buffTarget.ApplyBuff(buffConstructor);
        buffUI.AddBuff(buffConstructor, buff, null);//Added by Z.S
    }

}
