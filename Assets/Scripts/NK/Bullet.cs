// ## - NK
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float despawnTime = 1f;

    private float timeRemaining;

    private void Awake()
    {
        timeRemaining = despawnTime;
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
