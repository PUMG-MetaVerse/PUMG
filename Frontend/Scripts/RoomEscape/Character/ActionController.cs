using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;

// 플레이어와 물체의 상호작용과 관련된 코드
public class ActionController : MonoBehaviour
{
    public static bool itemPickupActivated;                 // 아이템을 습득 할 수 있는 상태인가?
    public static bool readObjectDetailActivated;           // 오브젝트의 상세 정보를 읽을 수 있는 상태인가?
    public static bool readLockDetailActivated;             // 자물쇠 상세 보기를 할 수 있는 상태인가?
    public static bool readNoteDetailActivated;             // 메모 상세 보기를 할 수 있는 상태인가?
    public static bool actionObjectActivated;               // 물체를 열고 닫는 것을 할 수 있는 상태인가?
    public static bool isFlashOn;                           // 손전등이 켜져있는 상태인가?
    public static bool isSecondFloorClear;                  // 2층을 탈출했는가?
    public static bool isFirstFloorClear;                   // 1층을 탈출했는가?

    public float range;                                     // 습득 가능한 최대 거리
    public static RaycastHit hitInfo;                       // 충돌체 정보 저장
    public static RaycastHit preHitInfo;                    // 이전 충돌체 정보 저장
    public LayerMask layerMask;                             // 특정 레이어에서만 아이템을 인식하도록 레이어 마스크를 설정

    public Text actionText;                                 // 상호작용 활성화 메시지
    public Text notificationText;                           // 알림 메시지
    public GameObject notificationBackground;               // 알림 메시지 배경
    public GameObject lockInfoMiniDesc;                     // 자물쇠 단축키 미니 설명

    public Inventory theInventory;                          // 인벤토리

    public Text objectDetailText;                           // 오브젝트 상세정보 한 줄씩
    public string[] objectDetailTextList;                   // 오브젝트 상세정보 배열
    public int currentObjectDetailTextIdx;

    public Text escapeMessage; 
    public bool isIKeyEnabled;

    public static Camera currentCamera;
    public static Camera targetCamera;

    public bool isOpen;

    private PhotonView photonView;

    void Start()
    {
        currentCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        ShootLayCast();
        SetKeyEvent();
    }

    private void SetKeyEvent()
    {
        // 조준선 토글
        if (Input.GetKeyDown(KeyCode.Z)
            && !Inventory.inventoryActivated
            && !InteractionObject.objectDetailTextActivated
            && !Lock.lockSolvingActivated
            && !KeyInfo.keyInfoActivated
            )
        {
            CrossHair.ToggleCrossHair();
        }

        // 아이템 획득
        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemPickUp();
        }

