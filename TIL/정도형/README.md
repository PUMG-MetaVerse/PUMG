# TIL

## 2023-04-13

- Unity 개인 학습
- 오브젝트 생성 및 설정
- 캐릭터 애니메이션 추가 학습
- 맵 에셋 불러오기 학습

## 2023-04-14
- Unity 개인 학습
- 로그인 화면을 만들고, 로그인이 되었을 시 게임 화면으로 이동
- 1인칭 3인칭 스위칭 구현

## 2023-04-24
- 로그인, 회원가입 UI 수정
- API에 맞게 REQUEST, RESPONSE 변경

## 2023-04-25
- UI 버튼 추가
- 친구 창 구현 중

## 2023-04-27

- 포톤 매니저를 활용하여 멀티플레이 구현
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using VivoxUnity;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    //버전 입력
    private readonly string version = "1.0f";
    public VivoxManager vivoxManager = VivoxManager.Instance;
    //사용자 아이디 입력
    private float nextUpdate=0f;
    private string userId = "Mary";
    private int charNum;
    private Transform playerTransform;
    //스크립트가 시작되자마자 시작되는 함수
    void Awake() 
    {
        string userId = PlayerPrefs.GetString("PlayerName");
        Debug.Log($"잘 넘어온 아이디 : {userId}");
        charNum = PlayerPrefs.GetInt("CharacterNum");
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // PhotonNetwork.CharacterNum = characterNum;
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;

        // 포톤 서버와 통신 횟수 설정 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
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
        GameObject playerLocation = PhotonNetwork.Instantiate(PlayerPrefs.GetInt("CharacterNum").ToString(), points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("0", points[idx].position, points[idx].rotation, 0);
        playerTransform = playerLocation.transform;
        VivoxManager.Instance.Login(PhotonNetwork.NickName); // 캐릭터의 고유한 이름을 사용합니다. 
        StartCoroutine(SpawnCharacterWhenChannelJoined());
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.NonPositional); // 채널 이름과 채널 유형을 지정합니다.

    }
    private IEnumerator SpawnCharacterWhenChannelJoined()
    {
        while (!VivoxManager.Instance.vivoxLoginCheck)
        {
            yield return null;
        }
        VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.Positional); // 채널 이름과 채널 유형을 지정합니다.
        nextUpdate = Time.time;
        

        // Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        // int idx = Random.Range(1, points.Length);
        // PhotonNetwork.Instantiate("Gary_mesh", points[idx].position, points[idx].rotation, 0);
    }
    private IEnumerator EndJoinChannel()
    {
        while (!VivoxManager.Instance.channelJoined)
        {
            yield return null;
        }

        
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
        if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        {
            
            UpdatePosition(playerTransform, playerTransform);
            nextUpdate+=0.3f;
        }
    }
}

## 2023-04-28
- 파티 플레이를 위한 포톤 네트워킹 작업 진행 중
- Room에 이미 참여하여 멀티플레이 진행 중이라 Room을 새로 생성할 수 없기 때문에 다른 통신 방법 활용해야 함
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PartyManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private const byte PartyInviteEventCode = 1;
    private const byte PartyJoinEventCode = 2;

    private Dictionary<int, string> partyMembers = new Dictionary<int, string>();
    private Dictionary<string, int> playerNameToActorNumber = new Dictionary<string, int>();


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
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
            Debug.Log($"Received a party invitation from {inviterName} ({inviterId})");

            // Automatically join the party when an invitation is received.
            JoinParty(inviterId, inviterName);
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
            playerNameToActorNumber[player.NickName] = player.ActorNumber;
        }
    }
    public void InvitePlayerToPartyByNickname(string targetPlayerNickname)
    {
        if (playerNameToActorNumber.TryGetValue(targetPlayerNickname, out int targetPlayerId))
        {
            InvitePlayerToParty(targetPlayerId);
        }
        else
        {
            Debug.LogError($"Player with nickname {targetPlayerNickname} not found.");
        }
    }

}
## 2023-05-1
- 파티 관련 기능 추가
- 특정 사용자와 파티를 맺을 시 기존의 파티원 모두에게 새로운 파티원의 정보를 전송 후 UI Update하도록 수정 중
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
public class PhotonManager : MonoBehaviourPunCallbacks,IOnEventCallback
{
    private const byte PartyInviteEventCode = 1;
    private const byte PartyJoinEventCode = 2;
    private const byte PartyMembersListEventCode = 3;
    public GameObject invitationPanel; // 초대 확인 패널
    public TMP_Text invitationMessage; // 초대 메시지 표시용 Text (TMP)
    public TMP_InputField inputField;
    private bool isInParty = false;
    private Dictionary<int, string> partyMembers = new Dictionary<int, string>();
    private Dictionary<string, int> playerNameToActorNumber = new Dictionary<string, int>();
    private List<string> pendingInvitations = new List<string>(); // 초대 메시지 리스트
    public GameObject invitePrefab;
    public Transform panel;
    public RectTransform partyMemberList;
    public Button acceptInviteButtonPrefab;
    public Button declineInviteButtonPrefab;

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

