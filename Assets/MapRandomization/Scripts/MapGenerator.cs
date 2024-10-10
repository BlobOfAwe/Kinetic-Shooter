using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private TransformStruct[] positionRows;

    [SerializeField]
    private GameObject[] chunks;

    private Transform[,] positions;

    private int[,] randomValues;

    [System.Serializable]
    private struct TransformStruct
    {
        public Transform[] transforms;
    }

    private void Awake()
    {
        positions = new Transform[positionRows[0].transforms.Length, positionRows.Length];
        randomValues = new int[positions.GetLength(0), positions.GetLength(1)];
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
                Instantiate(chunks[r], positions[x, y]);
            }
        }
    }

    public void GenerateNoDuplicate()
    {
        for (int x = 0; x < randomValues.GetLength(0); x++)
        {
            for (int y = 0; y < randomValues.GetLength(1); y++)
            {
                randomValues[x, y] = -1;
            }
        }

        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                int r = Random.Range(0, chunks.Length);
                for (int rx = 0; rx < randomValues.GetLength(0); rx++)
                {
                    if (chunks.Length < positions.GetLength(0) * positions.GetLength(1))
                    {
                        Debug.LogWarning("Preventing duplicate chunks is not supported when there are fewer chunks than there are squares in the grid.");
                        break;
                    }
                    for (int ry = 0; ry < randomValues.GetLength(1); ry++)
                    {
                        while (r == randomValues[rx, ry])
                        {
                            r = Random.Range(0, chunks.Length);
                            if (r != randomValues[rx, ry])
                            {
                                rx = 0;
                                ry = 0;
                            }
                        }
                    }
                }
                randomValues[x, y] = r;
                Instantiate(chunks[r], positions[x, y]);
            }
        }
    }
}
