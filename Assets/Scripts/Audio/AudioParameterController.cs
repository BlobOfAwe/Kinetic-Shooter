// ## - GZ
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
        if (parameterValueStage < 4)
        {
            parameterValueStage++;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
        }
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

    public void Paused()
    {
        parameterValuePause = 1;
        AudioManager.instance.SetMusicIntensity(parameterNamePause, parameterValuePause);
    }

    public void Unpaused()
    {
        parameterValuePause = 0;
        AudioManager.instance.SetMusicIntensity(parameterNamePause, parameterValuePause);
    }

    //temporary intensity

    public void IntensityOne()
    {
        parameterValueIntensity = 1;
        AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
    }
    public void IntensityTwo()
    {
        parameterValueIntensity = 2;
        AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
    }

    public void IntensityThree()
    {
        parameterValueIntensity = 3;
        AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
    }

    //temporary stage

    public void StageOne()
    {
        parameterValueStage = 1;
        AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
    }
    public void StageTwo()
    {
        parameterValueStage = 2;
        AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
    }
    public void StageThree()
    {
        parameterValueStage = 3;
        AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
    }
    public void StageFour()
    {
        parameterValueStage = 4;
        AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
    }
}