    //스크립트가 시작되자마자 시작되는 함수
    void Awake() 
    {
        string userId = PlayerPrefs.GetString("PlayerName");
        // AuthenticationValues.UserId(userId);
        Debug.Log($"잘 넘어온 아이디 : {userId}");
        charNum = PlayerPrefs.GetInt("CharacterNum");
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // PhotonNetwork.CharacterNum = characterNum;
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;

        // 포톤 서버와 통신 횟수 설정 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("PlayerName");
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
        GameObject playerLocation = PhotonNetwork.Instantiate(PlayerPrefs.GetInt("CharacterNum").ToString(), points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("0", points[idx].position, points[idx].rotation, 0);
        playerTransform = playerLocation.transform;
        VivoxManager.Instance.Login(PhotonNetwork.NickName); // 캐릭터의 고유한 이름을 사용합니다. 
        StartCoroutine(SpawnCharacterWhenChannelJoined());
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.NonPositional); // 채널 이름과 채널 유형을 지정합니다.

    }
    private IEnumerator SpawnCharacterWhenChannelJoined()
    {
        while (!VivoxManager.Instance.vivoxLoginCheck)
        {
            yield return null;
        }
        VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.Positional); // 채널 이름과 채널 유형을 지정합니다.
        nextUpdate = Time.time;
        

        // Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        // int idx = Random.Range(1, points.Length);
        // PhotonNetwork.Instantiate("Gary_mesh", points[idx].position, points[idx].rotation, 0);
    }
    private IEnumerator EndJoinChannel()
    {
        while (!VivoxManager.Instance.channelJoined)
        {
            yield return null;
        }

        
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
        UpdatePlayerList();
        if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        {
            UpdatePosition(playerTransform, playerTransform);
            nextUpdate+=0.3f;
        }
    }
    [PunRPC]
    public void SendFriendRequestByName(string friendNickname)
    {
        // 닉네임을 기반으로 친구 요청 처리 로직
        Debug.Log($"포톤네트워크 플레이어 리스트 : {PhotonNetwork.PlayerList}");
        Debug.Log($"추가한 닉네임 : {friendNickname}");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == friendNickname)
            {
                // 친구 요청 전송 로직
                break;
            }
        }
    }
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
                    SendPartyMembersList(memberId);
                }
            }
            UpdatePartyMemberListUI(); // 파티원 목록 UI를 업데이트합니다.
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
            // Debug.Log($"플레이어 리스트 업데이트 {player.ActorNumber}");
            playerNameToActorNumber[player.NickName] = player.ActorNumber;
        }
        UpdatePartyMemberListUI();
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

        invitationPanel.SetActive(true);
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
            noPartyMessage.SetActive(true);
        }
        else
        {
            noPartyMessage.SetActive(false);
            foreach (KeyValuePair<int, string> member in partyMembers)
            {
                GameObject newMemberUI = Instantiate(partyMemberPrefab, partyMemberListContent);
                newMemberUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{member.Value} ({member.Key})";
            }
        }
    }

    public void ToggleUI()
    {
        partyMembersUI.SetActive(!partyMembersUI.activeSelf);
        invitationsUI.SetActive(!invitationsUI.activeSelf);
    }
    public void SendPartyMembersList(int targetPlayerId)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new[] { targetPlayerId } };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(PartyMembersListEventCode, partyMembers, raiseEventOptions, sendOptions);
    }


}

