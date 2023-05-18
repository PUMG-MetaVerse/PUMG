using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

// 인벤토리 슬롯과 관련된 파일
public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Vector3 originPos;
    public Item item;       // 획득한 아이템
    public Image itemImage; // 아이템의 이미지
    public LayerMask groundLayer; // 지면 레이어
    public Transform player; // 플레이어 참조

    public static float currentDropAngle = 0f; // 현재 아이템 드롭 각도
    public static float dropRadius = 0.45f; // 아이템이 떨어지는 반지름
    public static float dropAngleStep = 65f; // 아이템 드롭 각도 간격

    private PhotonView photonView;

    void Start()
    {
        originPos = transform.position;
    }

    // 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        Debug.Log("Slot.cs : AddItem() called");
        Debug.Log("Slot.cs : itemImage : " + _item.itemImage);
        item = _item;
        itemImage.sprite = item.itemImage;
        SetColor(1);
    }

    // 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        SetColor(0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 인벤토리에서 아이템 버리기
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if (item != null)
    //    {
    //        // 드래그 앤 드롭이 끝난 위치에 있는 모든 레이캐스트 결과를 가져옵니다.
    //        List<RaycastResult> raycastResults = new List<RaycastResult>();
    //        EventSystem.current.RaycastAll(eventData, raycastResults);

    //        // 인벤토리 밖으로 드래그 앤 드롭되었는지 확인하기 위한 변수
    //        bool isOutsideInventory = true;

    //        foreach (RaycastResult result in raycastResults)
    //        {
    //            if (result.gameObject != null && result.gameObject.name == "Inventory_Base")
    //            {
    //                // 드래그 앤 드롭이 끝난 위치에 인벤토리가 있다면 인벤토리 밖으로 드래그 앤 드롭되지 않았다고 판단
    //                isOutsideInventory = false;
    //                break;
    //            }
    //        }

    //        if (isOutsideInventory)
    //        {
    //            // 떨어뜨린 아이템이 손전등일 경우
    //            if (item.itemName == "손전등")
    //            {
    //                player.GetComponent<Light>().enabled = false;
    //                ActionController.isFlashOn = false;
    //            }

    //            // 아이템이 떨어질 위치 계산
    //            Vector3 dropDirection = Quaternion.Euler(0, currentDropAngle, 0) * player.forward;
    //            Vector3 spawnPosition = player.position + dropDirection * dropRadius;

    //            // 아이템 프리팹 생성
    //            SpawnItem(item.itemPrefab, spawnPosition);

    //            // 아이템 생성 후, 인벤토리에서 아이템 제거
    //            ClearSlot();

    //            // 다음 아이템 드롭 각도 업데이트
    //            currentDropAngle += dropAngleStep;

    //        }
    //    }

    //    DragSlot.instance.SetColor(0);
    //    DragSlot.instance.dragSlot = null;
    //}

    public void OnEndDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            // 드래그 앤 드롭이 끝난 위치에 있는 모든 레이캐스트 결과를 가져옵니다.
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            // 인벤토리 밖으로 드래그 앤 드롭되었는지 확인하기 위한 변수
            bool isOutsideInventory = true;

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject != null && result.gameObject.name == "Inventory_Base")
                {
                    // 드래그 앤 드롭이 끝난 위치에 인벤토리가 있다면 인벤토리 밖으로 드래그 앤 드롭되지 않았다고 판단
                    isOutsideInventory = false;
                    break;
                }
            }

            if (isOutsideInventory)
            {
                // 떨어뜨린 아이템이 손전등일 경우
                if (item.itemName == "손전등")
                {
                    player.GetComponent<Light>().enabled = false;
                    ActionController.isFlashOn = false;
                }

                // 아이템이 떨어질 위치 계산
                Vector3 dropDirection = Quaternion.Euler(0, currentDropAngle, 0) * player.forward;
                Vector3 spawnPosition = player.position + dropDirection * dropRadius;

                // 아이템 프리팹 생성
                SpawnItem(item.itemPrefab, spawnPosition);

                // 아이템 생성 후, 인벤토리에서 아이템 제거
                ClearSlot();

                // 다음 아이템 드롭 각도 업데이트
                currentDropAngle += dropAngleStep;
            }
        }

        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void SpawnItem(GameObject itemPrefab, Vector3 spawnPosition)
    {
        RaycastHit hit;
        Vector3 raycastStartPos = new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z);
        // Player 레이어를 무시하는 LayerMask 생성
        int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        if (Physics.Raycast(raycastStartPos, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 groundedSpawnPosition = hit.point;
            PhotonNetwork.Instantiate(itemPrefab.name, groundedSpawnPosition + new Vector3(0, 0.01f, 0), Quaternion.identity * itemPrefab.transform.localRotation);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    public void ChangeSlot()
    {
        Item _tempItem = item;

        AddItem(DragSlot.instance.dragSlot.item);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
