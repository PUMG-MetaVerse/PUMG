using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.UI;

public class PartyUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button toggleButton;
    public GameObject partyObject;

    void Start()
    {
        // 버튼 클릭 시 TogglePartyVisibility 함수 호출
        toggleButton.onClick.AddListener(TogglePartyVisibility);
    }

    void TogglePartyVisibility()
    {
        // Party 오브젝트의 활성 상태를 토글
        partyObject.SetActive(!partyObject.activeSelf);
    }
}
