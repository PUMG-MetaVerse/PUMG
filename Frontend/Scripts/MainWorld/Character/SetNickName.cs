using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using Photon.Pun;
public class SetNickName : MonoBehaviour
{
    public TMP_InputField Input_nickname;
    

    public void OnConfirmButtonClick()
    {
        string playerName = Input_nickname.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        SceneManager.LoadScene("04 - City");
    }

    // private void LoadGameScene()
    // {
    //     SceneManager.LoadScene("Test_MAP");
    // }
}
