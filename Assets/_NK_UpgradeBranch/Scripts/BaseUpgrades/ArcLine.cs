using UnityEngine;

public class ArcLine : MonoBehaviour
{
    private GameObject start;

    private GameObject end;

    private float time;

    private bool isSetUp = false;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isSetUp)
        {
            if (start != null && end != null)
            {
                lineRenderer.SetPosition(0, start.transform.position);
                lineRenderer.SetPosition(1, end.transform.position);
                time -= Time.deltaTime;
                if (time <= 0f)
                {
                    Destroy(gameObject);
                }
            } else
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetParameters(GameObject start, GameObject end, float time)
    {
        this.start = start;
        this.end = end;
        this.time = time;
        isSetUp = true;
    }
}
