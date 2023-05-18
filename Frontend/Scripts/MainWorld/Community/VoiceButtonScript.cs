using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class VoiceButtonScript : MonoBehaviour
{
    public Animator voiceMemberListAnimator;
    private Button button;
    private bool isVoiceListVisible = false;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleVoiceList);
    }

    public void ToggleVoiceList()
    {
        if (isVoiceListVisible)
        {
            // Debug.Log(friendListAnimator);
            voiceMemberListAnimator.SetTrigger("hide"); // 'Hide' 트리거를 설정하여 친구 창을 닫는 애니메이션을 재생
        }
        else
        {
            voiceMemberListAnimator.SetTrigger("show"); // 'Show' 트리거를 설정하여 친구 창을 여는 애니메이션을 재생
        }

        isVoiceListVisible = !isVoiceListVisible;
    }
}