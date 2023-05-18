using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牢亥配府客 包访等 内靛
public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    public GameObject goInventoryBase;
    public GameObject goSlotsParent;
    public GameObject itemDetail;
    public GameObject itemPrefabImage;
    public GameObject inventoryBackground;
    public GameObject inventoryMiniDesc;
    public Slot[] slots;
    private bool cursorVisible;

    // Start is called before the first frame update
    void Start()
    {
        slots = goSlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    public void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I)
            && !InteractionObject.objectDetailTextActivated
            && !Lock.lockSolvingActivated
            && !Note.noteActivated
            && !KeyInfo.keyInfoActivated
        )
        {
            inventoryActivated = !inventoryActivated;

            inventoryBackground.transform.SetSiblingIndex(0);

            if (inventoryActivated)
            {
                CrossHair.preIsCrossHair = !CrossHair.crossHairActivated;
                CrossHair.crossHairActivated = true;
                OpenInventory();
                ToggleCursor();
                inventoryBackground.SetActive(true);
            }
            else
            {
                CrossHair.crossHairActivated = CrossHair.preIsCrossHair;
                CloseInventory();
                ToggleCursor();
                inventoryBackground.SetActive(false);
            }

            if (itemDetail.activeSelf)
            {
                itemDetail.SetActive(false);
            }

            foreach (Transform child in itemPrefabImage.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
            }

            CrossHair.ToggleCrossHair();
        }
    }

    public void ActiveInventory(bool active)
    {
        inventoryActivated = active;
        if (inventoryActivated)
        {
            CrossHair.preIsCrossHair = !CrossHair.crossHairActivated;
            CrossHair.crossHairActivated = true;
            goInventoryBase.SetActive(true);
            OpenInventory();
            ToggleCursor();
            inventoryBackground.SetActive(true);
        }
        else
        {
            CrossHair.crossHairActivated = CrossHair.preIsCrossHair;
            CloseInventory();
            ToggleCursor();
            inventoryBackground.SetActive(false);
        }
    }

    public void OpenInventory()
    {
        goInventoryBase.SetActive(true);
        inventoryMiniDesc.SetActive(true);
    }

    public void CloseInventory()
    {
        goInventoryBase.SetActive(false);
        inventoryMiniDesc.SetActive(false);
    }

    public void ToggleCursor()
    {
        cursorVisible = !cursorVisible;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public bool AcquireItem(Item _item, int _count = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return true;
            }
        }

        return false;
    }
}
