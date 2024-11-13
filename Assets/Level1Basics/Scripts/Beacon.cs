using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField]
    private Forcefield forcefield;

    public void Activate()
    {
        Debug.Log("Beacon activated!");
        forcefield.gameObject.SetActive(true);
    }
}
