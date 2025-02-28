using UnityEngine;

public class Portawall : MonoBehaviour
{
    public float despawnTime;
    
    [HideInInspector]
    public float timeRemaining;

    protected virtual void Update()
    {

        // Despawns the projectile automatically after a set amount of time has passed without it hitting anything.
        // If despawnTime is set to 0 or a negative number, there is no time limit.
        if (despawnTime > 0f)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}