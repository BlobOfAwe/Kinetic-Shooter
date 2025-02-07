// ## - JV
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Ability : MonoBehaviour
{
    public float cooldown = 0.5f; // Changed to public so it can be accessed by upgrades. - NK
    public float cooldownMultiplier = 1f; // Added this so that cooldown can be multiplied by upgrades. - NK
    public bool available = true;
    public bool inUse = false; // Is the ability currently being used?
    private float cooldownTimer;
    public float range = 3f;
    [SerializeField] protected float damageModifier = 1f; // Multiplies Entity.baseDamage. 1 = 100% of base damage
    protected Entity thisEntity;

    protected void Awake()
    {
        thisEntity = GetComponent<Entity>();
    }

    public abstract void OnActivate();

    // Called on an Entity's death to destroy any objects created by the ability
    // Does nothing by default, and must be overriden by child class
    public virtual void PurgeDependantObjects()
    {
        return;
    }

    public IEnumerator BeginCooldown()
    {
        available = false;
        yield return new WaitForSeconds(cooldown * cooldownMultiplier);
        available = true;
    }


    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
