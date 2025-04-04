using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : MonoBehaviour
{
    // Damage the player on contact and disappear
    [SerializeField]
    private LayerMask shootableLayer;
    [SerializeField] private float speedDebuffMod = 0.5f;
    [SerializeField] private float debuffDuration = 10f;
    [SerializeField] private float damage = 10f;
    private BuffUIManager buffUI;

    private void Start()
    {
        
        buffUI = FindObjectOfType<BuffUIManager>();
    }

    // When the bullet collides with something, disable it
    // Changed so that bullet is only disabled if it collides with something that matches a specific layer. - NK
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Entity damageable = collision.gameObject.GetComponent<Entity>();
            if (damageable != null)
            {
                damageable.Damage(damage);
                Buff speedDebuff = ScriptableObject.CreateInstance<Buff>();
                speedDebuff.buffType = Buff.buffCategory.SPEED_BUFF;
                speedDebuff.value = speedDebuffMod;
                speedDebuff.modification = Buff.modificationType.Multiplicative;
                speedDebuff.duration = debuffDuration;
                damageable.ApplyBuff(speedDebuff);
                buffUI.AddBuff(speedDebuff, GenericBuffDebuff.buffType.Debuff, null);//Added by Z.S
            }
        }
    }
}