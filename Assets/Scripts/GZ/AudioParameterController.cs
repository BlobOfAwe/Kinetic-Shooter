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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            parameterValueIntensity--;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            parameterValueIntensity++;
            AudioManager.instance.SetMusicIntensity(parameterNameIntensity, parameterValueIntensity);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            parameterValueStage--;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            parameterValueStage++;
            AudioManager.instance.SetMusicIntensity(parameterNameStage, parameterValueStage);
        }
    }


}
