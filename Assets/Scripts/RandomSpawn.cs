// ## - NK
using Pathfinding;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private Vector2 spawnPosition;

    private void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    public void Spawn()
    {
        if (mapGenerator != null)
        {
            spawnPosition.x = Random.Range(mapGenerator.levelXMin, mapGenerator.levelXMax);
            spawnPosition.y = Random.Range(mapGenerator.levelYMin, mapGenerator.levelYMax);
        } else
        {
            spawnPosition = Vector2.zero;
        }
        transform.position = spawnPosition;
        GraphNode node = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        transform.position = (Vector3)node.position;
        AstarPath.active.Scan();
    }
}
