using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerNameDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;

    public void SetName(string playerName)
    {
        playerNameText.text = playerName;
    }
}
