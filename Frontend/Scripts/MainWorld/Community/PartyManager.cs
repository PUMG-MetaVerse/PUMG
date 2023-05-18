using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using TMPro;
public class PartyManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte PartyInviteEventCode = 1;
    private const byte PartyJoinEventCode = 2;
    public GameObject invitationPanel; // 초대 확인 패널
    public TMP_Text invitationMessage; // 초대 메시지 표시용 Text (TMP)
    public TMP_InputField inputField;
    private Dictionary<int, string> partyMembers = new Dictionary<int, string>();
    private Dictionary<string, int> playerNameToActorNumber = new Dictionary<string, int>();


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start() {
        Debug.Log("플레이어 목록:");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"닉네임: {player.NickName}, ActorNumber: {player.ActorNumber}");
        }
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            UpdatePlayerList();
        }
    }

    public override void OnConnectedToMaster()
    {
        // Debug.Log("Connected to Master Server.");
        // PhotonNetwork.LocalPlayer.NickName = "Player_" + Random.Range(1000, 9999);
    }

    public void InvitePlayerToParty(int targetPlayerId)
    {
        if (PhotonNetwork.IsConnected)
        {
            object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PartyInviteEventCode, content, raiseEventOptions, sendOptions);
        }
    }

    public void JoinParty(int inviterPlayerId, string inviterPlayerName)
    {
        if (PhotonNetwork.IsConnected)
        {
            object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { inviterPlayerId } };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PartyJoinEventCode, content, raiseEventOptions, sendOptions);

            partyMembers[inviterPlayerId] = inviterPlayerName;
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == PartyInviteEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int inviterId = (int)data[0];
            string inviterName = (string)data[1];
            // Debug.Log($"Received a party invitation from {inviterName} ({inviterId})");
            ShowInvitation(inviterName);
            // Automatically join the party when an invitation is received.
            // JoinParty(inviterId, inviterName);
        }
        else if (photonEvent.Code == PartyJoinEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int joinerId = (int)data[0];
            string joinerName = (string)data[1];
            Debug.Log($"{joinerName} ({joinerId}) has joined the party");

            partyMembers[joinerId] = joinerName;
        }
    }

    public void PrintPartyMembers()
    {
        Debug.Log("Current Party Members:");
        foreach (KeyValuePair<int, string> member in partyMembers)
        {
            Debug.Log($"{member.Value} ({member.Key})");
        }
    }

    public void LeaveParty()
    {
        partyMembers.Clear();
        Debug.Log("Left the party.");
    }
    private void UpdatePlayerList()
    {
        playerNameToActorNumber.Clear();
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            Debug.Log($"플레이어 리스트 업데이트 {player.ActorNumber}");
            playerNameToActorNumber[player.NickName] = player.ActorNumber;
        }
    }
    public void InvitePlayerToPartyByNickname(string targetPlayerNickname)
    {
        Debug.Log($"타겟플레이어네임 : {targetPlayerNickname}");
        // Debug.Log($"플레이어 리스트 : {playerNameToActorNumber.get}");
        
        foreach(KeyValuePair<string,int> players in playerNameToActorNumber)
        {
            Debug.Log($"플레이어 : {players.Key}, {players.Value}");
        }
        if (playerNameToActorNumber.TryGetValue(targetPlayerNickname, out int targetPlayerId))
        {
            InvitePlayerToParty(targetPlayerId);
        }
        else
        {
            Debug.LogError($"Player with nickname {targetPlayerNickname} not found.");
        }
    }
    private void ShowInvitation(string inviterNickname)
    {
        invitationMessage.text = $"{inviterNickname}님이 당신을 파티에 초대하였습니다.";
        invitationPanel.SetActive(true);
    }
    public void OnInviteButtonClicked()
    {
        string targetPlayerNickname = inputField.text;
        InvitePlayerToPartyByNickname(targetPlayerNickname);
    }
}
