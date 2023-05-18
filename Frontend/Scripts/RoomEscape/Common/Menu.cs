using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class Menu : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryBackground;
    public GameObject itemDetail;
    public GameObject itemPrefabImage;
    public GameObject menu;
    public Button exitGame;
    public Button cancel;

    private bool cursorVisible;

    public static bool isActiveMenu = false;       // �޴� Ȱ�� ����

    // Start is called before the first frame update
    void Start()
    {
        exitGame.onClick.AddListener(OnClickExit);
        // exitGame.GetComponentInChildren<TMP_Text>().text = "���� ����";

        cancel.onClick.AddListener(OnClickCancel);
        // cancel.GetComponentInChildren<TMP_Text>().text = "���";

        isActiveMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetKeyEvent();
    }

    private void SetKeyEvent()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Inventory.inventoryActivated)
            {
                inventory.ActiveInventory(false);
                itemDetail.SetActive(false);
                foreach (Transform child in itemPrefabImage.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                isActiveMenu = !isActiveMenu;  
                menu.SetActive(isActiveMenu);
                inventoryBackground.SetActive(isActiveMenu);
                ToggleCursor();
            }
        }
    }

    public void ToggleCursor()
    {
        cursorVisible = !cursorVisible;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnClickExit()
    {
        StartCoroutine(ReturnToMainMenuCoroutine());
    }

    private void OnClickCancel()
    {
        ToggleCursor();
        isActiveMenu = !isActiveMenu;
        menu.SetActive(isActiveMenu);
        inventoryBackground.SetActive(isActiveMenu);
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
        isActiveMenu = false;
        CrossHair.crossHairActivated = true;
        CrossHair.preIsCrossHair = false;

        ActionController.itemPickupActivated = false;
        ActionController.readObjectDetailActivated = false;
        ActionController.readLockDetailActivated = false;
        ActionController.readNoteDetailActivated = false;
        ActionController.actionObjectActivated = false;
        ActionController.isFlashOn = false;
        ActionController.isSecondFloorClear = false;
        ActionController.isFirstFloorClear = false;

        ActionController.hitInfo = new RaycastHit();
        ActionController.preHitInfo = new RaycastHit();
        ActionController.currentCamera = null;
        ActionController.targetCamera = null;

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
}
