
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using VivoxUnity;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class Vivox
{
    public Client client;
    // public Uri server = new Uri("https://unity.vivox.com/appconfig/14568-vivox-13727-udash");
    // public String issuer = "14568-vivox-13727-udash";
    // public String domain = "mtu1xp.vivox.com";
    // public String tokenKey = "qyyoD6WihD5gkL1P1iuLwXM1w3rhaqDf";
    public Uri server = new Uri("https://unity.vivox.com/appconfig/14568-vivox-13727-udash");
    public String issuer = "14568-vivox-13727-udash";
    public String domain = "mtu1xp.vivox.com";
    public String tokenKey = "qyyoD6WihD5gkL1P1iuLwXM1w3rhaqDf";
    public TimeSpan timeSpan = TimeSpan.FromSeconds(90);

    public ILoginSession loginSession;
    public IChannelSession channelSession;

    
}

public class VivoxManager : MonoBehaviour
{
    public static VivoxManager Instance;
    public bool channelJoined = false;
    public bool vivoxLoginCheck = false;
    public Vivox vivox = new Vivox();
    private List<string> ignoreString = new List<string>{ "Default Communication Device", "No Device" };
    public TMP_Dropdown speakerDropdown;
    public TMP_Dropdown microphoneDropdown;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log($"인스턴스 생성됨 : {Instance}");
            vivox.client = new Client();
            vivox.client.Uninitialize();
            vivox.client.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        // FillSpeakerDropdown();
        // FillMicrophoneDropdown();
    }

    private void OnApplicationQuit()
    {
        vivox.client.Uninitialize();
    }

    public void UserCallbacks(bool bind, IChannelSession session)
    {
        if (bind)
        {
            vivox.channelSession.Participants.AfterKeyAdded += AddUser;
            vivox.channelSession.Participants.BeforeKeyRemoved += LeaveUser;
        } 
        else
        {
            vivox.channelSession.Participants.AfterKeyAdded -= AddUser;
            vivox.channelSession.Participants.BeforeKeyRemoved -= LeaveUser;
        }
    }

    public void ChannelCallbacks(bool bind, IChannelSession session)
    {
        if (bind)
        {
            session.MessageLog.AfterItemAdded += ReceiveMessage;
        }
        else
        {
            session.MessageLog.AfterItemAdded -= ReceiveMessage;
        }
    }

    public void AddUser(object sender, KeyEventArg<string> userData)
    {
        var temp = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;

        IParticipant user = temp[userData.Key];
        // ui.InputChat($"{user.Account.Name} ���� ä�ο� �����߽��ϴ�.");
        // ui.InputUser(user.Account.Name);
    }

    public void LeaveUser(object sender, KeyEventArg<string> userData)
    {
        var temp = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;

        IParticipant user = temp[userData.Key];
        // ui.InputChat($"{user.Account.Name} ���� ä���� �������ϴ�.");
    }

    public void Login(string userName)
    {
        AccountId accountId = new AccountId(vivox.issuer, userName, vivox.domain);
        vivox.loginSession = vivox.client.GetLoginSession(accountId);
        vivox.loginSession.BeginLogin(vivox.server, vivox.loginSession.GetLoginToken(vivox.tokenKey, vivox.timeSpan), callback =>
        {
            try
            {
                vivox.loginSession.EndLogin(callback);
                // ui.InputChat("�α��� �Ϸ�");
                Debug.Log("로그인 ");
                vivoxLoginCheck = true;
                // JoinChannel("world",ChannelType.NonPositional);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
    }


    public void JoinChannel(string channelName, ChannelType channelType)
    {
        ChannelId channelId = new ChannelId(vivox.issuer, channelName, vivox.domain, ChannelType.Positional);
        Debug.Log($"채널 아이디 : {channelId}");
        vivox.channelSession = vivox.loginSession.GetChannelSession(channelId);
        Channel3DProperties properties = new Channel3DProperties();
        UserCallbacks(true, vivox.channelSession);
        ChannelCallbacks(true, vivox.channelSession);
        vivox.channelSession.BeginConnect(true, true, true,
            vivox.channelSession.GetConnectToken(vivox.tokenKey, vivox.timeSpan),
            callback =>
            {
                try
                {
                    vivox.channelSession.EndConnect(callback);
                    // ui.InputChat("ä�� ���� �Ϸ�");
                    Debug.Log("채널 진입 성공");
                    new System.Threading.Timer(callJoinTimer, null, 2000,-1);

                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
    }
    private void callJoinTimer(object? garbage)
    {
        channelJoined = true;
    }
    public void SendMessage(string str)
    {
        vivox.channelSession.BeginSendText(str, callback => 
        {
            try
            {
                vivox.channelSession.EndSendText(callback);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    public void ReceiveMessage(object sender, QueueItemAddedEventArgs<IChannelTextMessage> queueItemAddedEventArgs)
    {
        var name = queueItemAddedEventArgs.Value.Sender.Name;
        var message = queueItemAddedEventArgs.Value.Message;

        // ui.InputChat(name + " : " + message);
    }
    public void LeaveChannel()
    {
            vivox.channelSession.Disconnect();
    }

    public List<IAudioDevice> GetOutputDevices()
    {
        List<IAudioDevice> outputs = new List<IAudioDevice>();
        var available = vivox.client.AudioOutputDevices.ActiveDevice;
        outputs.Add(available);

        bool isAvailable = false;
        foreach(var device in vivox.client.AudioOutputDevices.AvailableDevices)
        {
            if (ignoreString.Contains(device.Name)) continue;
            if (device.Name.Equals(available.Name))
            {
                isAvailable = true;
                continue;
            }
            
            outputs.Add(device);
        }

        if (!isAvailable)
        {
            outputs.RemoveAt(0);
            SetOutputDevice(outputs[0]);
        }

        return outputs;
    }

    public List<IAudioDevice> GetInputDevices()
    {
        List<IAudioDevice> inputs = new List<IAudioDevice>();
        var available = vivox.client.AudioInputDevices.ActiveDevice;
        inputs.Add(available);

        bool isAvailable = false;
        foreach (var device in vivox.client.AudioInputDevices.AvailableDevices)
        {
            if (ignoreString.Contains(device.Name)) continue;
            if (device.Name.Equals(available.Name))
            {
                isAvailable = true;
                continue;
            }

            inputs.Add(device);
        }

        if (!isAvailable)
        {
            inputs.RemoveAt(0);
            SetInputDevice(inputs[0]);
        }

        return inputs;
    }

    public void SetOutputDevice(IAudioDevice device)
    {
        vivox.client.AudioOutputDevices.BeginSetActiveDevice(device, callback =>
        {
            vivox.client.AudioOutputDevices.EndSetActiveDevice(callback);
        });
    }

    public void SetInputDevice(IAudioDevice device)
    {
        vivox.client.AudioInputDevices.BeginSetActiveDevice(device, callback =>
        {
            vivox.client.AudioInputDevices.EndSetActiveDevice(callback);
        });
    }

}