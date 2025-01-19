using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : Ability
{
    [SerializeField] private float countdown = 1f;
    [SerializeField] private float dropoff = 2f; // A value of 1 indicates linear drop-off. The greater the value, the slower the initial dropoff
    [SerializeField] private ParticleSystem particles;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<Enemy>().sprite;
    }
    public override void OnActivate()
    {
        available = false;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        thisEntity.totalSpeed *= 0.2f;
        sprite.color = Color.red;
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
        particles.transform.parent = null;
        particles.gameObject.SetActive(true);
        particles.Play();
        // explosion audio
        AudioManager.instance.PlayOneShot(FMODEvents.instance.grenadeExplosionAbility, this.transform.position);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        Destroy(particles.gameObject);
        Destroy(gameObject);
    }

    public override void PurgeDependantObjects()
    {
        Destroy(particles.gameObject);
    }

}
