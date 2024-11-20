// ## - NK
using UnityEngine;

public class TestUpgrade : Upgrade
{
    [SerializeField]
    private float speedMultiplier = 1f;

    protected override void ApplyUpgrade()
    {
        player.totalSpeed *= speedMultiplier; // This needs to be changed.
        Debug.Log("Applied super speed!");
    }
}
