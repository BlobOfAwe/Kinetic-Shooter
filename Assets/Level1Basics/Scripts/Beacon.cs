using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField]
    private Forcefield forcefield;

    /*private void Awake()
    {
        forcefield = GetComponentInChildren<Forcefield>();
    }*/

    public void Activate()
    {
        Debug.Log("Beacon activated!");
        forcefield.gameObject.SetActive(true);
    }
}
