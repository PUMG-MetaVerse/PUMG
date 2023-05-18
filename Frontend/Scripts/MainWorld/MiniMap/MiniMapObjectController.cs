using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMapObjectController : MonoBehaviour
{
    public GameObject minimapIcon;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // IsThisMyCharacter()는 현재 오브젝트가 "내" 캐릭터인지 확인하는 메서드입니다.
        // 이를 적절히 구현해야 합니다.
        if (IsThisMyCharacter())
        {
            // 이 오브젝트의 미니맵 아이콘을 활성화합니다.
            this.minimapIcon.SetActive(true);

            // 미니맵 아이콘 이동 로직
            // ...
        }
        else
        {
            // 이 오브젝트는 "내" 캐릭터가 아니므로 미니맵 아이콘을 비활성화합니다.
            this.minimapIcon.SetActive(false);
        }
    }

    bool IsThisMyCharacter()
    {
        PhotonView photonView = this.GetComponentInParent<PhotonView>();

        // photonView가 null이 아니고, 이 photonView가 "내" 것인지 확인합니다.
        // Debug.Log(photonView.IsMine);
        return photonView != null && photonView.IsMine;
    }
}
