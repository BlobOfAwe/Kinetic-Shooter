using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] chunks;

    [SerializeField]
    private Transform[] positions;

    public void Generate()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int r = Random.Range(0, chunks.Length);
            Debug.Log("r" + i + " = " + r);
            Instantiate(chunks[r], positions[i]);
        }
    }
}
