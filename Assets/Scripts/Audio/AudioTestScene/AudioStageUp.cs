using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStageUp : MonoBehaviour
{
    [SerializeField] AudioParameterController parameterController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        parameterController.IncrementStage();
    }
}
