using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CityMiniMapController : MonoBehaviour
{
    public GameObject miniMapUI;
    public TMP_InputField ChatInput;
    public TMP_InputField PartyMemberInput;
    // Start is called before the first frame update
    void Start() { 
        GameObject inputFieldObject = GameObject.Find("ChatBox"); // InputField GameObject의 이름을 지정합니다.
        if (inputFieldObject != null)
        {
            ChatInput = inputFieldObject.GetComponent<TMP_InputField>(); // InputField 컴포넌트를 가져옵니다.
        }
        GameObject partyInputObject = GameObject.Find("PartyInput");
        if (partyInputObject != null)
        {
            PartyMemberInput = partyInputObject.GetComponent<TMP_InputField>(); // InputField 컴포넌트를 가져옵니다.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !ChatInput.isFocused && (PartyMemberInput == null || !PartyMemberInput.isFocused))
        {
            miniMapUI.SetActive(!miniMapUI.activeSelf);
        }
    }
}
