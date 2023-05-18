using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ExitDoor_1F_1_AnimationController : MonoBehaviour
{
    public Animator exitDoorAnimator;
    public string openTrigger = "IsOpen";
    public string closeTrigger = "IsClose";
    public GameObject lockData;

    public bool isExitDoorOpen = false;

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
                if (isExitDoorOpen)
                {
                    exitDoorAnimator.SetTrigger(closeTrigger);

                    if (closeCell != null)
                    {
                        closeCell.Play();
                        photonView.RPC("RPC_CloseAudio", RpcTarget.All);
                    }

                    // ��� ������ ����ȭ
                    photonView.RPC("RPC_CloseExitDoor", RpcTarget.All);
                }
                else
                {
                    exitDoorAnimator.SetTrigger(openTrigger);

                    if (openCell != null)
                    {
                        openCell.Play();
                        photonView.RPC("RPC_OpenAudio", RpcTarget.All);
                    }

                    // ��� ������ ����ȭ
                    photonView.RPC("RPC_OpenExitDoor", RpcTarget.All);
                }

                // ������ ����ϸ� �ڱ� �ڽſ��Ե� �޽����� ���� ������
                // �Ʒ� �ڵ�� ��ü ����
                //isExitDoorOpen = !isExitDoorOpen;

                // ��� ������ ����ȭ
                int viewId = gameObject.GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPC_ChangeExitDoor_1F_1_OpenState", RpcTarget.All, viewId);
            }
        }
    }

    [PunRPC]
    void RPC_OpenExitDoor()
    {
        exitDoorAnimator.SetTrigger(openTrigger);
    }

    [PunRPC]
    void RPC_CloseExitDoor()
    {
        exitDoorAnimator.SetTrigger(closeTrigger);
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
    void RPC_ChangeExitDoor_1F_1_OpenState(int viewId)
    {
        PhotonView itemPhotonView = PhotonView.Find(viewId);

        if (itemPhotonView != null)
        {
            itemPhotonView.gameObject.GetComponent<ExitDoor_1F_1_AnimationController>().isExitDoorOpen =
                !itemPhotonView.gameObject.GetComponent<ExitDoor_1F_1_AnimationController>().isExitDoorOpen;
        }
    }
}

