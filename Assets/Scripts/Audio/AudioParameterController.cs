// ## - GZ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParameterController : MonoBehaviour
{
    [Header("Paramater Change")]
    [SerializeField] private string parameterNameIntensity;
    [SerializeField] private float parameterValueIntensity;

    [SerializeField] private string parameterNameStage;
    [SerializeField] private float parameterValueStage;

    [SerializeField] private string parameterNamePause;
    [SerializeField] private float parameterValuePause;

    [SerializeField] private string parameterNameEnding;
    [SerializeField] private float parameterValueEnding;

    public void IncrementIntensity()
    {
        if (parameterValueIntensity < 3)
        {
            parameterValueIntensity++;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
        
    }
    public void DecreaseIntensity()
    {
        if (parameterValueIntensity > 0)
        {
            parameterValueIntensity--;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
      
    }
    public void IncrementStage()
    {
        parameterValueStage ++;
        AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
    }
    public void EndingWin()
    {
        if (parameterValueStage == 4)
        {
            parameterValueEnding = 1;
            AudioManager.instance.SetMusicIntensity(parameterNameEnding, parameterValueEnding);
        }
        
    }

    public void EndingLoss()
    {
        parameterValueEnding = 2;
        AudioManager.instance.SetMusicIntensity(parameterNameEnding, parameterValueEnding);
    }

}