        // 오브젝트 상호작용 & 자물쇠 상세보기
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithObject();
            ZoomLock();
            ReadNote();
        }

        // 손전등 on, off
        if (Input.GetKeyDown(KeyCode.C)
            && !Inventory.inventoryActivated
            && !InteractionObject.objectDetailTextActivated
            && !Lock.lockSolvingActivated
            && !KeyInfo.keyInfoActivated
            )
        {
            if (!HasFlashlightInInventory())
            {
                StartCoroutine(ShowNotificationText(1.0f, "불빛을 비추려면 손전등이 필요합니다."));
                return;
            }

            if (!isFlashOn)
            {
                GetComponent<Light>().enabled = true;
                isFlashOn = true;
            }
            else
            {
                GetComponent<Light>().enabled = false;
                isFlashOn = false;
            }
        }
    }

    // 상호작용 할 수 있는 오브젝트를 끊임없이 탐색하는 함수
    // 키보드, 마우스를 이리저리 움직일 때 계속 레이캐스트를 쏘며 상호작용 할 수 있는 오브젝트가 있는 지 확인한다.
    private void ShootLayCast()
    {
        // 이전의 액티브 메시지, 하이라이트 제거
        if (preHitInfo.transform != null && preHitInfo.transform.tag is "Item" or "Interaction" or "Lock" or "Note" or "ActionObject")
        {
            ClearActiveText(preHitInfo.transform.tag);
            ClearObjectHighlight(preHitInfo.transform);
        }

        // 오브젝트에 대한 상세 설명이 나오는 동안은 레이캐스트 비활성화
        // 인벤토리가 열려있는 동안은 레이캐스트 비활성화
        if (Physics.Raycast(
            transform.position, transform.TransformDirection(Vector3.forward),
            out hitInfo, range, (1 << 20) | (1 << 21) | (1 << 22) | (1 << 23) | (1 << 24) | (1 << 0))
            && !InteractionObject.objectDetailTextActivated
            && !Inventory.inventoryActivated
            && !KeyInfo.keyInfoActivated
        )
        {
            if (hitInfo.transform.gameObject.layer == 0)
            {
                return;
            }

            ShowActiveText(hitInfo.transform.tag);
            ShowObjectHighlight(hitInfo.transform, 0);
        }

        preHitInfo = hitInfo;
    }

    private void InteractWithObject()
    {
        if (InteractionObject.objectDetailTextActivated)
        {
            // 상호작용 오브젝트의 메시지들 출력이 끝나면
            if (currentObjectDetailTextIdx == objectDetailTextList.Length)
            {
                InteractionObject.objectDetailTextActivated = !InteractionObject.objectDetailTextActivated;
                objectDetailText.gameObject.SetActive(false);
                currentObjectDetailTextIdx = 0;

                // 크로스헤어 활성화
                ShowCrossHair();
            }
            else
            {
                objectDetailText.text = objectDetailTextList[currentObjectDetailTextIdx++];
            }
        }

        if (readObjectDetailActivated && hitInfo.transform != null)
        {
            if (hitInfo.transform.name != "Door_Final")
            {
                InteractionObject.objectDetailTextActivated = !InteractionObject.objectDetailTextActivated;
            }

            // 크로스헤어 비활성화
            ClearCrossHair();

            // 건물 최종 탈출 
            if (hitInfo.transform.name == "Door_Final")
            {
                Debug.Log(PlayerPrefs.GetInt("Idx") + "번 플레이어 건물 최종 탈출!!! 메인 월드로 이동!!!");

                // 최종 탈출 성공 화면 띄우기
                escapeMessage.gameObject.SetActive(true);
                escapeMessage.color = new Color(escapeMessage.color.r, escapeMessage.color.g, escapeMessage.color.b, 1f);
                escapeMessage.text = "최종 탈출 성공";

                ClearActiveText("Interaction");

                StartCoroutine(FadeOutAfterDelay(1f));

                return;
            }

            // 오브젝트 상세정보 띄우기
            objectDetailTextList = hitInfo.transform.GetComponent<InteractionObject>().messages;
            currentObjectDetailTextIdx = 0;
            objectDetailText.text = objectDetailTextList[currentObjectDetailTextIdx++];
            objectDetailText.gameObject.SetActive(true);
        }
    }

    IEnumerator FadeOutAfterDelay(float delay)
    {
        InteractionObject.objectDetailTextActivated = !InteractionObject.objectDetailTextActivated;

        yield return new WaitForSeconds(delay);

        float fadeTime = 1f;
        float startAlpha = escapeMessage.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(escapeMessage.color.r, escapeMessage.color.g, escapeMessage.color.b, Mathf.Lerp(startAlpha, 0, t));
            escapeMessage.color = newColor;
            yield return null;
        }

        escapeMessage.color = new Color(escapeMessage.color.r, escapeMessage.color.g, escapeMessage.color.b, 0);

        // 칭호 API 쏘기
        StartCoroutine(PostRequest());

        // 메인 월드로 이동
        StartCoroutine(ReturnToMainMenuCoroutine());

        escapeMessage.gameObject.SetActive(true);
    }

    IEnumerator ReturnToMainMenuCoroutine()
    {
        Slot.currentDropAngle = 0f;
        Slot.dropRadius = 0.45f;
        Slot.dropAngleStep = 65f;

        Note.noteActivated = false;
        Lock.lockSolvingActivated = false;
        Inventory.inventoryActivated = false;
        //InteractionObject.objectDetailTextActivated = false;
        HoverSlotController.isMouseDown = false;
        HoverSlotController.isMouseOver = false;
        KeyInfo.keyInfoActivated = false;
        Menu.isActiveMenu = false;
        CrossHair.crossHairActivated = true;
        CrossHair.preIsCrossHair = false;

        itemPickupActivated = false;
        readObjectDetailActivated = false;
        readLockDetailActivated = false;
        readNoteDetailActivated = false;
        actionObjectActivated = false;
        isFlashOn = false;
        isSecondFloorClear = false;
        isFirstFloorClear = false;

        hitInfo = new RaycastHit();
        preHitInfo = new RaycastHit();
        currentCamera = null;
        targetCamera = null;

        // Photon 네트워크에서 연결 해제
        PhotonNetwork.Disconnect();

        // 연결 해제가 완료될 때까지 대기
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        // Main Scene으로 이동
        SceneManager.LoadScene("04 - City");
        VivoxManager.Instance.LeaveChannel();
        VivoxManager.Instance.channelJoined = false;
        // 씬 전환 완료를 기다립니다.
        yield return null;

        // Photon 네트워크에 다시 연결
        PhotonNetwork.ConnectUsingSettings();
    }

    private void ReadNote()
    {
        if (readNoteDetailActivated && hitInfo.transform != null)
        {
            if (!hitInfo.transform.GetComponent<Note>().isQuit)
            {
                hitInfo.transform.GetComponent<Note>().ReadNoteDetail();
                //hitInfo.transform.GetComponent<BoxCollider>().enabled = false;
                ClearActiveText("Note");
            }
            hitInfo.transform.GetComponent<Note>().isQuit = false;
        }
    }

    private void ZoomLock()
    {
        if (readLockDetailActivated)
        {
            string lockType = LayerMask.LayerToName(hitInfo.transform.gameObject.layer);
            string lockObjectName = hitInfo.transform.gameObject.name;

            // 다른 사람이 해당 자물쇠를 풀고 있는 경우
            if (hitInfo.transform.GetComponent<Lock>().isOtherSolving)
            {
                StartCoroutine(ShowNotificationText(1.0f, "다른 플레이어가 해당 자물쇠를 풀고 있습니다."));
                return;
            }

            if (lockType.Equals("OldLock"))
            {
                if (!HasOldLockKeyInInventory(lockObjectName))
                {
                    StartCoroutine(ShowNotificationText(1.0f, "자물쇠를 열기 위해서는 열쇠가 필요합니다."));
                    return;
                }
                GameObject.Find(lockObjectName).transform.Find("base").transform.Find("Key").gameObject.SetActive(true);
                GameObject.Find(lockObjectName).GetComponent<OldlockController>().OnUnLock();
            }
            else if (lockType.Equals("NumberLock"))
            {
                GameObject.Find(lockObjectName).GetComponent<NumberlockController>().ShowNumberlock();
                lockInfoMiniDesc.SetActive(true);
                //GameObject.Find(lockObjectName).GetComponent<BoxCollider>().enabled = false;
                //lockGuide.gameObject.SetActive(true);
            }
            else if (lockType.Equals("WordLock"))
            {
                GameObject.Find(lockObjectName).GetComponent<WordlockController>().ShowWordlock();
                lockInfoMiniDesc.SetActive(true);
                //GameObject.Find(lockObjectName).GetComponent<BoxCollider>().enabled = false;
                //lockGuide.gameObject.SetActive(true);
            }

            // 인벤토리에 손전등이 있고, 손전등을 켜놓은 상태일 경우
            // 자물쇠의 카메라에도 후레쉬가 비춰지도록 함
            if (HasFlashlightInInventory() && isFlashOn)
            {
                GameObject
                    .Find(hitInfo.transform.GetComponent<Lock>().mappingCameraControllerName).transform
                    .Find("LockCamera").GetComponent<Camera>().GetComponent<Light>().enabled = true;
            }

            Lock.lockSolvingActivated = !Lock.lockSolvingActivated;

            ChangeMainCamera();

            // 크로스헤어 비활성화
            ClearCrossHair();

            // 모든 다른 플레이어들의 해당 락의 isOtherSolving 값을 true 로 변경
            int viewId = hitInfo.transform.gameObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("RPC_SetIsOtherSolvingTrue", RpcTarget.All, viewId);
        }
    }

    IEnumerator ShowNotificationText(float delay, string message)
    {
        notificationText.text = message;
        //notificationBackground.SetActive(true);
        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        //notificationBackground.SetActive(false);
        notificationText.gameObject.SetActive(false);
    }

    private bool HasOldLockKeyInInventory(string lockObjectName)
    {
        Slot[] slots = theInventory.slots;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item.name.Contains("_Key"))
            {
                if (slots[i].item.name.Replace("_Key", "").Equals(lockObjectName))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool HasFlashlightInInventory()
    {
        Slot[] slots = theInventory.slots;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item.name.Contains("Flashlight"))
            {
                return true;
            }
        }

        return false;
    }

    // 메인 카메라에서 자물쇠 확대 카메라로 변환
    public void ChangeMainCamera()
    {
        ClearActiveText("Lock");
        currentCamera.gameObject.SetActive(false);
        targetCamera = GameObject
            .Find(hitInfo.transform.GetComponent<Lock>().mappingCameraControllerName).transform
            .Find("LockCamera").GetComponent<Camera>();
        targetCamera.gameObject.SetActive(true);
        currentCamera = targetCamera;
    }

    // 자물쇠 확대 카메라에서 메인 카메라로 변환
    public static void RestoreMainCamera()
    {
        // 자물쇠 카메라 조명 제거
        currentCamera.GetComponent<Light>().enabled = false;

        // 크로스헤어 활성화
        ShowCrossHair();
        currentCamera.gameObject.SetActive(false);
        targetCamera = GameObject.Find("MainController").transform.Find("MainCamera").GetComponent<Camera>();
        targetCamera.gameObject.SetActive(true);
        currentCamera = targetCamera;
    }

    // 객체 하이라이트
    private void ShowObjectHighlight(Transform parent, int idx)
    {
        if (idx == 0 && parent.tag == "ActionObject")
        {
            if (!parent.GetComponent<InteractionObject>().myLock.GetComponent<Lock>().isSolved)
            {
                return;
            }
        }

        // 서랍 안에 있는 열쇠, 서류는 하이라이팅 X
        if (idx > 0 && (parent.name.Contains("_Key") || parent.name.Contains("Paper")/*|| parent.name.Contains("Post")*/))
        {
            return;
        }

        cakeslice.Outline outLine = parent.GetComponent<cakeslice.Outline>();

        if (outLine != null)
        {
            outLine.enabled = true;
            outLine.eraseRenderer = false;
        }

        foreach (Transform child in parent)
        {
            // 자식 객체 중 Outline 스크립트가 걸려있는 오브젝트는 모두 하이라이트 됨
            ShowObjectHighlight(child, idx + 1);
        }
    }

    // 객체 하이라이트 제거
    private void ClearObjectHighlight(Transform parent)
    {
        cakeslice.Outline outLine = parent.GetComponent<cakeslice.Outline>();

        if (outLine != null)
        {
            outLine.enabled = false;
            outLine.eraseRenderer = true;
        }

        foreach (Transform child in parent)
        {
            // 자식 객체 중 Outline 스크립트가 걸려있는 오브젝트는 모두 하이라이트를 제거
            ClearObjectHighlight(child);
        }
    }

    private void ShowActiveText(string tag)
    {
        if (tag == "Item") 
        {
            itemPickupActivated = true;
            actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName +
                " 획득 " + "<color=yellow>" + "(E)" + "</color>";
        }
        else if (tag == "Interaction")
        {
            readObjectDetailActivated = true;
            actionText.text = hitInfo.transform.GetComponent<InteractionObject>().objectName +
                " 확인 " + "<color=red>" + "(F)" + "</color>";
        }
        else if (tag == "Note")
        {
            readNoteDetailActivated = true;
            actionText.text = hitInfo.transform.GetComponent<Note>().noteName +
                " 확인 " + "<color=red>" + "(F)" + "</color>";
        }
        else if (tag == "Lock")
        {
            readLockDetailActivated = true;
            actionText.text = hitInfo.transform.GetComponent<Lock>().lockName +
                " 열기 " + "<color=red>" + "(F)" + "</color>";
        }
        else if (tag == "ActionObject")
        {
            // 사물에 걸려있는 자물쇠가 풀려있는지 확인
            if (hitInfo.transform.GetComponent<InteractionObject>().myLock.gameObject.GetComponent<Lock>().isSolved)
            {
                actionObjectActivated = true;
                actionText.text = hitInfo.transform.GetComponent<InteractionObject>().objectName +
                    " 열고 닫기 " + "<color=red>" + "(F)" + "</color>";
            }
            else
            {
                return;
            }
        }

        actionText.gameObject.SetActive(true);
    }

    private void ClearActiveText(string tag)
    {
        if (tag == "Item")
        {
            itemPickupActivated = false;
        }
        else if (tag == "Interaction")
        {
            readObjectDetailActivated = false;
        }
        else if (tag == "Note")
        {
            readNoteDetailActivated = false;
        }
        else if (tag == "Lock")
        {
            readLockDetailActivated = false;
        }
        else if (tag == "ActionObject")
        {
            actionObjectActivated = false;
        }

        actionText.gameObject.SetActive(false);
    }

    private static void ShowCrossHair()
    {
        CrossHair.crossHairActivated = CrossHair.preIsCrossHair;
        CrossHair.ToggleCrossHair();
    }

    private static void ClearCrossHair()
    {
        CrossHair.preIsCrossHair = !CrossHair.crossHairActivated;
        CrossHair.crossHairActivated = true;
        CrossHair.ToggleCrossHair();
    }

    private void ItemPickUp()
    {
        if (itemPickupActivated && hitInfo.transform != null)
        {
            if (theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickup>().item))
            {
                // 모든 유저와 동기화
                int viewId = hitInfo.transform.gameObject.GetComponent<PhotonView>().ViewID;
                photonView.RPC("RPC_DeleteItemPrefab", RpcTarget.All, viewId);

                // 내 코드
                Destroy(hitInfo.transform.gameObject);
            }
            else
            {
                StartCoroutine(ShowNotificationText(1.0f, "인벤토리가 가득 찼습니다."));
            }

            ClearActiveText("Item");
        }
    }

    [PunRPC]
    private void RPC_DeleteItemPrefab(int itemPhotonViewID)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemPhotonViewID);

        if (itemPhotonView != null)
        {
            GameObject item = itemPhotonView.gameObject;
            ClearActiveText("Item");
            Destroy(item);
        }
    }

    [PunRPC]
    private void RPC_SetIsOtherSolvingTrue(int itemPhotonViewID)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemPhotonViewID);

        if (itemPhotonView != null)
        {
            GameObject lockObject = itemPhotonView.gameObject;
            lockObject.GetComponent<Lock>().isOtherSolving = true;
        }
    }

    private IEnumerator PostRequest()
    {
        Debug.Log("최종 클리어 칭호 저장 요청");

        string json = JsonUtility.ToJson(
            new RoomEscapeInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                titleIdx = 8,
            }
        );

        using (UnityWebRequest webRequest = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/title/earn", "POST"))
        {
            // Content-Type 헤더를 설정합니다.
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 데이터를 업로드 핸들러에 할당합니다.
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));

            // 다운로드 핸들러를 할당합니다. 이것은 서버로부터의 응답을 처리합니다.
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 요청 보내기
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                // 오류 처리
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // 응답 처리
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }
}

