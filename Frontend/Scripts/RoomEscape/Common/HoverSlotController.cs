using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, /*IPointerClickHandler,*/ IPointerDownHandler, IPointerUpHandler
{
    public GameObject itemDetail;
    public GameObject itemPrefabImage;
    public Text ItemNameObject;
    public Text ItemDescObject;
    public static bool isMouseDown = false;
    public static bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        if (!isMouseDown)
        {
            ShowItemDetail();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        HideItemDetail();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDown = false;
        if (isMouseOver)
        {
            ShowItemDetail();
        }
    }

    private void ShowItemDetail()
    {
        if (gameObject.GetComponent<Slot>().item != null)
        {
            itemDetail.SetActive(true);
            ItemNameObject.text = GetComponent<Slot>().item.itemName;
            ItemDescObject.text = GetComponent<Slot>().item.itemDesc;

            string itemImageName = GetComponent<Slot>().item.name + "_Detail_Image";
            if (itemPrefabImage.transform.Find(itemImageName) != null)
            {
                itemPrefabImage.transform.Find(itemImageName).gameObject.SetActive(true);
            }
        }
    }

    private void HideItemDetail()
    {
        itemDetail.SetActive(false);

        if (GetComponent<Slot>().item != null)
        {
            string itemImageName = GetComponent<Slot>().item.name + "_Detail_Image";
            if (itemPrefabImage.transform.Find(itemImageName) != null)
            {
                itemPrefabImage.transform.Find(itemImageName).gameObject.SetActive(false);
            }
        }
    }
}
