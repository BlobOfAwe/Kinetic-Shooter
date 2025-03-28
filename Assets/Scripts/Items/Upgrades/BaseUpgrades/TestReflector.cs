using UnityEngine;

public class TestReflector : MonoBehaviour
{
    [SerializeField]
    private float secondsUntilDestroy = 1f;

    private void Update()
    {
        secondsUntilDestroy -= Time.deltaTime;
        if (secondsUntilDestroy <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
