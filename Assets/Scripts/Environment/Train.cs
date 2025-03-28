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

    [SerializeField]
    private float damage = 0f;

    [SerializeField]
    private Vector2 impactForceDirPos = Vector2.zero;

    [SerializeField]
    private Vector2 impactForceDirNeg = Vector2.zero;

    [SerializeField]
    private float impactForceStrength = 0f;

    [SerializeField]
    private BoxCollider2D trainFrontCollider;

    [SerializeField]
    private bool isHorizontal = true;

    [SerializeField]
    private LayerMask collideLayerMask;

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

        /*if (hitInfo != null && hitInfo.gameObject.GetComponent<Entity>() != null)
        {
            hitInfo.gameObject.GetComponent<Entity>().Damage(damage);
            if (hitInfo.gameObject.GetComponent<PlayerBehaviour>() != null)
            {
                PlayerBehaviour player = hitInfo.gameObject.GetComponent<PlayerBehaviour>();
                player.rb.isKinematic = false;
                player.canMoveManually = true;
                player.playerAnimator.SetBool("isHunkered", false);
            }
            Vector2 impactForce = impactForceDir * impactForceStrength;
            if (isHorizontal && (Random.Range(0, 2) == 1))
            {
                impactForce.y = -impactForce.y;
            }
            if (isVertical && (Random.Range(0, 2) == 1))
            {
                impactForce.x = -impactForce.x;
            }
            hitInfo.gameObject.GetComponent<Rigidbody2D>().AddForce(impactForce);
        }*/
    }

    public void TrainHit(GameObject hitObject)
    {
        Vector2 posDir;
        if (isHorizontal)
        {
            posDir = Vector2.up;
        }
        else
        {
            posDir = Vector2.right;
        }
        Collider2D hitInfoPos = Physics2D.OverlapBox((Vector2)trainFrontCollider.transform.position + trainFrontCollider.offset + (posDir * trainFrontCollider.size / 4f), trainFrontCollider.size - (posDir * trainFrontCollider.size / 2f), 0f, collideLayerMask);
        Collider2D hitInfoNeg = Physics2D.OverlapBox((Vector2)trainFrontCollider.transform.position + trainFrontCollider.offset - (posDir * trainFrontCollider.size / 4f), trainFrontCollider.size - (posDir * trainFrontCollider.size / 2f), 0f, collideLayerMask);
        if (hitInfoPos != null && hitInfoNeg != null && hitInfoPos.gameObject == hitObject && hitInfoNeg.gameObject == hitObject)
        {
            Debug.Log("Hit center of train.");
            hitObject.GetComponent<Entity>().Damage(damage);
            if (hitObject.GetComponent<PlayerBehaviour>() != null)
            {
                PlayerBehaviour player = hitObject.GetComponent<PlayerBehaviour>();
                player.rb.isKinematic = false;
                player.canMoveManually = true;
                player.playerAnimator.SetBool("isHunkered", false);
            }
            if (Random.Range(0, 2) == 0)
            {
                hitObject.GetComponent<Rigidbody2D>().AddForce(impactForceDirPos * impactForceStrength);
            }
            else
            {
                hitObject.GetComponent<Rigidbody2D>().AddForce(impactForceDirNeg * impactForceStrength);
            }
        } else if (hitInfoPos != null && hitInfoPos.gameObject == hitObject)
        {
            Debug.Log("Hit positive facing side of train.");
            hitObject.GetComponent<Entity>().Damage(damage);
            if (hitObject.GetComponent<PlayerBehaviour>() != null)
            {
                PlayerBehaviour player = hitObject.GetComponent<PlayerBehaviour>();
                player.rb.isKinematic = false;
                player.canMoveManually = true;
                player.playerAnimator.SetBool("isHunkered", false);
            }
            hitObject.GetComponent<Rigidbody2D>().AddForce(impactForceDirPos * impactForceStrength);
        } else if (hitInfoNeg != null && hitInfoNeg.gameObject == hitObject)
        {
            Debug.Log("Hit negative facing side of train.");
            hitObject.GetComponent<Entity>().Damage(damage);
            if (hitObject.GetComponent<PlayerBehaviour>() != null)
            {
                PlayerBehaviour player = hitObject.GetComponent<PlayerBehaviour>();
                player.rb.isKinematic = false;
                player.canMoveManually = true;
                player.playerAnimator.SetBool("isHunkered", false);
            }
            hitObject.GetComponent<Rigidbody2D>().AddForce(impactForceDirNeg * impactForceStrength);
        } else
        {
            Debug.LogWarning("Overlap box collisions do not match hit object.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 posDir;
        if (isHorizontal)
        {
            posDir = Vector2.up;
        } else
        {
            posDir = Vector2.right;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)trainFrontCollider.transform.position + trainFrontCollider.offset - (posDir * trainFrontCollider.size / 4f), trainFrontCollider.size - (posDir * trainFrontCollider.size / 2f));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)trainFrontCollider.transform.position + trainFrontCollider.offset + (posDir * trainFrontCollider.size / 4f), trainFrontCollider.size - (posDir * trainFrontCollider.size / 2f));
    }
}