## 2023-05-03
- 월드 씬에서 방탈출 씬으로 이동 구현 완료
- PhotonManager에서 PhotonManager_RoomEscape 로 이동하여 룸 재접속 및 캐릭터 생성 완료
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
public class PhotonManager_RoomEscape : MonoBehaviourPunCallbacks
{
    private const byte PartyInviteEventCode = 1;
    private const byte PartyJoinEventCode = 2;
    private const byte PartyMembersListEventCode = 3;
    private const byte otherPartyMembersListEventCode = 4;
    private const byte PlayerLeftEventCode = 5;
    public GameObject invitationPanel; // 초대 확인 패널
    public TMP_Text invitationMessage; // 초대 메시지 표시용 Text (TMP)
    public TMP_InputField inputField;
    private bool isInParty = false;
    private Dictionary<int, string> partyMembers = new Dictionary<int, string>();
    private Dictionary<string, int> playerNameToActorNumber = new Dictionary<string, int>();
    private List<string> pendingInvitations = new List<string>(); // 초대 메시지 리스트
    public GameObject invitePrefab;
    public Transform panel;
    public RectTransform partyMemberList;
    public Button acceptInviteButtonPrefab;
    public Button declineInviteButtonPrefab;
    public Button leavePartyBtn;
    //버전 입력
    private readonly string version = "1.0f";
    // public VivoxManager vivoxManager = VivoxManager.Instance; 비복스 살릴 것
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
    //스크립트가 시작되자마자 시작되는 함수
    void Awake() 
    {
        
        partyMembers = PartyMembersManager.Instance.partyMembers;
        // string userId = PlayerPrefs.GetString("PlayerName");
        // AuthenticationValues.UserId(userId);
        // Debug.Log($"잘 넘어온 아이디 : {userId}");
        // charNum = PlayerPrefs.GetInt("CharacterNum");
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        // PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // PhotonNetwork.CharacterNum = characterNum;
        // 유저 아이디 할당
        // PhotonNetwork.NickName = userId;

        // 포톤 서버와 통신 횟수 설정 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        // PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("PlayerName");
        // PhotonNetwork.LocalPlayer.ActorNumber = PlayerPrefs.GetInt("CharacterNum").toString();
        // PhotonNetwork.LocalPlayer.
        PhotonNetwork.JoinLobby(); // 로비 입장
    }

    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        Debug.Log($"온 조인드 로비 포톤 네트워크의 현재 서버 : {PhotonNetwork.NetworkingClient.Server}");
        Debug.Log("룸이스케이프로");
        // PhotonNetwork.JoinRandomRoom();
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // 최대 접속자
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸을 찾을 수 있음
        string uniqueRoomName = GenerateUniqueRoomName();
        Debug.Log($"방 이름 : {uniqueRoomName}");
        PhotonNetwork.JoinOrCreateRoom(uniqueRoomName, ro, TypedLobby.Default);
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
        string uniqueRoomName = GenerateUniqueRoomName();
        Debug.Log($"방 이름 : {uniqueRoomName}");
        PhotonNetwork.JoinOrCreateRoom(uniqueRoomName, ro, TypedLobby.Default); // 이 줄을 추가
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
        GameObject playerLocation = PhotonNetwork.Instantiate("Gary_mesh", points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("0", points[idx].position, points[idx].rotation, 0);
        playerTransform = playerLocation.transform;
        
        //VivoxManager.Instance.Login(PhotonNetwork.NickName); // 캐릭터의 고유한 이름을 사용합니다. 비복스 살릴 것
        StartCoroutine(SpawnCharacterWhenChannelJoined());
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.NonPositional); // 채널 이름과 채널 유형을 지정합니다.

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
    // private IEnumerator EndJoinChannel()
    // {
    //     while (!VivoxManager.Instance.channelJoined)
    //     {
    //         yield return null;
    //     }

        
    // }
    // void UpdatePosition(Transform listener, Transform speaker)
    // {
    //     if (listener == null || speaker == null)
    //     {
    //         return;
    //     }
    //     VivoxManager.Instance.vivox.channelSession.Set3DPosition(speaker.position, listener.position, listener.forward, listener.up);
    // }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //비복스 살릴 것
        // if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        // {
        //     UpdatePosition(playerTransform, playerTransform);
        //     nextUpdate+=0.3f;
        // }
    }
    public string GenerateUniqueRoomName()
    {
        StringBuilder roomNameBuilder = new StringBuilder();
        Debug.Log($"partymembers count : {partyMembers.Count}");
        // 모든 파티원의 닉네임을 가져옵니다.
        if(partyMembers.Count < 1)
        {
            Debug.Log("혼자라서 ? ");
            return PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {

            var partyMemberNicknames = partyMembers.Values.ToList();
            Debug.Log("둘인데도 ?");
            // 닉네임을 정렬하여 동일한 파티 구성에 대해 동일한 룸 이름이 생성되도록 합니다.
            partyMemberNicknames.Sort();

            foreach (string nickname in partyMemberNicknames)
            {
                roomNameBuilder.Append(nickname);
                roomNameBuilder.Append("_");
            }

            // 마지막에 '_' 문자를 제거합니다.
            // roomNameBuilder.Length--;

            return roomNameBuilder.ToString();
        }
    }

}

