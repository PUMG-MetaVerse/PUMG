using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemover : MonoBehaviour
{
    [HideInInspector] public bool isStart_Input;
    [HideInInspector]public float duration_Input;
    float Timer;

    void Update()
    {
        RemoveEffect(isStart_Input, duration_Input);
    }

    void RemoveEffect(bool isStart, float duration)
    {
        if (isStart)
        {
            if (Timer < duration)
            {
                Timer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
