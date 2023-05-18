using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using VivoxUnity;
using UnityEngine.UI;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
public class PhotonManager : MonoBehaviourPunCallbacks,IOnEventCallback
{
    private const byte PartyInviteEventCode = 1;
    private const byte PartyJoinEventCode = 2;
    private const byte PartyMembersListEventCode = 3;
    private const byte otherPartyMembersListEventCode = 4;
    private const byte PlayerLeftEventCode = 5;
    private const byte GotoRoomEscapeEventCode = 6;
    public string targetSceneName;
    public GameObject invitationPanel; // 초대 확인 패널
    public TMP_Text invitationMessage; // 초대 메시지 표시용 Text (TMP)
    public TMP_InputField inputField;
    private bool isInParty = false;
    public Dictionary<int, string> partyMembers = new Dictionary<int, string>();
    private Dictionary<string, int> playerNameToActorNumber = new Dictionary<string, int>();
    private List<string> pendingInvitations = new List<string>(); // 초대 메시지 리스트
    public GameObject invitePrefab;
    public Transform panel;
    public RectTransform partyMemberList;
    public Button acceptInviteButtonPrefab;
    public Button declineInviteButtonPrefab;
    public Button leavePartyBtn;
    //비복스 관련 설정
    public Button joinVivoxChannelButton;
    public Button leaveVivoxChannelButton;
    public bool checkRoomEscape;
    //버전 입력
    private readonly string version = "1.0f";
    public VivoxManager vivoxManager = VivoxManager.Instance;
    //사용자 아이디 입력
    private float nextUpdate=0f;
    private string userId = "Mary";
    private int charNum;
    private Transform playerTransform;


