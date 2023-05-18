using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;

public class KeyDisplayController : MonoBehaviour
{
    public Dictionary<KeyCode, Image> keyImages;

    void Start()
    {
        keyImages = new Dictionary<KeyCode, Image>
        {
            { KeyCode.UpArrow, GameObject.Find("UpArrow").GetComponent<Image>() },
            { KeyCode.DownArrow, GameObject.Find("DownArrow").GetComponent<Image>() },
            { KeyCode.LeftArrow, GameObject.Find("LeftArrow").GetComponent<Image>() },
            { KeyCode.RightArrow, GameObject.Find("RightArrow").GetComponent<Image>() },
            { KeyCode.Q, GameObject.Find("Q").GetComponent<Image>() },
            { KeyCode.E, GameObject.Find("E").GetComponent<Image>() },
            { KeyCode.F, GameObject.Find("F").GetComponent<Image>() },
            { KeyCode.Space, GameObject.Find("Space").GetComponent<Image>() },
        };
    }

    void Update()
    {
        foreach (var keyImage in keyImages)
        {
            if (Input.GetKey(keyImage.Key))
            {
                keyImage.Value.color = new Color(1, 0, 1, 1); // 키가 눌린 경우 불투명하게 표시
            }
            else
            {
                keyImage.Value.color = new Color(1, 0, 1, 0.5f); // 키가 눌리지 않은 경우 반투명하게 표시
            }
        }
    }
}
