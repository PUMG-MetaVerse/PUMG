using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CabinetDoor_1F_1_AnimationController : MonoBehaviour
{
    public Animator cabinetDoorAnimator;
    public string openTrigger = "IsOpen";
    public string closeTrigger = "IsClose";
    public GameObject lockData;

    public bool isCabinetDoorOpen = false;

    private PhotonView photonView;

    private AudioSource openAudio, closeAudio;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        var audios = gameObject.GetComponents<AudioSource>();
        if (audios.Length > 0)
        {
            closeAudio = audios[0];
            openAudio = audios[1];
        }
    }

    void Update()
    {
        if (lockData.GetComponent<Lock>().isSolved 
            && ActionController.actionObjectActivated
            && ActionController.hitInfo.transform.name == gameObject.name)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("push F");

                if (isCabinetDoorOpen)
                {
                    Debug.Log("Cabinet close");
                    cabinetDoorAnimator.SetTrigger(closeTrigger);

                    if (closeAudio != null)
                    {
                        closeAudio.Play();
                        photonView.RPC("RPC_CloseAudio", RpcTarget.All);
                    }

                    // 모든 유저와 동기화
                    photonView.RPC("RPC_CloseCabinetDoor", RpcTarget.All);
                }
                else
                {
                    Debug.Log("Cabinet open");
                    cabinetDoorAnimator.SetTrigger(openTrigger);

                    if (openAudio != null)
                    {
                        openAudio.Play();
                        photonView.RPC("RPC_OpenAudio", RpcTarget.All);
                    }

                    // 모든 유저와 동기화
                    photonView.RPC("RPC_OpenCabinetDoor", RpcTarget.All);
                }

                // 포톤을 사용하면 자기 자신에게도 메시지가 오기 때문에
                // 아래 코드로 대체 가능
                //isExitDoorOpen = !isExitDoorOpen;

                // 모든 유저와 동기화
                int viewId = gameObject.GetComponent<PhotonView>().ViewID;
                Debug.Log("viewId : " + viewId);
                photonView.RPC("RPC_ChangeCabinetDoor_1F_1_OpenState", RpcTarget.All, viewId);
            }
        }
    }

    [PunRPC]
    void RPC_OpenAudio()
    {
        openAudio.Play();
    }

    [PunRPC]
    void RPC_CloseAudio()
    {
        closeAudio.Play();
    }

    [PunRPC]
    void RPC_OpenCabinetDoor()
    {
        cabinetDoorAnimator.SetTrigger(openTrigger);
    }

    [PunRPC]
    void RPC_CloseCabinetDoor()
    {
        cabinetDoorAnimator.SetTrigger(closeTrigger);
    }

    [PunRPC]
    void RPC_ChangeCabinetDoor_1F_1_OpenState(int viewId)
    {
        PhotonView itemPhotonView = PhotonView.Find(viewId);

        if (itemPhotonView != null)
        {
            itemPhotonView.gameObject.GetComponent<CabinetDoor_1F_1_AnimationController>().isCabinetDoorOpen =
                !itemPhotonView.gameObject.GetComponent<CabinetDoor_1F_1_AnimationController>().isCabinetDoorOpen;
        }
    }
}

