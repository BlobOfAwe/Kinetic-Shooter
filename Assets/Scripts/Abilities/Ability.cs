// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public float cooldown = 0.5f;
    public bool available = true;
    private float cooldownTimer;
    public float range = 3f;

    public abstract void OnActivate();

    public IEnumerator BeginCooldown()
    {
        available = false;
        yield return new WaitForSeconds(cooldown);
        available = true;
    }
}
