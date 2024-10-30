using UnityEngine;
using UnityEngine.UI;

public class HPBarSystem : MonoBehaviour
{
    public Image fillImage;
    public int maxHp = 100;
    private int currentHp;
    private float targetFill;
    public float fillSpeed = 5f; 

    void Start()
    {
        currentHp = maxHp;
        targetFill = 1f;
        fillImage.fillAmount = targetFill;
    }
    //Debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            HealHp(10);
        }
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        targetFill = (float)currentHp / maxHp;
    }

    public void HealHp(int amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        targetFill = (float)currentHp / maxHp;
    }
}