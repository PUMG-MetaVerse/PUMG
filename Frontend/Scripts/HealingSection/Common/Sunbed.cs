using System;
using Photon.Pun;
using UnityEngine;

public class Sunbed : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private bool isOccupied = false;

    [SerializeField]
    private int currentPlayerViewID = 0; // 현재 소유자의 PhotonView ID를 저장하는 변수

    public bool IsOccupied
    {
        get { return isOccupied; }
    }

    public int GetCurrentPlayerViewID
    {
        get { return currentPlayerViewID; }
    }


    private void Update()
    {
        // 현재 소유자의 PhotonView ID가 존재하는지 확인합니다.
        if (currentPlayerViewID != 0 && PhotonNetwork.GetPhotonView(currentPlayerViewID) == null)
        {
            // 소유자를 찾을 수 없으므로 Occupy()를 실행하고 currentOwnerViewID를 업데이트합니다.
            photonView.RPC("Vacate", RpcTarget.All);
            currentPlayerViewID = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isOccupied);
            stream.SendNext(currentPlayerViewID);
        }
        else
        {
            isOccupied = (bool)stream.ReceiveNext();
            currentPlayerViewID = (int)stream.ReceiveNext();
        }
    }
    
    [PunRPC]
    public void Occupy(int playerId)
    {
        isOccupied = true;
        currentPlayerViewID = playerId;
    }

    [PunRPC]
    public void Vacate()
    {
        isOccupied = false;
        currentPlayerViewID = 0;
    }

    public static implicit operator GameObject(Sunbed v)
    {
        throw new NotImplementedException();
    }
}