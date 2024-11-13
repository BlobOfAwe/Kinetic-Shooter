// ## - NK
//using System;
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

    [SerializeField]
    private GameObject[] itemPrefabs;

    [SerializeField]
    private int minItems = 0;

    [SerializeField]
    private int maxItems = 0;

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

    // Pure random generation. No longer used.
    /*public void Generate()
    {
        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                int r = Random.Range(0, chunks.Length);
                Instantiate(chunks[r], positions[x, y]);
            }
        }
    }*/

    // No duplicates. No longer used.
    /*public void GenerateNoDuplicate()
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
    }*/

    // No adjacent chunks. Previously called GenerateNoAdjacent()
    public void Generate()
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

                int left;
                if (x - 1 >= 0)
                {
                    left = randomValues[x - 1, y];
                } else
                {
                    left = -1;
                }
                int right;
                if (x + 1 < positions.GetLength(0))
                {
                    right = randomValues[x + 1, y];
                } else
                {
                    right = -1;
                }
                int down;
                if (y - 1 >= 0)
                {
                    down = randomValues[x, y - 1];
                } else
                {
                    down = -1;
                }
                int up;
                if (y + 1 < positions.GetLength(1))
                {
                    up = randomValues[x, y + 1];
                } else
                {
                    up = -1;
                }

                if (chunks.Length > 1)
                {
                    while (r == left || r == right || r == down || r == up)
                    {
                        r = Random.Range(0, chunks.Length);
                    }
                } else
                {
                    Debug.LogWarning("There must be more than one chunk in order to prevent adjacent chunks.");
                }
                randomValues[x, y] = r;
                float rotation = 0f;
                if (Random.Range(0, 2) == 1)
                {
                    rotation = 180f;
                }
                Instantiate(chunks[r], positions[x, y].position, Quaternion.Euler(0f, rotation, 0f));
            }
        }

        // Recalculate A* graph
        AstarPath.active.Scan();

        // Spawn items around the map.
        int itemAmount = Random.Range(minItems, maxItems);
        for (int i = 0; i < itemAmount; i++)
        {
            Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
        }

        // Find and update all positions of randomly spawning objects.
        RandomSpawn[] randomSpawns = FindObjectsOfType<RandomSpawn>();
        for (int i = 0; i < randomSpawns.Length; i++)
        {
            randomSpawns[i].Spawn();
        }
    }
}
