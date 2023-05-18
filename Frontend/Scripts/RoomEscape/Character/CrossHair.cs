using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public static bool crossHairActivated = true;

    // 특정 상호작용 전에 크로스헤어가 활성화되어있었는가?
    public static bool preIsCrossHair;

    public static void ToggleCrossHair()
    {
        GameObject FingerCursor = GameObject.Find("CrossHair").transform.Find("Dot").gameObject;
        crossHairActivated = !crossHairActivated;

        if (crossHairActivated)
        {
            FingerCursor.SetActive(true);
        }
        else
        {
            FingerCursor.SetActive(false);
        }
    }
}
