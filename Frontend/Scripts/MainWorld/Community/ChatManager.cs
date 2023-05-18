// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Photon.Pun;
// using Photon.Chat;

// public class ChatManager : MonoBehaviour, IChatClientListener
// {
//     public InputField chatInputField;
//     public Text chatText;
//     public ScrollRect scrollRect;
//     private ChatClient chatClient;
//     private string chatRoomName = "GlobalChatRoom";

//     private void Start()
//     {
//         chatClient = new ChatClient(this);
//         chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName));
//     }

//     private void Update()
//     {
//         if (chatClient != null)
//         {
//             chatClient.Service();
//         }

//         if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
//         {
//             if (chatInputField.isFocused)
//             {
//                 if (!string.IsNullOrWhiteSpace(chatInputField.text))
//                 {
//                     chatClient.PublishMessage(chatRoomName, chatInputField.text);
//                     chatInputField.text = "";
//                 }
//             }
//             else
//             {
//                 chatInputField.ActivateInputField();
//             }
//         }
//     }

//     public void OnConnected()
//     {
//         chatClient.Subscribe(new string[] { chatRoomName });
//     }

//     public void OnDisconnected()
//     {
//     }

//     public void OnChatStateChange(ChatState state)
//     {
//     }

//     public void OnGetMessages(string channelName, string[] senders, object[] messages)
//     {
//         for (int i = 0; i < senders.Length; i++)
//         {
//             string sender = senders[i];
//             string message = messages[i].ToString();
//             AddMessageToChat($"{sender}: {message}");
//         }
//     }
//     public void OnPrivateMessage(string sender, object message, string channelName)
//     {
//     }

//     public void OnSubscribed(string[] channels, bool[] results)
//     {
//         foreach (string channel in channels)
//         {
//             chatClient.PublishMessage(channel, $"{PhotonNetwork.NickName} joined the chat.");
//         }
//     }

//     public void OnUnsubscribed(string[] channels)
//     {
//     }

//     public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
//     {
//     }

//     public void OnUserSubscribed(string channel, string user)
//     {
//     }

//     public void OnUserUnsubscribed(string channel, string user)
//     {
//     }

//     private void AddMessageToChat(string message)
//     {
//         chatText.text += $"{message}\n";
//         Canvas.ForceUpdateCanvases();
//         scrollRect.verticalNormalizedPosition = 0f;
//         Canvas.ForceUpdateCanvases();
//     }
//     public void DebugReturn(DebugLevel level, string message)
//     {
//         // 디버그 메시지를 처리하거나 출력하려면 여기에 코드를 추가하세요.
//         Debug.Log("Photon Chat: " + message);
//     }

// }
