using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DUMMYplayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float reloadTime = 1.0f;
    [SerializeField] float force = 1.0f;
    [SerializeField] float cooldown = 0f;
    [SerializeField] float topSpeed = 30f;
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        FaceMouse();
        if (cooldown <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0)) { Shoot(); }
        }
        else { cooldown -= Time.deltaTime; }
    }

    private void LateUpdate()
    {
        cam.transform.position = transform.position;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > topSpeed) { rb.velocity = rb.velocity.normalized * topSpeed; }
    }

    void FaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = dir;
    }

    void Shoot()
    {
        cooldown += reloadTime;

        rb.AddForce(-transform.up * force, ForceMode2D.Impulse);

    }
}
