// ## - NK
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float recoilForce = 1f;

    [SerializeField]
    private float fireInterval = 0f;

    [SerializeField]
    private GameObject bullet;

    private float fireTime = 0f;

    private bool isShooting = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;

        }
        if (context.canceled)
        {
            isShooting = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Update()
    {
        if (isShooting && fireTime <= 0f)
        {
            Debug.Log("Shoot!");
            Instantiate(bullet, transform.position, transform.rotation);
            rb.AddForce(-transform.up * recoilForce, ForceMode2D.Impulse);
            fireTime = fireInterval;
            //Calls on FMOD event "Gunshot" to play whenever player shoots
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerGunshot, this.transform.position);
        } else if (fireTime > 0f)
        {
            fireTime -= Time.deltaTime;
        }
    }
}
