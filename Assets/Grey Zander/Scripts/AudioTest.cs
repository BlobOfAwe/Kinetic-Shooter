using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioTest : MonoBehaviour
{
    [SerializeField] private EventReference spacePressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Space"))
        {
            AudioManager.instance.PlayOneShot(spacePressed, this.transform.position);
        }
    }
}
