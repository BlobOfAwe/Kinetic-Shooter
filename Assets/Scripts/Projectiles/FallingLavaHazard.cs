using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class FallingLavaHazard : Item
{
    [SerializeField] float size;
    [SerializeField]
    private LayerMask shootableLayer;
    [SerializeField] private ParticleSystem particles;
    public float damageMultiplier = 1f; // Deals damage based on the totalAttack of the Shooter // Changed to public. - NK
    protected Entity shooterEntity;
    [SerializeField] private Animator animator;

    protected override void Start()
    {
        _INIT();
    }
    // SO That this can be called from other scripts, as well as on Start
    public void _INIT()
    {
        particles = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
    }
    public void Fall()
    {
        animator.SetTrigger("fall");
    }

    public void Impact()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, size, shootableLayer);
        if (player != null) { player.GetComponent<Entity>().Damage(shooterEntity.totalAttack * damageMultiplier); }
        particles.Stop();
        particles.Play();
        StartCoroutine(HideAfterImpact());
    }

    private IEnumerator HideAfterImpact()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}