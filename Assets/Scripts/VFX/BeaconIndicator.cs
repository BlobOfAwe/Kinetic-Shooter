using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeaconIndicator : MonoBehaviour
{
    private VisualEffect beaconVfx;

    void Start()
    {
        beaconVfx = GetComponent<VisualEffect>();
        beaconVfx.SetFloat("SpawnRate", 0); 
    }

    public void StartBurst()
    {
        beaconVfx.SetFloat("SpawnRate", 6000);
    }
    public void StopBurst()
    {
        beaconVfx.SetFloat("SpawnRate", 0);
    }
}
