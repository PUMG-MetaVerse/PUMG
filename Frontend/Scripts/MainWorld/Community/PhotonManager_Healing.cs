using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Text;

using VivoxUnity;
public class PhotonManager_Healing : MonoBehaviourPunCallbacks
{
    //버전 입력
    private readonly string version = "1.0f";
    // public VivoxManager vivoxManager = VivoxManager.Instance;
    //사용자 아이디 입력
    private float nextUpdate=0f;
    private string userId = "Mary";
    // private string charNum = "";
    private Transform playerTransform;
    public Button joinVivoxChannelButton;
    public Button leaveVivoxChannelButton;
    //스크립트가 시작되자마자 시작되는 함수
    void Awake() 
    {
        string userId = PlayerPrefs.GetString("PlayerName");

        // charNum = PlayerPrefs.GetString("HealingCharacterNum");
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // PhotonNetwork = charNum;
        // 유저 아이디 할당
        PhotonNetwork.NickName = userId;

        // 마우스 락
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 포톤 서버와 통신 횟수 설정 초당 30회
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        Debug.Log(PhotonNetwork.SendRate);

        // OnJoinedLobby();
        PhotonNetwork.JoinLobby(); // 로비 입장

        // 서버 접속
        // PhotonNetwork.ConnectUsingSettings();
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
        // PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinOrCreateRoom("healing", new RoomOptions { MaxPlayers = 20, IsOpen = true, IsVisible = true }, TypedLobby.Default); // 이 줄을 추가

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
        PhotonNetwork.JoinOrCreateRoom("healing", ro, TypedLobby.Default); // 이 줄을 추가

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

        GameObject playerLocation = PhotonNetwork.Instantiate(PlayerPrefs.GetString("HealingCharacterNum"), points[idx].position, points[idx].rotation, 0);
        // GameObject playerLocation = PhotonNetwork.Instantiate("6", points[idx].position, points[idx].rotation, 0);
        playerTransform = playerLocation.transform;
        // VivoxManager.Instance.Login(PhotonNetwork.NickName); // 캐릭터의 고유한 이름을 사용합니다. 
        // StartCoroutine(SpawnCharacterWhenChannelJoined());
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.NonPositional); // 채널 이름과 채널 유형을 지정합니다.

    }
    private IEnumerator SpawnCharacterWhenChannelJoined()
    {
        // while (!VivoxManager.Instance.vivoxLoginCheck)
        // {
            yield return null;
        // }
        // VivoxManager.Instance.JoinChannel("ChannelName", ChannelType.Positional); // 채널 이름과 채널 유형을 지정합니다.
        // nextUpdate = Time.time;
        

        // Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        // int idx = Random.Range(1, points.Length);
        // PhotonNetwork.Instantiate("Gary_mesh", points[idx].position, points[idx].rotation, 0);
    }
    // private IEnumerator EndJoinChannel()
    // {
    //     // while (!VivoxManager.Instance.channelJoined)
    //     // {
    //         yield return null;
    //     // }

        
    // }
    // void UpdatePosition(Transform listener, Transform speaker)
    // {
    //     // VivoxManager.Instance.vivox.channelSession.Set3DPosition(speaker.position, listener.position, listener.forward, listener.up);
    // }

    // Update is called once per frame
    void Update()
    {
        if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        {
            UpdatePosition(playerTransform, playerTransform);
            nextUpdate+=0.3f;
        }
        // if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
        // {
        //     UpdatePosition(playerTransform, playerTransform);
        //     nextUpdate+=0.3f;
        // }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if((Time.time > nextUpdate) && VivoxManager.Instance.channelJoined)
    //     {
    //         UpdatePosition(playerTransform, playerTransform);
    //         nextUpdate+=0.3f;
    //     }
    // }
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
            VivoxManager.Instance.JoinChannel("Healing", ChannelType.Positional); // 채널 이름과 채널 유형을 지정합니다.
        }
        StartCoroutine("EndJoinChannel");
    }
    public void VivoxChannelLeave()
    {
        VivoxManager.Instance.LeaveChannel();
        VivoxManager.Instance.channelJoined = false;
        StartCoroutine("EndLeftChannel");
    }
}

