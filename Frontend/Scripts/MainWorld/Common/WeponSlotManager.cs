using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeponSlotManager : MonoBehaviour
{
public RawImage[] weaponSlots;  // 각 무기 슬롯의 RawImage 컴포넌트에 대한 참조를 저장합니다.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectWeapon(3);
    }

    private void SelectWeapon(int index)
    {
        // 모든 슬롯의 투명도를 낮춥니다.
        foreach (RawImage weaponSlot in weaponSlots)
        {
            Color color = weaponSlot.color;
            color.a = 0.88f;  // 225 / 255
            weaponSlot.color = color;
        }

        // 선택된 슬롯의 투명도를 높입니다.
        Color selectedColor = weaponSlots[index].color;
        selectedColor.a = 1f;
        weaponSlots[index].color = selectedColor;
    }


}
