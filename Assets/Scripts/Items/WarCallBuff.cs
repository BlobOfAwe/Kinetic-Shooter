// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCallBuff : MonoBehaviour
{
    public enum buffType { Damage, Heal, Speed }
    public buffType buff;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color recoverColor = Color.green;
    [SerializeField] private Color speedColor = Color.cyan;
    private ParticleSystem particleSys;
    private Enemy buffTarget;

    [SerializeField] private float damageBuff = 0.2f; // Multiply base damage stat by this value
    [SerializeField] private float recoverBuff = 20f; // Heal this much HP per second
    [SerializeField] private float speedBuff = 0.75f; // Multiply base speed stat by this value

    private void Awake()
    {
        particleSys = gameObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (buff == buffType.Heal)
        {
            try { buffTarget.Heal(recoverBuff * Time.deltaTime); } catch { }
        }
    }

    public void ApplyBuff(float duration)
    {
        buffTarget = GetComponentInParent<Enemy>();

        Buff buffConstructor = ScriptableObject.CreateInstance<Buff>(); // Create a new buff object

        // Get a reference to the indicator particle system
        var particleSysMain = particleSys.main;

        // Construct the buff object and assign the particle indicator colour
        switch (buff)
        {
            case buffType.Damage:
                buffConstructor.buffType = Buff.buffCategory.ATTACK_BUFF;
                buffConstructor.value = damageBuff;
                particleSysMain.startColor = damageColor;
                break;
            case buffType.Heal:
                particleSysMain.startColor = recoverColor;
                buffConstructor.buffType = Buff.buffCategory.RECOVER_BUFF;
                buffConstructor.value = recoverBuff;
                break;
            case buffType.Speed:
                particleSysMain.startColor = speedColor;
                buffConstructor.buffType = Buff.buffCategory.SPEED_BUFF;
                buffConstructor.value = speedBuff;
                break;
        }
        buffConstructor.modification = Buff.modificationType.Multiplicative;
        buffConstructor.duration = duration;
        buffTarget.ApplyBuff(buffConstructor);

        // Apply the buff to the entity
        StartCoroutine(ActiveBuff(duration));
    }

    IEnumerator ActiveBuff(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.parent = null;
        buffTarget = null;
        gameObject.SetActive(false);
    }
}
