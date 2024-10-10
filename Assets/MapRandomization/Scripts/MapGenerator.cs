using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TransformStruct[] positionRows;

    [SerializeField]
    private GameObject[] chunks;

    private Transform[,] positions;

    [System.Serializable]
    private struct TransformStruct
    {
        public Transform[] transforms;
    }

    private void Awake()
    {
        positions = new Transform[positionRows[0].transforms.Length, positionRows.Length];
        for (int y = 0; y < positionRows.Length; y++)
        {
            for (int x = 0; x < positionRows[y].transforms.Length; x++)
            {
                positions[x, y] = positionRows[y].transforms[x];
            }
        }
    }

    public void Generate()
    {
        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                int r = Random.Range(0, chunks.Length);
                Debug.Log("r[" + x + ", " + y + "] = " + r);
                Instantiate(chunks[r], positions[x, y]);
            }
        }
    }
}
