using UnityEngine;

public class Cushion : Upgrade
{
    [SerializeField]
    private GameObject cushionShield;

    public override void ApplyUpgrade(int quantity)
    {
        base.ApplyUpgrade(quantity);
        if (FindObjectOfType<CushionShield>() == null)
        {
            Instantiate(cushionShield, player.transform);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.cushionActivateAbility, this.transform.position);
            Debug.Log("Shield activated.");
        } else
        {
            Debug.Log("Shield already active.");
        }
    }
}
