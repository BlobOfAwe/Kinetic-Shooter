using UnityEngine;

public class Geyser : MonoBehaviour
{
    [SerializeField]
    private float geyserDamage = 0f;

    [SerializeField]
    private float eruptionDuration = 1f;

    [SerializeField]
    private float eruptionInterval = 1f;

    [SerializeField]
    private float warningTime = 0f;

    [SerializeField]
    private float startDelayMax = 1f;

    [SerializeField]
    private Color normalColor = Color.white;

    [SerializeField]
    private Color warningColor = Color.white;

    [SerializeField]
    private Color eruptionColor = Color.white;

    [SerializeField]
    private string playerTag = "Player";

    private float timer = 0f;

    private bool isErupting = false;

    private bool isWarning = false;

    private PlayerBehaviour player;

    private Collider2D col;

    private SpriteRenderer sr;

    private void Awake()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        timer = Random.Range(0f, startDelayMax);
    }

    private void Update()
    {
        if (isErupting)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            } else
            {
                StopEruption();
            }
        } else
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (!isWarning && timer <= warningTime)
                {
                    EruptWarning();
                }
            } else
            {
                Erupt();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isErupting && collision.CompareTag(playerTag))
        {
            player.Damage(geyserDamage);
        }
    }

    private void Erupt()
    {
        isWarning = false;
        isErupting = true;
        col.enabled = true;
        timer = eruptionDuration;
        sr.color = eruptionColor;
    }

    private void EruptWarning()
    {
        isWarning = true;
        sr.color = warningColor;
    }

    private void StopEruption()
    {
        isErupting = false;
        col.enabled = false;
        timer = eruptionInterval;
        sr.color = normalColor;
    }
}
