using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Text;
public class HealingPortalScript : MonoBehaviourPunCallbacks
{
    public string targetSceneName; // 다음 씬의 이름을 인스펙터에서 지정할 수 있도록 변경
    private bool hasTriggered = false; // 변수 추가
    PhotonManager photonManager;
    private void Awake() {
        photonManager = FindObjectOfType<PhotonManager>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"이게 뭐고... : {targetSceneName}");
        if ((other.CompareTag("Player") && !hasTriggered) && other.GetComponent<PhotonView>().IsMine) // "Player" 태그가 있는 오브젝트와 충돌했을 때, 이전에 호출되지 않았을 때
        {
            // foreach (int memberId in photonManager.partyMembers.Keys)
            // {
            //     if (memberId != PhotonNetwork.LocalPlayer.ActorNumber)
            //     {
            //         photonManager.SendGotoRoomEscape(memberId);
            //     }       
            // }
            // Debug.Log("HealingCharacter: " + userInfo.healingCharacter);
            hasTriggered = true; // 호출되었음을 표시
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        if (!hasTriggered) // 이미 호출되었다면 반환
            return;
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; // 최대 접속자
        ro.IsOpen = true; // 룸의 오픈 여부
        ro.IsVisible = true; // 로비에서 룸을 찾을 수 있음
        Debug.Log($"왜 이상한곳으로... : {targetSceneName}");
        // photonManager.checkRoomEscape = true;
        // if(photonManager.partyMembers.Count < 2)
        // {
        //     PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.LocalPlayer.NickName, ro, TypedLobby.Default); // 이 줄을 추가
        //     SceneManager.LoadScene(targetSceneName);
        // }
        // else
        // {
        //     StringBuilder roomNameBuilder = new StringBuilder();
        //     var partyMemberNicknames = photonManager.partyMembers.Values.ToList();
        //     partyMemberNicknames.Sort();
        //     foreach(string nickname in partyMemberNicknames)
        //     {
        //         roomNameBuilder.Append(nickname);
        //         roomNameBuilder.Append("_");
        //     }
        //     roomNameBuilder.Length--;
        //     // PhotonNetwork.JoinOrCreateRoom(roomNameBuilder.ToString(), ro, TypedLobby.Default); // 이 줄을 추가
        //     SceneManager.LoadScene(targetSceneName);
        // }
        // PartyMembersManager.Instance.SetPartyMembers(photonManager.partyMembers);
        Debug.Log($"포톤의 현재 상태 : {PhotonNetwork.NetworkClientState}");
        Debug.Log("캐릭터 넘버 상태" + PlayerPrefs.GetString("HealingCharacterNum")+"2222s");
        Debug.Log("캐릭터 넘버 상태" + PlayerPrefs.GetString("HealingCharacterNum") == "");
        Debug.Log("캐릭터 넘버 상태 NULL !!!!!" + PlayerPrefs.GetString("") == null);
        if(PlayerPrefs.GetString("HealingCharacterNum") == "") 
        {
            LoadingSceneController.LoadScene("SelectChar_Healing");
        } 
        else
        {
            LoadingSceneController.LoadScene(targetSceneName);
        } 

        // SceneManager.LoadScene(targetSceneName); // 인스펙터에서 지정한 씬 로드
        // return;
        // VivoxManager.Instance.LeaveChannel();
        // VivoxManager.Instance.channelJoined = false;
    }
}