## 2023-05-04
- NPC Random 이동 테스트 (Nav Mesh 사용)
using UnityEngine;
using UnityEngine.AI;

public class MoveToRandomTarget : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderInterval = 3f;
    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderInterval)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(randomDirection, out navMeshHit, wanderRadius, 1))
            {
                agent.SetDestination(navMeshHit.position);
            }
            timer = 0;
        }
    }
}

## 2023-05-08
- 포톤매니저 완성. 씬 이동 및 돌아왔을 때 로직 완료
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
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // PhotonNetwork.CharacterNum = characterNum;
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;

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
        GameObject playerLocation = PhotonNetwork.Instantiate(PlayerPrefs.GetInt("CharacterNum").ToString(), points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("0", points[idx].position, points[idx].rotation, 0);
        playerTransform = playerLocation.transform;
        
        if(!VivoxManager.Instance.vivoxLoginCheck)
        {
            VivoxManager.Instance.Login(PhotonNetwork.NickName);
        }
         // 캐릭터의 고유한 이름을 사용합니다. 비복스 살릴 것
        StartCoroutine(SpawnCharacterWhenChannelJoined());
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.NonPositional); // 채널 이름과 채널 유형을 지정합니다.

    }
    public override void OnLeftRoom()
    {
        if(checkRoomEscape)
        {
            SceneManager.LoadScene(targetSceneName);
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
    // private IEnumerator EndJoinChannel()
    // {
    //     while (!VivoxManager.Instance.channelJoined)
    //     {
    //         yield return null;
    //     }

        
    // }
    // void UpdatePosition(Transform listener, Transform speaker)
    // {
    //     if (listener == null || speaker == null)
    //     {
    //         return;
    //     }
    //     VivoxManager.Instance.vivox.channelSession.Set3DPosition(speaker.position, listener.position, listener.forward, listener.up);
    // }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //비복스 살릴 것
        UpdatePlayerList();
        // if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        // {
        //     UpdatePosition(playerTransform, playerTransform);
        //     nextUpdate+=0.3f;
        // }
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

## 2023-05-09
- 메인 월드 캐릭터 좌,우클릭 이벤트 추가 중
using UnityEngine;
using Cinemachine;
public class PlayerActions : MonoBehaviour

{
    public ParticleSystem magicParticlePrefab;
    public float magicDuration = 1f; // 추가한 지속 시간 변수
    private Animator animator;
    private Transform playerTransform;
    private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            ShootMagic();
        }
    }

    void ShootMagic()
    {
        animator.SetTrigger("Magic");
        Vector3 particlePosition = playerTransform.position + cinemachineBrain.transform.forward * 0.5f + Vector3.up * 1f;
        Quaternion particleRotation = Quaternion.LookRotation(cinemachineBrain.transform.forward);
        ParticleSystem magicInstance = Instantiate(magicParticlePrefab, particlePosition, particleRotation);

        // 지속 시간이 지난 후 자동으로 제거
        Destroy(magicInstance.gameObject, magicDuration);
    }
    // public GameObject particlePrefab; // 파티클 프리팹
    // private Animator animator;
    // private Transform playerTransform;

    // private void Start()
    // {
    //     animator = GetComponent<Animator>();
    //     playerTransform = GetComponent<Transform>();
    // }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) // 좌클릭을 감지합니다.
    //     {
    //         animator.SetTrigger("Punching");
    //     }
    //     if (Input.GetMouseButtonDown(1)) // 우클릭을 감지합니다.
    //     {
    //         animator.SetTrigger("MagicCasting");

    //         // 파티클 위치 계산
    //         // Vector3 particlePosition = playerTransform.position + playerTransform.forward * 1f;
    //         Vector3 particlePosition = playerTransform.position + playerTransform.forward * 1f + Vector3.up * 1f;

    //         // 파티클 생성
    //         GameObject particleInstance = Instantiate(particlePrefab, particlePosition, Quaternion.identity);

    //         // 파티클 제거 (옵션)
    //         Destroy(particleInstance, 1f); // 파티클의 수명이 2초일 경우 설정
    //     }
    // }
}

