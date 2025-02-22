using UnityEngine;

public class CushionShield : MonoBehaviour
{
    [SerializeField]
    private float chargeSpeed = 1f;

    [SerializeField]
    private int shardsAmount = 1;

    [SerializeField]
    private GameObject shardPrefab;

    [SerializeField]
    private LayerMask solidLayers;

    [SerializeField]
    private Color chargedColor = Color.green;

    private Color originalColor = Color.white;

    private GameObject[] shards;

    private float shieldCharge = 0f;

    //private StandardPrimaryFire shootAbility;

    private SpriteRenderer sr;

    private PlayerBehaviour player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerBehaviour>();
        //shootAbility = player.GetComponent<StandardPrimaryFire>();
        originalColor = sr.color;
    }

    private void Start()
    {
        shards = new GameObject[shardsAmount];
        for (int i = 0; i < shards.Length; i++)
        {
            shards[i] = Instantiate(shardPrefab);
            shards[i].GetComponent<TestBullet>().shooter = gameObject;
            shards[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (player.isFiringPrimary)
        {
            if (shieldCharge < 1f)
            {
                shieldCharge += Time.deltaTime * chargeSpeed;
            } else
            {
                shieldCharge = 1f;
                sr.color = chargedColor;
            }
        }
        if (player.GetComponent<Collider2D>().IsTouchingLayers(solidLayers) && shieldCharge >= 1f)
        {
            Debug.Log("Shield shattered!");
            for (int i = 0; i < shardsAmount; i++)
            {
                foreach (GameObject shard in shards)
                {
                    if (!shard.activeSelf)
                    {
                        shard.transform.position = gameObject.transform.position;
                        shard.transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
                        shard.GetComponent<Projectile>().timeRemaining = shard.GetComponent<Projectile>().despawnTime;
                        //shard.GetComponent<Projectile>().speedMultiplier = shootAbility.bulletSpeedMultiplier;
                        //shard.GetComponent<Projectile>().knockbackMultiplier = shootAbility.bulletKnockbackMultiplier;
                        //shard.GetComponent<Projectile>().damageMultiplier = shootAbility.bulletDamageMultiplier;
                        shard.SetActive(true);
                        break;
                    }
                }
            }
            shieldCharge = 0f;
            sr.color = originalColor;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, shieldCharge);
        player.cushion = shieldCharge;
    }
}
