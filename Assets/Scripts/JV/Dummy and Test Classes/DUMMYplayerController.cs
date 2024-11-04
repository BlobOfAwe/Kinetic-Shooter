// ## - JV
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DUMMYplayerController : MonoBehaviour
{
    Rigidbody2D rb;

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
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * topSpeed;
    }

    private void LateUpdate()
    {
        cam.transform.position = transform.position;
    }
}
