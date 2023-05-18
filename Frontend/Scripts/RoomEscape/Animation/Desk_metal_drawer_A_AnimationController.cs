using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Desk_metal_drawer_A_AnimationController : MonoBehaviour
{
    public Animator deskDrawerAnimator;
    public string openTrigger = "IsOpen";
    public string closeTrigger = "IsClose";
    public GameObject lockData;

    public bool isDeskDrawerOpen = false;

    private PhotonView photonView;

    private AudioSource openDrawer, closeDrawer;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        var audios = gameObject.GetComponents<AudioSource>();
        
        if (audios.Length > 0)
        {
            closeDrawer = audios[0];
            openDrawer = audios[1];
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
                if (isDeskDrawerOpen)
                {
                    deskDrawerAnimator.SetTrigger(closeTrigger);

                    if (closeDrawer != null)
                    {
                        closeDrawer.Play();
                        photonView.RPC("RPC_CloseAudio", RpcTarget.All);
                    }

                    // 모든 유저와 동기화
                    photonView.RPC("RPC_CloseDresser", RpcTarget.All);
                }
                else
                {
                    deskDrawerAnimator.SetTrigger(openTrigger);

                    if (openDrawer != null)
                    {
                        openDrawer.Play();
                        photonView.RPC("RPC_OpenAudio", RpcTarget.All);
                    }

                    // 모든 유저와 동기화
                    photonView.RPC("RPC_OpenDresser", RpcTarget.All);
                }

                // 모든 유저와 동기화
                int viewId = gameObject.GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPC_DeskDrawer_1F_1_OpenState", RpcTarget.All, viewId);
            }
        }
    }

    [PunRPC]
    void RPC_OpenAudio()
    {
        openDrawer.Play();
    }

    [PunRPC]
    void RPC_CloseAudio()
    {
        closeDrawer.Play();
    }

    [PunRPC]
    void RPC_OpenDresser()
    {
        deskDrawerAnimator.SetTrigger(openTrigger);
    }

    [PunRPC]
    void RPC_CloseDresser()
    {
        deskDrawerAnimator.SetTrigger(closeTrigger);
    }

    [PunRPC]

    void RPC_DeskDrawer_1F_1_OpenState(int viewId)
    {
        PhotonView itemPhotonView = PhotonView.Find(viewId);

        if (itemPhotonView != null)
        {
            itemPhotonView.gameObject.GetComponent<Desk_metal_drawer_A_AnimationController>().isDeskDrawerOpen =
                !itemPhotonView.gameObject.GetComponent<Desk_metal_drawer_A_AnimationController>().isDeskDrawerOpen;
        }
    }
}

