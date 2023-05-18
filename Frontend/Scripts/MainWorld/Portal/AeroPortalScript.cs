using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Text;
public class AeroPortalScript : MonoBehaviourPunCallbacks
{
    public GameObject selectModeUIPanel;
    public string targetSceneName; // 다음 씬의 이름을 인스펙터에서 지정할 수 있도록 변경
    private bool hasTriggered = false; // 변수 추가
    PhotonManager photonManager;
    public void ToggleUIPanel()
    {
        // 토글 끔
        if(Cursor.visible){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            hasTriggered = false; 
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        selectModeUIPanel.SetActive(!selectModeUIPanel.activeSelf);
    }
    private void Awake() {
        photonManager = FindObjectOfType<PhotonManager>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"이게 뭐고... : {targetSceneName}");
        if ((other.CompareTag("Player") && !hasTriggered) && other.GetComponent<PhotonView>().IsMine) // "Player" 태그가 있는 오브젝트와 충돌했을 때, 이전에 호출되지 않았을 때
        {
            ToggleUIPanel();
            hasTriggered = true; // 호출되었음을 표시
            // PhotonNetwork.LeaveRoom();
        }
    }
    public void moveScene(string sceneName)
    {
        targetSceneName = sceneName;
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        if (!hasTriggered) // 이미 호출되었다면 반환
            return;

        Debug.Log($"왜 이상한곳으로... : {targetSceneName}");

        Debug.Log($"포톤의 현재 상태 : {PhotonNetwork.NetworkClientState}");
        LoadingSceneController.LoadScene(targetSceneName);

    }
} 