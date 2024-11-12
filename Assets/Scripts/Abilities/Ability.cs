// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float cooldown = 0.5f;
    public bool available = true;
    private float cooldownTimer;
    public float range = 3f;
    [SerializeField] protected float damageModifier = 1f; // Multiplies Entity.baseDamage. 1 = 100% of base damage

    public abstract void OnActivate();

    protected IEnumerator BeginCooldown()
    {
        available = false;
        yield return new WaitForSeconds(cooldown);
        available = true;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
