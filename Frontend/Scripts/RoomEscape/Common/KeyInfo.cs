using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInfo : MonoBehaviour
{
    public static bool keyInfoActivated = false;

    public GameObject goKeyInfoBase;
    public GameObject inventoryBackground;
    public GameObject keyInfoMiniDesc;

    private bool cursorVisible;

    void Start()
    {
        //StartCoroutine(StartShowKeyInfo(4.0f));
        ShowKeyInfo();
    }

    void Update()
    {
        TryOpenKeyInfo();
    }

    public void TryOpenKeyInfo()
    {
        if (Input.GetKeyDown(KeyCode.H)
            && !InteractionObject.objectDetailTextActivated
            && !Lock.lockSolvingActivated
            && !Note.noteActivated
            && !Inventory.inventoryActivated
        )
        {
            ShowKeyInfo();
        }
    }

    public void ShowKeyInfo()
    {
        keyInfoActivated = !keyInfoActivated;

        inventoryBackground.transform.SetSiblingIndex(0);

        if (keyInfoActivated)
        {
            CrossHair.preIsCrossHair = !CrossHair.crossHairActivated;
            CrossHair.crossHairActivated = true;
            OpenKeyInfo();
            //ToggleCursor();
            inventoryBackground.SetActive(true);
        }
        else
        {
            CrossHair.crossHairActivated = CrossHair.preIsCrossHair;
            CloseKeyInfo();
            //ToggleCursor();
            inventoryBackground.SetActive(false);
        }

        CrossHair.ToggleCrossHair();
    }

    //IEnumerator StartShowKeyInfo(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    ShowKeyInfo();
    //}

    public void OpenKeyInfo()
    {
        goKeyInfoBase.SetActive(true);
        keyInfoMiniDesc.SetActive(true);
    }

    public void CloseKeyInfo()
    {
        goKeyInfoBase.SetActive(false);
        keyInfoMiniDesc.SetActive(false);
    }

    public void ToggleCursor()
    {
        cursorVisible = !cursorVisible;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
