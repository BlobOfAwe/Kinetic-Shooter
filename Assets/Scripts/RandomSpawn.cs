// ## - NK
using Pathfinding;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField]
    private bool wholeMap = true;

    [SerializeField]
    private float xMin = 0f;

    [SerializeField]
    private float xMax = 0f;

    [SerializeField]
    private float yMin = 0f;

    [SerializeField]
    private float yMax = 0f;

    private MapGenerator mapGenerator;

    private Vector2 spawnPosition;

    private void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    public void Spawn()
    {
        if (wholeMap && mapGenerator != null)
        {
            spawnPosition.x = Random.Range(mapGenerator.levelXMin, mapGenerator.levelXMax);
            spawnPosition.y = Random.Range(mapGenerator.levelYMin, mapGenerator.levelYMax);
        }
        else
        {
            spawnPosition.x = Random.Range(xMin, xMax);
            spawnPosition.y = Random.Range(yMin, yMax);
        }
        transform.position = spawnPosition;
        GraphNode node = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        transform.position = (Vector3)node.position;
        AstarPath.active.Scan();
    }
}
