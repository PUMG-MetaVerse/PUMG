using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PortalScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public string targetSceneName; // 다음 씬의 이름을 인스펙터에서 지정할 수 있도록 변경

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // "Player" 태그가 있는 오브젝트와 충돌했을 때
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        // 현재 룸을 나간 후 호출되는 콜백 함수
        SceneManager.LoadScene(targetSceneName); // 인스펙터에서 지정한 씬 로드
        // VivoxManager.Instance.LeaveChannel();
        // VivoxManager.Instance.channelJoined = false;
    }
}