## 2023-05-10
- 플레이어 무기 추가 및 스왑 기능 추가
using UnityEngine;
using Cinemachine;
public class PlayerActions : MonoBehaviour

{
    public ParticleSystem magicParticlePrefab;
    public float magicDuration = 1f; // 추가한 지속 시간 변수
    public GameObject[] weapons;

    GameObject nearObject;
    GameObject equipWeapon;

    bool sDown1;
    bool sDown2;
    bool sDown3;


    private Animator animator;
    private Transform playerTransform;
    private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        // if (Input.GetMouseButton(1))
        // {
        //     ShootMagic();
        // }
        GetInput();
        Swap();
        if (Input.GetMouseButtonDown(1))
        {
            ShootMagic();
        }
    }

    void GetInput()
    {
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1);
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2);
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3);    
    }

    void Swap()
    {
        int weaponIndex = -1;
        if(sDown1) weaponIndex = 0;
        if(sDown2) weaponIndex = 1;
        if(sDown3) weaponIndex = 2;
        if((sDown1 || sDown2 || sDown3))
        {
            if(equipWeapon != null)
                equipWeapon.SetActive(false);
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
        }
    }

    void ShootMagic()
    {
        animator.SetTrigger("Magic");
        animator.SetTrigger("IsAttack");

        // 카메라의 방향으로 캐릭터의 위치에서 1만큼 앞에서 파티클이 나가게 합니다.
        Vector3 particlePosition = playerTransform.position + mainCamera.transform.forward * 1f + Vector3.up * 1f;

        Quaternion particleRotation = Quaternion.LookRotation(mainCamera.transform.forward);

        // 45도로 기울이기
        particleRotation *= Quaternion.Euler(0, 0, 0); // x 축을 기준으로 -45도 회전

        ParticleSystem magicInstance = Instantiate(magicParticlePrefab, particlePosition, particleRotation);

        // 지속 시간이 지난 후 자동으로 제거
        Destroy(magicInstance.gameObject, magicDuration);
    }
    // void ShootMagic()
    // {
    //     animator.SetTrigger("Magic");
    //     animator.SetTrigger("IsAttack");
        
    //     // 카메라의 방향으로 캐릭터의 위치에서 1만큼 앞에서 파티클이 나가게 합니다.
    //     Vector3 particlePosition = playerTransform.position + mainCamera.transform.forward * 1f + Vector3.up * 1f;

    //     Quaternion particleRotation = Quaternion.LookRotation(mainCamera.transform.forward);
    //     ParticleSystem magicInstance = Instantiate(magicParticlePrefab, particlePosition, particleRotation);

    //     // 지속 시간이 지난 후 자동으로 제거
    //     Destroy(magicInstance.gameObject, magicDuration);
    // }
    // void ShootMagic()
    // {
    //     animator.SetTrigger("Magic");
    //     Vector3 particlePosition = playerTransform.position + cinemachineBrain.transform.forward * 1f + Vector3.up * 1f;
    //     Quaternion particleRotation = Quaternion.LookRotation(cinemachineBrain.transform.forward);
    //     ParticleSystem magicInstance = Instantiate(magicParticlePrefab, particlePosition, particleRotation);

    //     // 지속 시간이 지난 후 자동으로 제거
    //     Destroy(magicInstance.gameObject, magicDuration);
    // }
    // public GameObject particlePrefab; // 파티클 프리팹
    // private Animator animator;
    // private Transform playerTransform;

    // private void Start()
    // {
    //     animator = GetComponent<Animator>();
    //     playerTransform = GetComponent<Transform>();
    // }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) // 좌클릭을 감지합니다.
    //     {
    //         animator.SetTrigger("Punching");
    //     }
    //     if (Input.GetMouseButtonDown(1)) // 우클릭을 감지합니다.
    //     {
    //         animator.SetTrigger("MagicCasting");

    //         // 파티클 위치 계산
    //         // Vector3 particlePosition = playerTransform.position + playerTransform.forward * 1f;
    //         Vector3 particlePosition = playerTransform.position + playerTransform.forward * 1f + Vector3.up * 1f;

    //         // 파티클 생성
    //         GameObject particleInstance = Instantiate(particlePrefab, particlePosition, Quaternion.identity);

    //         // 파티클 제거 (옵션)
    //         Destroy(particleInstance, 1f); // 파티클의 수명이 2초일 경우 설정
    //     }
    // }
}
