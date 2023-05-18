using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static bool noteActivated = false;
    public string noteName;

    public GameObject noteDetail;
    public GameObject noteBackground;

    // 현재 노트 상세보기 중인 객체의 스크립트를 구별할 수 있도록 함
    public bool isShowing;

    // 방금 꺼진 노트인지 구분할 수 있도록 함
    public bool isQuit;

    void Update()
    {
        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //gameObject.GetComponent<BoxCollider>().enabled = true;
                CloseNoteDetail();
                isQuit = true;
            }
        }
    }

    public void ReadNoteDetail()
    {
        Debug.Log("노트 자세히 보기는 출력 ~~");

        isShowing = true;
        noteActivated = true;

        noteBackground.transform.SetSiblingIndex(0);

        CrossHair.preIsCrossHair = !CrossHair.crossHairActivated;
        CrossHair.crossHairActivated = true;
        noteBackground.SetActive(true);
        noteDetail.SetActive(true);
        CrossHair.ToggleCrossHair();
    }

    public void CloseNoteDetail()
    {
        Debug.Log("노트 자세히 보기 끄기도 출력!!");

        isShowing = false;
        noteActivated = false;

        noteBackground.transform.SetSiblingIndex(0);

        CrossHair.crossHairActivated = CrossHair.preIsCrossHair;
        noteBackground.SetActive(false);
        noteDetail.SetActive(false);
        CrossHair.ToggleCrossHair();
    }
}
