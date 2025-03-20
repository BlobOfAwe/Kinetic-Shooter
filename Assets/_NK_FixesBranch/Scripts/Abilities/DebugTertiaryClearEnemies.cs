using UnityEngine;

public class DebugTertiaryClearEnemies : Ability
{
    [SerializeField]
    private Beacon beacon;

    protected override void Awake()
    {
        base.Awake();
        beacon = FindObjectOfType<Beacon>();
    }

    public override void OnActivate()
    {
        //Debug.Log("Cleared enemies.");
        if (beacon != null && !beacon.active)
        {
            beacon.Activate();
        }
    }
}
