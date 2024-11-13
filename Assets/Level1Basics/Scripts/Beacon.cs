using UnityEngine;

public class Beacon : MonoBehaviour
{
    private Forcefield forcefield;

    private void Awake()
    {
        forcefield = GetComponent<Forcefield>();
    }

    public void Activate()
    {
        forcefield.gameObject.SetActive(true);
        Debug.Log("Beacon activated!");
    }
}
