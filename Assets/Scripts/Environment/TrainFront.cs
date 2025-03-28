using UnityEngine;

public class TrainFront : MonoBehaviour
{
    private Train train;

    private void Awake()
    {
        train = GetComponentInParent<Train>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>() != null)
        {
            train.TrainHit(collision.gameObject);
        }
    }
}
