using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : Ability
{
    [SerializeField] private float countdown = 1f;
    [SerializeField] private float dropoff = 2f; // A value of 1 indicates linear drop-off. The greater the value, the slower the initial dropoff


    public override void OnActivate()
    {
        available = false;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        thisEntity.totalSpeed *= 0.5f;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(countdown);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var obj in colliders)
        {
            Entity targetEntity = obj.gameObject.GetComponent<Entity>();
            if (targetEntity != null && obj.gameObject != this.gameObject)
            {
                float explosionDamage = (thisEntity.totalAttack * damageModifier) * (1 - (Mathf.Pow(Vector2.Distance(transform.position, targetEntity.transform.position)/range, dropoff)));
                Debug.Log("explosionDamage for entity " + targetEntity.gameObject.name + " at " + (Vector2.Distance(transform.position, targetEntity.transform.position) / range) * 100 + "% of range equals " + explosionDamage);
                targetEntity.Damage(explosionDamage);
            }
        }
        Destroy(gameObject);
    }

}
