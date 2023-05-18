using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Door_Metal_1F_1_AnimationController : MonoBehaviour
{
    public Animator doorAnimator;
    public string openTrigger = "IsOpen";
    public string closeTrigger = "IsClose";
    public GameObject lockData;

    public bool isDoorOpen = false;

    private PhotonView photonView;

    private AudioSource openCell, closeCell;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        var audios = gameObject.GetComponents<AudioSource>();

        if (audios.Length > 0)
        {
            openCell = audios[0];
            closeCell = audios[1];
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
                if (isDoorOpen)
                {
                    doorAnimator.SetTrigger(closeTrigger);

                    if (closeCell != null)
                    {
                        closeCell.Play();
                        photonView.RPC("RPC_CloseAudio", RpcTarget.All);
                    }

                    // ��� ������ ����ȭ
                    photonView.RPC("RPC_CloseDoor", RpcTarget.All);
                }
                else
                {
                    doorAnimator.SetTrigger(openTrigger);

                    if (openCell != null)
                    {
                        openCell.Play();
                        photonView.RPC("RPC_OpenAudio", RpcTarget.All);
                    }

                    // ��� ������ ����ȭ
                    photonView.RPC("RPC_OpenDoor", RpcTarget.All);
                }

                // ������ ����ϸ� �ڱ� �ڽſ��Ե� �޽����� ���� ������
                // �Ʒ� �ڵ�� ��ü ����
                //isExitDoorOpen = !isExitDoorOpen;

                // ��� ������ ����ȭ
                int viewId = gameObject.GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPC_ChangeMetalDoor_1F_1_OpenState", RpcTarget.All, viewId);
            }
        }
    }

    [PunRPC]
    void RPC_OpenDoor()
    {
        doorAnimator.SetTrigger(openTrigger);
    }

    [PunRPC]
    void RPC_CloseDoor()
    {
        doorAnimator.SetTrigger(closeTrigger);
    }

    [PunRPC]
    void RPC_OpenAudio()
    {
        openCell.Play();
    }

    [PunRPC]
    void RPC_CloseAudio()
    {
        closeCell.Play();
    }

    [PunRPC]
    void RPC_ChangeMetalDoor_1F_1_OpenState(int viewId)
    {
        PhotonView itemPhotonView = PhotonView.Find(viewId);

        if (itemPhotonView != null)
        {
            itemPhotonView.gameObject.GetComponent<Door_Metal_1F_1_AnimationController>().isDoorOpen =
                !itemPhotonView.gameObject.GetComponent<Door_Metal_1F_1_AnimationController>().isDoorOpen;
        }
    }
}

