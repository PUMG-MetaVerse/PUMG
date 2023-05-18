using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoorInteraction : MonoBehaviour
{
    public Animator doorAnimator;
    public string openTrigger = "IsOpen";
    public string closeTrigger = "IsClose";
    public GameObject Lock;

    public bool isDoorOpen = false;

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
        if (Lock.GetComponent<Lock>().isSolved 
            && ActionController.actionObjectActivated
            && ActionController.hitInfo.transform.name == gameObject.name)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isDoorOpen)
                {
                    doorAnimator.SetTrigger(closeTrigger);

                    if (closeAudio != null)
                    {
                        closeAudio.Play();
                        photonView.RPC("RPC_CloseAudio", RpcTarget.All);
                    }

                    // ��� ������ ����ȭ
                    photonView.RPC("RPC_CloseDoor", RpcTarget.All);
                }
                else
                {
                    doorAnimator.SetTrigger(openTrigger);

                    if (openAudio != null)
                    {
                        openAudio.Play();
                        photonView.RPC("RPC_OpenAudio", RpcTarget.All);
                    }

                    // ��� ������ ����ȭ
                    photonView.RPC("RPC_OpenDoor", RpcTarget.All);
                }

                // ��� ������ ����ȭ
                int viewId = gameObject.GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPC_ChangeDoorInteraction_OpenState", RpcTarget.All, viewId);
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
    void RPC_ChangeDoorInteraction_OpenState(int viewId)
    {
        PhotonView itemPhotonView = PhotonView.Find(viewId);

        if (itemPhotonView != null)
        {
            itemPhotonView.gameObject.GetComponent<DoorInteraction>().isDoorOpen =
                !itemPhotonView.gameObject.GetComponent<DoorInteraction>().isDoorOpen;
        }
    }
}
