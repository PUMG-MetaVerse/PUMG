using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpManager : MonoBehaviour
{
    public GameObject Help_Pannel;
    public TMP_InputField ChatInput;
    public TMP_InputField PartyMemberInput;

    bool checkHelp=true;
    // Start is called before the first frame update
    void Start()
    {
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
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
    public bool IsSystemUIActive()
    {
        return checkHelp;
    }
    void Update()
    {
        // Debug.Log($"test : {PartyMemberInput==null}");
        if (Input.GetKeyDown(KeyCode.H) && !ChatInput.isFocused && (PartyMemberInput == null || !PartyMemberInput.isFocused))
        {
            // "H" 키가 눌렸을 때 실행할 코드를 이곳에 작성합니다.
            // Debug.Log("H key was pressed.");

            Debug.Log($"체크헬프 : {checkHelp}");
            // checkHelp = !checkHelp;

            if(checkHelp)
            {
                Help_Pannel.SetActive(false);
                Cursor.visible = false;
                checkHelp = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Help_Pannel.SetActive(true);
                Cursor.visible = true;
                checkHelp = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
        }
    }
    public void CloseHelp()
    {
        Debug.Log($"체크헬프 : {checkHelp}");
        checkHelp = false;
        Help_Pannel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