    public GameObject partyMembersUI;
    public GameObject invitationsUI;
    public GameObject noPartyMessage;
    public Transform partyMemberListContent;
    public GameObject partyMemberPrefab;
    public static PhotonManager Instance;
    
    
    //스크립트가 시작되자마자 시작되는 함수
    void Awake() 
    {
        Debug.Log("HealingNum :::::::: " + PlayerPrefs.GetString("HealingCharacterNum"));
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        string userId = PlayerPrefs.GetString("PlayerName");
        // AuthenticationValues.UserId(userId);
        Debug.Log($"잘 넘어온 아이디 : {userId}");
        charNum = PlayerPrefs.GetInt("CharacterNum");
        // 타이틀 설정
        string title = PlayerPrefs.GetString("PlayerTitle");
// 새로운 커스텀 프로퍼티를 만듭니다.
ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
customProperties["Title"] = title;

// 커스텀 프로퍼티를 현재 플레이어에 추가합니다.
PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // PhotonNetwork.CharacterNum = characterNum;
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;
        // 포톤 서버와 통신 횟수 설정 초당 30회
        PhotonNetwork.SendRate = 60; // 초당 데이터 전송 횟수 설정
        PhotonNetwork.SerializationRate = 30;
        // 포톤 서버와 통신 횟수 설정 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        string previousSceneName = PlayerPrefs.GetString("previousSceneName");
        Debug.Log($"이전 씬 : {previousSceneName}");
        
        if(previousSceneName != "AerocraftTargetScene" && previousSceneName != "AerocraftRacingScene" && previousSceneName != "HealingScene"&& previousSceneName != "OfficePack_Example")
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            // previousSceneName = "";
            PhotonNetwork.JoinLobby();
            PlayerPrefs.SetString("previousSceneName", "");
        }
        // 서버 접속
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("PlayerName");
        Debug.Log("PhotonNetwork.LocalPlayer : " + PhotonNetwork.LocalPlayer);
        // PhotonNetwork.LocalPlayer.ActorNumber = PlayerPrefs.GetInt("CharacterNum").toString();
        // PhotonNetwork.LocalPlayer.
        PhotonNetwork.JoinLobby(); // 로비 입장
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        Debug.Log($"온 조인드 로비 포톤 네트워크의 현재 서버 : {PhotonNetwork.NetworkingClient.Server}");
        // PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinOrCreateRoom("world", new RoomOptions { MaxPlayers = 20, IsOpen = true, IsVisible = true }, TypedLobby.Default); // 이 줄을 추가
    }

    //랜덤한 룸 입장이 실패했을 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed{returnCode}:{message}");

        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // 최대 접속자
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸을 찾을 수 있음

        //룸 생성
        // PhotonNetwork.CreateRoom("My Room",ro);
        PhotonNetwork.JoinOrCreateRoom("world", ro, TypedLobby.Default); // 이 줄을 추가
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // 룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"온 조인드 룸 포톤 네트워크의 현재 서버 : {PhotonNetwork.NetworkingClient.Server}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");
        
        
        // 룸에 접속한 사용자 정보 확인
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        // 캐릭터 출현 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        // 캐릭터를 생성
        
        // PhotonNetwork.Instantiate("Gary_mesh", points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("2", points[idx].position, points[idx].rotation, 0);
        GameObject playerLocation = PhotonNetwork.Instantiate(PlayerPrefs.GetInt("CharacterNum").ToString(), points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("0", points[idx].position, points[idx].rotation, 0);
        playerTransform = playerLocation.transform;
        
        if(!VivoxManager.Instance.vivoxLoginCheck)
        {
            // VivoxManager.Instance.Login(PhotonNetwork.NickName);
                StringBuilder sb = new StringBuilder();

            foreach (char c in PhotonNetwork.NickName)
            {
                sb.Append(((int)c).ToString("x4"));
            }

            string hexString = sb.ToString();
            VivoxManager.Instance.Login(hexString);
        }
         // 캐릭터의 고유한 이름을 사용합니다. 비복스 살릴 것
        StartCoroutine(SpawnCharacterWhenChannelJoined());
        

    }
    public override void OnLeftRoom()
    {
        if(checkRoomEscape)
        {
            //SceneManager.LoadScene(targetSceneName);
            EscapeLoadingSceneController.LoadScene(targetSceneName);
            checkRoomEscape = false;
            return;
        }
        // else
        // {
        //      foreach (int memberId in partyMembers.Keys)
        //     {
        //         Debug.Log(memberId);
        //     }

        //     partyMembers.Remove(PhotonNetwork.LocalPlayer.ActorNumber);
        //     foreach (int memberId in partyMembers.Keys)
        //     {
        //         Debug.Log(memberId);
        //         if (memberId != PhotonNetwork.LocalPlayer.ActorNumber)
        //         {
        //             SendPartyMembersList(memberId);
        //         }
        //     }
        //     partyMembers.Clear();
        //     foreach (int memberId in partyMembers.Keys)
        //     {
        //         Debug.Log($"최종적으로 남은 인원 : {memberId}");
        //     }
        // }
        
        UpdatePartyMemberListUI();
        
        
        Debug.Log("Left the party.");
    }
    private IEnumerator SpawnCharacterWhenChannelJoined()
    {
        // while (!VivoxManager.Instance.vivoxLoginCheck)
        // {
        yield return null;
        // }
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.Positional); // 채널 이름과 채널 유형을 지정합니다.
        // nextUpdate = Time.time; 비복스 살릴 것 
        

        // Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        // int idx = Random.Range(1, points.Length);
        // PhotonNetwork.Instantiate("Gary_mesh", points[idx].position, points[idx].rotation, 0);
    }


    // 비복스 살릴 것
    private IEnumerator EndJoinChannel()
    {
        while (!VivoxManager.Instance.channelJoined)
        {
            yield return null;
        }
        joinVivoxChannelButton.gameObject.SetActive(false);
        leaveVivoxChannelButton.gameObject.SetActive(true);
    }
    private IEnumerator EndLeftChannel()
    {
        while (VivoxManager.Instance.channelJoined)
        {
            yield return null;
        }
        joinVivoxChannelButton.gameObject.SetActive(true);
        leaveVivoxChannelButton.gameObject.SetActive(false);
    }
    void UpdatePosition(Transform listener, Transform speaker)
    {
        if (listener == null || speaker == null)
        {
            return;
        }
        VivoxManager.Instance.vivox.channelSession.Set3DPosition(speaker.position, listener.position, listener.forward, listener.up);
    }
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        //비복스 살릴 것
        UpdatePlayerList();
        if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        {
            UpdatePosition(playerTransform, playerTransform);
            nextUpdate+=0.3f;
        }
    }
    // [PunRPC]
    // public void SendFriendRequestByName(string friendNickname)
    // {
    //     // 닉네임을 기반으로 친구 요청 처리 로직
    //     Debug.Log($"포톤네트워크 플레이어 리스트 : {PhotonNetwork.PlayerList}");
    //     Debug.Log($"추가한 닉네임 : {friendNickname}");
    //     foreach (Player player in PhotonNetwork.PlayerList)
    //     {
    //         if (player.NickName == friendNickname)
    //         {
    //             // 친구 요청 전송 로직
    //             break;
    //         }
    //     }
    // }
    // 플레이어를 파티에 초대하는 메서드
    public void InvitePlayerToParty(int targetPlayerId)
    {
        // 현재 Photon 서버에 연결되어 있다면
        if (PhotonNetwork.IsConnected)
        {
            // 이벤트에 전달할 데이터를 설정합니다.
            // content 배열에 로컬 플레이어의 ActorNumber와 NickName을 저장합니다.
            object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName };

            // 이벤트 발생 옵션을 설정합니다.
            // 이 경우 초대 대상인 targetPlayerId에게만 이벤트를 전송하도록 설정합니다.
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };

            // 이벤트 전송 옵션을 설정합니다.
            // 이 경우 이벤트의 신뢰성을 보장하기 위해 Reliability를 true로 설정합니다.
            SendOptions sendOptions = new SendOptions { Reliability = true };

            // 파티 초대 이벤트를 발생시킵니다.
            // PartyInviteEventCode는 이벤트의 고유 코드입니다.
            PhotonNetwork.RaiseEvent(PartyInviteEventCode, content, raiseEventOptions, sendOptions);
        }
    }

    // 파티에 가입하는 메서드
    public void JoinParty(int inviterPlayerId, string inviterPlayerName)
    {
        // 현재 Photon 서버에 연결되어 있다면
        if (PhotonNetwork.IsConnected)
        {
            if (partyMembers.ContainsKey(inviterPlayerId))
            {
                Debug.LogError($"Player {inviterPlayerName} ({inviterPlayerId}) is already in the party.");
                return;
            }

            // 파티 멤버 목록에 초대한 플레이어를 추가합니다.
            partyMembers[inviterPlayerId] = inviterPlayerName;

            // 이벤트에 전달할 데이터를 설정합니다.
            // content 배열에 로컬 플레이어의 ActorNumber와 NickName, 그리고 현재의 파티 멤버 목록을 저장합니다.
            object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName, partyMembers };

            // 이벤트 발생 옵션을 설정합니다.
            // 이 경우 초대한 플레이어인 inviterPlayerId에게만 이벤트를 전송하도록 설정합니다.
        
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { inviterPlayerId } };

            // 이벤트 전송 옵션을 설정합니다.
            // 이 경우 이벤트의 신뢰성을 보장하기 위해 Reliability를 true로 설정합니다.
            SendOptions sendOptions = new SendOptions { Reliability = true };

            // 파티 가입 이벤트를 발생시킵니다.
            // PartyJoinEventCode는 이벤트의 고유 코드입니다.
            PhotonNetwork.RaiseEvent(PartyJoinEventCode, content, raiseEventOptions, sendOptions);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (photonEvent.Code == PartyInviteEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int inviterId = (int)data[0];
            string inviterName = (string)data[1];
            Debug.Log($"Received a party invitation from {inviterName} ({inviterId})");

            // 초대 메시지 리스트에 추가
            if (!pendingInvitations.Contains(inviterName))
            {
                pendingInvitations.Add(inviterName);
            }
            ShowInvitation(inviterName);
        }
        else if (photonEvent.Code == PartyJoinEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int joinerId = (int)data[0];
            string joinerName = (string)data[1];
            Debug.Log($"{joinerName} ({joinerId}) has joined the party");
            
            partyMembers[joinerId] = joinerName;
            UpdatePartyMemberListUI();
            SendPartyMembersList(joinerId);
            // 초대 메시지 리스트에서 제거
            if (pendingInvitations.Contains(joinerName))
            {
                pendingInvitations.Remove(joinerName);
            }
        }
        else if (photonEvent.Code == PartyMembersListEventCode)
        {
            Debug.Log($"포톤이벤트 커스텀 데이터 : {photonEvent.CustomData}");
            Dictionary<int, string> receivedData = (Dictionary<int, string>)photonEvent.CustomData;
            Dictionary<int, string> newPartyMembers = receivedData.ToDictionary(kv => kv.Key, kv => (string)kv.Value);
            // Dictionary<int, object> receivedData = (Dictionary<int, object>)photonEvent.CustomData;
            // Dictionary<int, string> newPartyMembers = receivedData.ToDictionary(kv => kv.Key, kv => kv.Value as string);
            // partyMembers.Clear();
            foreach (var newMember in newPartyMembers)
            {
                // 기존 파티원 목록에 없는 새로운 파티원을 추가합니다.
                if (!partyMembers.ContainsKey(newMember.Key))
                {
                    partyMembers.Add(newMember.Key, newMember.Value);
                }
            }
            foreach (int memberId in partyMembers.Keys)
            {
                if (memberId != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    otherSendPartyMembersList(memberId);
                }
            }
            UpdatePartyMemberListUI(); // 파티원 목록 UI를 업데이트합니다.
        }
        else if (photonEvent.Code == otherPartyMembersListEventCode)
        {
            Debug.Log($"포톤이벤트 커스텀 데이터 : {photonEvent.CustomData}");
            Dictionary<int, string> receivedData = (Dictionary<int, string>)photonEvent.CustomData;
            Dictionary<int, string> newPartyMembers = receivedData.ToDictionary(kv => kv.Key, kv => (string)kv.Value);
            // Dictionary<int, object> receivedData = (Dictionary<int, object>)photonEvent.CustomData;
            // Dictionary<int, string> newPartyMembers = receivedData.ToDictionary(kv => kv.Key, kv => kv.Value as string);
            // partyMembers.Clear();
            foreach (var newMember in newPartyMembers)
            {
                // 기존 파티원 목록에 없는 새로운 파티원을 추가합니다.
                if (!partyMembers.ContainsKey(newMember.Key))
                {
                    partyMembers.Add(newMember.Key, newMember.Value);
                }
            }
            UpdatePartyMemberListUI();
        }
        else if (photonEvent.Code == PlayerLeftEventCode)
        {
            int leftPlayerId = (int)photonEvent.CustomData;
            Debug.Log($"{leftPlayerId} 님이 파티를 나가셨습니다.");
            partyMembers.Remove(leftPlayerId);
            UpdatePartyMemberListUI();
        }
        else if (photonEvent.Code == GotoRoomEscapeEventCode)
        {
            checkRoomEscape = true;
            PartyMembersManager.Instance.SetPartyMembers(partyMembers);
            PhotonNetwork.LeaveRoom();
            // SceneManager.LoadScene(targetSceneName);
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
        
        // foreach (int memberId in partyMembers.Keys)
        // {
            // Debug.Log(memberId);
        // }

        // partyMembers.Remove(PhotonNetwork.LocalPlayer.ActorNumber);
        // Debug.Log(partyMembers.Keys);
        foreach (int memberId in partyMembers.Keys)
        {
            // Debug.Log(memberId);
            if (memberId != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                SendLeftPlayer(memberId);
            }
        }
        partyMembers.Clear();
        UpdatePartyMemberListUI();
        Debug.Log("Left the party.");
    }
    private void UpdatePlayerList()
    {
        playerNameToActorNumber.Clear();
        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            // Debug.Log($"플레이어 리스트 업데이트 {player.ActorNumber}");
            playerNameToActorNumber[player.NickName] = player.ActorNumber;
        }
        // UpdatePartyMemberListUI();
    }
    public void OnInviteButtonClicked()
    {
        string targetPlayerNickname = inputField.text;
        InvitePlayerToPartyByNickname(targetPlayerNickname);
        inputField.text = "";
    }
    public void InvitePlayerToPartyByNickname(string targetPlayerNickname)
    {
        // 파티 멤버인지 확인
        if (partyMembers.ContainsValue(targetPlayerNickname))
        {
            Debug.LogError($"{targetPlayerNickname} is already in the party.");
            return;
        }

        // 이미 초대 메시지를 받은 플레이어인지 확인
        if (pendingInvitations.Contains(targetPlayerNickname))
        {
            Debug.LogError($"Invitation already sent to {targetPlayerNickname}.");
            return;
        }

        // 플레이어 이름을 ActorNumber로 변환하여 초대 메시지를 전송
        if (playerNameToActorNumber.TryGetValue(targetPlayerNickname, out int targetPlayerId))
        {
            InvitePlayerToParty(targetPlayerId);
            pendingInvitations.Add(targetPlayerNickname); // 초대 메시지 리스트에 추가

            // 기존 파티 멤버들에게 새로운 멤버에 대한 정보를 전송합니다.
        }
        else
        {
            Debug.LogError($"Player with nickname {targetPlayerNickname} not found.");
        }
    }
    private void ShowInvitation(string inviterNickname)
    {
        GameObject inviteObj = Instantiate(invitePrefab, panel);
        string message = $"{inviterNickname}님이 당신을 파티에 초대하였습니다.";
        inviteObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;

        // Get the buttons from the instantiated prefab
        Button acceptButton = inviteObj.transform.Find("Button").GetComponent<Button>();
        Button declineButton = inviteObj.transform.Find("Button (1)").GetComponent<Button>();

        // Add event listeners to the buttons
        acceptButton.onClick.AddListener(() => AcceptInvitation(inviterNickname));
        declineButton.onClick.AddListener(() => DeclineInvitation(inviterNickname));

        // invitationPanel.SetActive(true);
    }
    private void CloseInvitationPanel(string inviterNickname)
    {
        foreach (Transform child in panel)
        {
            if (child.GetComponentInChildren<TMPro.TextMeshProUGUI>().text.Contains(inviterNickname))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        if (panel.childCount == 0)
        {
            invitationPanel.SetActive(false);
        }
    }


    public void AcceptPartyInvite(int inviterPlayerId, string inviterPlayerName)
    {
        JoinParty(inviterPlayerId, inviterPlayerName);
        Debug.Log($"Accepted party invitation from {inviterPlayerName} ({inviterPlayerId})");
    }

    public void DeclinePartyInvite(int inviterPlayerId, string inviterPlayerName)
    {
        Debug.Log($"Declined party invitation from {inviterPlayerName} ({inviterPlayerId})");
    }
    public void AcceptInvitation(string inviterNickname)
    {
        if (playerNameToActorNumber.TryGetValue(inviterNickname, out int inviterPlayerId))
        {
            JoinParty(inviterPlayerId, inviterNickname);

            // 초대 메시지 리스트에서 제거
            if (pendingInvitations.Contains(inviterNickname))
            {
                pendingInvitations.Remove(inviterNickname);
            }

            // 초대 패널 닫기 및 초대 메시지 삭제
            CloseInvitationPanel(inviterNickname);

            // 파티 멤버 목록을 전송합니다.
            SendPartyMembersList(inviterPlayerId);
            foreach (int memberId in partyMembers.Keys)
            {
                SendPartyMembersList(memberId);
            }
        }
        else
        {
            Debug.LogError($"Player with nickname {inviterNickname} not found.");
        }
    }

    public void DeclineInvitation(string inviterNickname)
    {
        if (playerNameToActorNumber.TryGetValue(inviterNickname, out int inviterPlayerId))
        {
            DeclinePartyInvite(inviterPlayerId, inviterNickname);

            // 초대 메시지 리스트에서 제거
            if (pendingInvitations.Contains(inviterNickname))
            {
                pendingInvitations.Remove(inviterNickname);
            }

            // 초대 패널 닫기 및 초대 메시지 삭제
            CloseInvitationPanel(inviterNickname);
        }
        else
        {
            Debug.LogError($"Player with nickname {inviterNickname} not found.");
        }
    }
    private void UpdatePartyMemberListUI()
    {
        // Remove all current party member UI elements
        foreach (Transform child in partyMemberListContent)
        {
            Destroy(child.gameObject);
        }

        if (partyMembers.Count == 0)
        {
            noPartyMessage.gameObject.SetActive(true);
            leavePartyBtn.gameObject.SetActive(false);
        }
        else
        {
            noPartyMessage.gameObject.SetActive(false);
            leavePartyBtn.gameObject.SetActive(true);
            foreach (KeyValuePair<int, string> member in partyMembers)
            {
                GameObject newMemberUI = Instantiate(partyMemberPrefab, partyMemberListContent);
                newMemberUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{member.Value} ({member.Key})";
            }
        }
    }

    public void PartyMemberListView()
    {
        partyMembersUI.SetActive(true);
        invitationsUI.SetActive(false);
    }
    public void InvitationsView()
    {
        partyMembersUI.SetActive(false);
        invitationsUI.SetActive(true);        
    }

    public void VivoxChannelJoin()
    {
        if(!VivoxManager.Instance.vivoxLoginCheck)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in PhotonNetwork.NickName)
            {
                sb.Append(((int)c).ToString("x4"));
            }

            string hexString = sb.ToString();
            VivoxManager.Instance.Login(hexString);
        }
        else
        {
            VivoxManager.Instance.JoinChannel("MainWorld", ChannelType.Positional); // 채널 이름과 채널 유형을 지정합니다.
        }
        StartCoroutine("EndJoinChannel");
    }
    public void VivoxChannelLeave()
    {
        VivoxManager.Instance.LeaveChannel();
        VivoxManager.Instance.channelJoined = false;
        StartCoroutine("EndLeftChannel");
    }
    public void SendPartyMembersList(int targetPlayerId)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(PartyMembersListEventCode, partyMembers, raiseEventOptions, sendOptions);
    }
    public void otherSendPartyMembersList(int targetPlayerId)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(otherPartyMembersListEventCode, partyMembers, raiseEventOptions, sendOptions);
    }
    public void SendLeftPlayer(int targetPlayerId)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(PlayerLeftEventCode, PhotonNetwork.LocalPlayer.ActorNumber, raiseEventOptions, sendOptions);
    }
    public void SendGotoRoomEscape(int targetPlayerId)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(GotoRoomEscapeEventCode, PhotonNetwork.LocalPlayer.ActorNumber, raiseEventOptions, sendOptions);
    }
}
