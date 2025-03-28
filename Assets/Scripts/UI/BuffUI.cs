//Z.S
//The script manages the buff display, updating its icon, duration timer, and progress bar
//The buff icon chhanges over time based on defined intervals, which is at 20% right now to visually reflect the buff's remaining duration
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    public Image buffIcon;
    public Image durationFill;
    public TMP_Text durationText;
    private float duration;
    private float remainingDuration;
    private Buff buff;
    public BuffIconData buffIconData;
    private int lastIconIndex = -1;
    private GenericBuffDebuff.buffType currentBuffDebuff;


    public void Initialize(Buff buff, GenericBuffDebuff.buffType buffDebuff)
    {
        this.buff = buff;
        this.currentBuffDebuff = buffDebuff;
        buffIcon.sprite = buffIconData.GetIcon(buff.buffType, buffDebuff);
        duration = buff.duration;
        remainingDuration = duration;
        UpdateUI();
    }
        //Resets the buff's duration to a new value and updates the UI to show the change
        public void ResetDuration(float newDuration)
    {
        duration = newDuration;
        remainingDuration = duration;
        UpdateUI();
    }

    private void Update()
    {
        if (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            UpdateUI();

            if (remainingDuration <= 0)
            {
                FindObjectOfType<BuffUIManager>().RemoveBuff(buff);
            }
        }
    }
    //Updates the UI, counting down the remaining time and checking to see if the icon needs updating.
    private void UpdateUI()
    {
        durationFill.fillAmount = remainingDuration / duration;
        durationText.text = Mathf.Ceil(remainingDuration).ToString();
        float timePercentage = remainingDuration / duration;
        int iconIndex = Mathf.FloorToInt(timePercentage * 7);
        if (iconIndex != lastIconIndex)
        {
            lastIconIndex = iconIndex;
            buffIcon.sprite = GetIconForTimePercentage(iconIndex);
        }
    }
    // Get the respective icon based on the time remaining on around 14% intervals
    private Sprite GetIconForTimePercentage(int index)
    {
        if (index >= 0 && index < 7)
        {
            return buffIconData.GetIcon(buff.buffType, currentBuffDebuff, index);
        }
        else
        {
            return buffIconData.GetIcon(buff.buffType, currentBuffDebuff, 0);
        }
    }
}