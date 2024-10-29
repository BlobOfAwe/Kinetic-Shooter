using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestMenu : MonoBehaviour
{
    public void AudioTest()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.meowMoment, this.transform.position);
    }

    public void AudioTest2()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.chickenMoment, this.transform.position);
    }

}
