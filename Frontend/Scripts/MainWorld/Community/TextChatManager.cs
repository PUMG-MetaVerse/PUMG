using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextChatManager : MonoBehaviourPun
{
    public string username; // 사용자 이름
    public int maxMessages = 25; // 채팅창에서 보여질 최대 메시지 수
    public TMP_FontAsset chatFont; // 채팅 글꼴
    public GameObject chatPanel, chatEntry; // 채팅 패널 및 채팅 항목 게임 오브젝트
    public TMP_InputField chatBox; // 채팅 입력 상자
    public ScrollRect scrollRect; // 채팅창의 스크롤 뷰

    public Color mine; // 내 메시지의 색상
    public Color others; // 다른 사용자의 메시지 색상
    private bool InputFieldCheck; // 입력 상자의 상태를 확인하기 위한 변수
    [SerializeField] private List<Message> messageList = new List<Message>(); // 메시지 목록

    void Start()
    {
        username = PhotonNetwork.NickName; // 사용자 이름 설정
        photonView.RPC("joinMessageToChat", RpcTarget.All, PhotonNetwork.NickName); // 입장 메시지 전송
    }

    void Update()
    {
        // Return 키를 누르면
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 입력 상자의 텍스트가 비어 있지 않으면
            if (chatBox.text != "")
            {
                Debug.Log("Text Out");
                // 메시지 전송
                photonView.RPC("sendMessageToChat", RpcTarget.All, PhotonNetwork.NickName, chatBox.text);
                chatBox.text = ""; // 입력 상자 초기화
                EventSystem.current.SetSelectedGameObject(null); // 입력 상자 포커스 해제
                InputFieldCheck = false;
            }
            else
            {
                // 입력 상자가 비어 있고 포커스가 없으면
                if (!InputFieldCheck)
                {
                    Debug.Log("!chatBox");
                    chatBox.ActivateInputField(); // 입력 상자 활성화
                    InputFieldCheck = true;
                }
                else
                {
                    Debug.Log("chatBox");
                    chatBox.text = ""; // 입력 상자 초기화
                    chatBox.DeactivateInputField(); // 입력 상자 비활성화
                    InputFieldCheck = false;
                }
            }
        }
    }

    [PunRPC]
    public void sendMessageToChat(string sender, string text)
    {
        // 메시지 목록이 최대 메시지 수를 초과하면
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject); // 가장 오래된 메시지 삭제
            messageList.Remove(messageList[0]); // 메시지 목록에서 제거
        }
        Message newMessage = new Message();
        newMessage.text = sender + ": " + text; // 메시지 텍스트 생성

        GameObject newText = Instantiate(chatEntry, chatPanel.transform); // 새로운 채팅 항목 인스턴스 생성
        newMessage.textObject = newText.GetComponent<TMPro.TextMeshProUGUI>(); // 텍스트 컴포넌트 가져옴
        newMessage.textObject.text = newMessage.text; // 메시지 텍스트 설정
        newMessage.textObject.color = MessageTypeColor(sender); // 메시지의 색상을 결정하는 함수 호출
        newMessage.textObject.font = chatFont; // 글꼴 설정
        newMessage.textObject.fontSize = 25; // 글자 크기 설정
        messageList.Add(newMessage); // 메시지 목록에 추가
        
        Invoke("MoveScrollToBottom", 0.1f); // 스크롤을 아래로 이동하는 함수 호출
    }

    // 메시지 색상을 결정하는 함수
    Color MessageTypeColor(string sender)
    {
        Color color = sender == username ? mine : others; // 사용자가 자신이면 mine 색상, 아니면 others 색상 사용
        color.a = 1f;
        return color;
    }

    // 스크롤을 아래로 이동하는 함수
    void MoveScrollToBottom()
    {
        scrollRect.verticalNormalizedPosition = 0.0f; // 스크롤 위치를 맨 아래로 설정
    }
}

[System.Serializable]
public class Message
{
public string text; // 메시지 텍스트
public TextMeshProUGUI textObject; // 메시지의 TextMeshProUGUI 컴포넌트
}