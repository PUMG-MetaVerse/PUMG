using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 정보와 관련된 코드
// ScriptableObject : GameObject 에 붙일 필요가 없음
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}
