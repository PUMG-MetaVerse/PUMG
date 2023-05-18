using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가
using UnityEngine;  
using UnityEngine.UI;
using Photon.Pun;
using VivoxUnity;
using TMPro;
public class SystemUiManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button toggleButton;
    public Button optionButton;
    public Button quitButton;
    // 삭제 예정
    public Button returnButton;
    public GameObject systemObject;
    public GameObject optionObject;
    public GameObject KeyHelp;
    public TMP_Dropdown speakerDropdown;
    public TMP_Dropdown microphoneDropdown;
    private VivoxManager vivoxManager; 
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown screenModeDropdown;
    public TMP_InputField ChatInput;
    private Resolution[] resolutions;

    private bool isSystemUIActive = false;
    
    void Start()
    {
        // 버튼 클릭 시 TogglePartyVisibility 함수 호출
        toggleButton.onClick.AddListener(ToggleSystemUI);
        optionButton.onClick.AddListener(ToggleOptionUI);
        quitButton.onClick.AddListener(QuitGame);
        // 삭제 예정
        GameObject inputFieldObject = GameObject.Find("ChatBox"); // InputField GameObject의 이름을 지정합니다.
        if (inputFieldObject != null)
        {
            ChatInput = inputFieldObject.GetComponent<TMP_InputField>(); // InputField 컴포넌트를 가져옵니다.
        }
        else
        {
            Debug.LogError("InputField GameObject not found");
        }
        if(returnButton != null) 
        {
            returnButton.onClick.AddListener(ReturnToMainMenu);
        }
        vivoxManager = FindObjectOfType<VivoxManager>();
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        
        // Dropdown 메뉴에 해상도 옵션 추가하기
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        screenModeDropdown.value = (int)Screen.fullScreenMode;
        screenModeDropdown.RefreshShownValue();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        int currentIndex = 0;

        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            currentIndex = 0;
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            currentIndex = 1;
        }
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            currentIndex = 2;
        }

        screenModeDropdown.value = currentIndex;
        screenModeDropdown.RefreshShownValue();
        
    }
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !KeyHelp.activeInHierarchy) // Escape 키를 사용자가 정의한 키로 변경할 수 있습니다.
        {
            if(optionObject.activeSelf)
            {
                optionObject.SetActive(false);
            }
            else
            {
                ToggleSystemUI();
            }
        }    
    }
     // 옵션 UI를 토글하는 메서드
    void ToggleOptionUI()
    {
        if(KeyHelp.activeInHierarchy) return;
        FillSpeakerDropdown();
        FillMicrophoneDropdown();
        optionObject.SetActive(!optionObject.activeSelf);
    }
    void ToggleSystemUI()
    {
        if(KeyHelp.activeInHierarchy) return;
        isSystemUIActive = !isSystemUIActive;
        systemObject.SetActive(isSystemUIActive);
        
        // 마우스 커서 표시/숨기기
        // Cursor.visible = isSystemUIActive;
        // Cursor.lockState = isSystemUIActive ? CursorLockMode.None : CursorLockMode.Locked;
        if (isSystemUIActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void QuitGame()
    {
        // 에디터에서 실행 중이면 에디터를 중지하고, 빌드된 게임에서 실행 중이면 게임을 종료합니다.
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // 삭제 예정
    void ReturnToMainMenu()
    {
        StartCoroutine(ReturnToMainMenuCoroutine());
    }

    public bool IsSystemUIActive()
    {
        return isSystemUIActive;
    }

    
    public void FillSpeakerDropdown()
    {
        speakerDropdown.ClearOptions();
        List<string> options = new List<string>();

        List<IAudioDevice> outputDevices = vivoxManager.GetOutputDevices();
        for (int i = 0; i < outputDevices.Count; i++)
        {
            options.Add(outputDevices[i].Name);
        }

        speakerDropdown.AddOptions(options);
        speakerDropdown.onValueChanged.AddListener(SetSpeakerDevice);
    }
    public void SetSpeakerDevice(int index)
    {
        IAudioDevice device = vivoxManager.GetOutputDevices()[index];
        vivoxManager.SetOutputDevice(device);
    }

    private void FillMicrophoneDropdown()
    {
        microphoneDropdown.ClearOptions();
        List<string> options = new List<string>();

        List<IAudioDevice> inputDevices = vivoxManager.GetInputDevices();
        for (int i = 0; i < inputDevices.Count; i++)
        {
            options.Add(inputDevices[i].Name);
        }

        microphoneDropdown.AddOptions(options);
        microphoneDropdown.onValueChanged.AddListener(SetMicrophoneDevice);
    }
    private void SetMicrophoneDevice(int index)
    {
        IAudioDevice device = vivoxManager.GetInputDevices()[index];
        vivoxManager.SetInputDevice(device);
    }
    // public void SetResolution(int resolutionIndex)
    // {
    //     Resolution resolution = resolutions[resolutionIndex];
    //     Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    // }
    // public void SetScreenMode(int modeIndex)
    // {
    //     if (modeIndex == 0)
    //     {
    //         Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    //     }
    //     else if (modeIndex == 1)
    //     {
    //         Screen.fullScreenMode = FullScreenMode.Windowed;
    //     }
    //     else if (modeIndex == 2)
    //     {
    //         Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    //     }
    // }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Save the resolution index
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetScreenMode(int modeIndex)
    {
        if (modeIndex == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (modeIndex == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (modeIndex == 2)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        // Save the screen mode
        PlayerPrefs.SetInt("modeIndex", modeIndex);
        PlayerPrefs.Save();
    }


    // 삭제 예정
    IEnumerator ReturnToMainMenuCoroutine()
    {
        // Photon 네트워크에서 연결 해제
        PhotonNetwork.Disconnect();

        // 연결 해제가 완료될 때까지 대기
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        // Main Scene으로 이동
        SceneManager.LoadScene("04 - City");

        // 씬 전환 완료를 기다립니다.
        yield return null;

        // Photon 네트워크에 다시 연결
        PhotonNetwork.ConnectUsingSettings();
    }

}