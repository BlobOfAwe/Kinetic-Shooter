// ## - NK
using Pathfinding;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    [SerializeField]
    private float minY;

    [SerializeField]
    private float maxY;

    private Vector2 spawnPosition;

    private void Start()
    {
        spawnPosition.x = Random.Range(minX, maxX);
        spawnPosition.y = Random.Range(minY, maxY);
        transform.position = spawnPosition;
        GraphNode node = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        transform.position = (Vector3)node.position;
    }
}
