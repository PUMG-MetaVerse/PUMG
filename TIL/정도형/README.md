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

