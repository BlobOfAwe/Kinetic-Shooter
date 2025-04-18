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
            Instantiate(cushionShield, player.transform).GetComponent<CushionShield>().quantity = quantity;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.cushionActivateAbility, this.transform.position);
            Debug.Log("Shield activated.");
        } else
        {
            FindObjectOfType<CushionShield>().quantity = quantity;
            Debug.Log("Shield upgraded.");
        }
    }
}
