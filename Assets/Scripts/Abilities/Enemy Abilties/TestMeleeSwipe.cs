// ## - JV
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMeleeSwipe : Ability
{
    private BoxCollider2D hitbox;

    public float knockback;
    public float activeTime;

    // Create a gameObject as a child of this gameObject and add a BoxCollider2D trigger based on the ability's stats, then disable it
    void Awake()
    {
        GameObject hitboxObj = new GameObject("TestMeleeSwipeHitbox", typeof(BoxCollider2D));
        hitboxObj.transform.parent = transform;
        hitboxObj.transform.rotation = Quaternion.identity;
        hitboxObj.transform.position = transform.position;
        
        
        hitbox = hitboxObj.GetComponent<BoxCollider2D>();
        hitbox.size = new Vector2(hitbox.size.x, range);
        hitbox.offset = Vector2.up * (range / 2 + 0.5f);
        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    // Apply knockback to an object detected by the active hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        try { collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockback, ForceMode2D.Impulse); } catch { }
    }

    // Enable the hitbox for activeTime seconds
    public override void OnActivate()
    {
        hitbox.enabled = true;
        StartCoroutine(BeginCooldown());
        StartCoroutine(DisableAfterSeconds());
    }

    IEnumerator DisableAfterSeconds()
    {
        yield return new WaitForSeconds(activeTime);
        hitbox.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (hitbox && hitbox.enabled) 
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector2.zero + hitbox.offset, hitbox.size);
        }
    }
}
