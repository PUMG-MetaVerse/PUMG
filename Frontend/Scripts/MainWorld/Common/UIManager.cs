using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using VivoxUnity;

public class UIManager : MonoBehaviour
{
    // public VivoxManager vivoxManager;

    public Text userName;
    public Text channelName;
    public Text message;

    public GameObject item;
    public Transform userPos;
    public Transform textPos;

    public Scrollbar userScrollbar;
    public Scrollbar textScrollbar;

    public void LoginBtn()
    {
        // vivoxManager.Login(userName.text);
    }

    public void LogOutBtn()
    {
        // vivoxManager.vivox.loginSession.Logout();
    }

    public void JoinChannelBtn()
    {
        // vivoxManager.JoinChannel(channelName.text, ChannelType.NonPositional);
    }

    public void LeaveChannelBtn()
    {
        // vivoxManager.vivox.channelSession.Disconnect();
        // vivoxManager.vivox.loginSession.DeleteChannelSession(new ChannelId(vivoxManager.vivox.issuer, channelName.text, vivoxManager.vivox.domain, ChannelType.NonPositional));
    }

    public void InputChat(string str)
    {
        var temp = Instantiate(item, textPos);
        temp.GetComponentInChildren<Text>().text = str;
    }
    
    public void InputUser(string str)
    {
        var temp = Instantiate(item, userPos);
        temp.GetComponentInChildren<Text>().text = str;
    }

    public void messageBtn()
    {
        // vivoxManager.SendMessage(message.text);
    }
}
