using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPos = Vector2.zero;

    [SerializeField]
    private Vector2 endPos = Vector2.zero;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float trainInterval = 0f;

    private float nextTrain = 0f;

    private void Start()
    {
        transform.position = startPos;
        nextTrain = trainInterval;
    }

    private void Update()
    {
        if (nextTrain > 0f)
        {
            nextTrain -= Time.deltaTime;
        } else
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        }

        if ((Vector2)transform.position == endPos)
        {
            transform.position = startPos;
            nextTrain = trainInterval;
        }
    }
}
