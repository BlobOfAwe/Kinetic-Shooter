using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Projectile
{
    [SerializeField]
    private LayerMask shootableLayer;
    [SerializeField]
    private float range;
    [SerializeField]
    private ParticleSystem particles;
    private SpriteRenderer sprite;

    protected override void Start()
    {
        base.Start();
        //particles = GetComponentInChildren<ParticleSystem>(true);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject != shooter)
        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, range);
            particles.gameObject.SetActive(true);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.rocketImpactAbility, this.transform.position);
            particles.Play();
            foreach (Collider2D obj in objs)
            {
                if (collision.gameObject.GetComponent<Enemy>())
                {
                    collision.gameObject.GetComponent<Enemy>().Damage(damageMultiplier * shooterEntity.totalAttack);
                }
            }

            collider.enabled = false;
            sprite.enabled = false;
            
            StartCoroutine(HideMine());
        }
    }

    private IEnumerator HideMine()
    {
        yield return new WaitForSeconds(particles.main.startLifetime.constant);
        gameObject.SetActive(false);
        particles.gameObject.SetActive(false);
        collider.enabled = true;
        sprite.enabled = true;
    }

}