using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using System;
using UnityEngine.EventSystems;
public class Loginview : MonoBehaviour
{    
    EventSystem system;

    public TMP_InputField Input_id;
    public TMP_InputField Input_password;
    public TMP_InputField Signup_Input_id;
    public TMP_InputField Signup_Input_password;
    public Button submitButton;
    public Button signUpSubmitButton;
    public GameObject alert;
    public GameObject alert2;
    public GameObject SignUp_Pannel;
    public GameObject Login_Pannel;
    void Start()
    {
        system = EventSystem.current;
        // 처음은 이메일 Input Field를 선택하도록 한다.
        alert = GameObject.Find("Alert");
        alert2 = GameObject.Find("Alert2");
        SignUp_Pannel = GameObject.Find("SignUpPannel");
        Login_Pannel = GameObject.Find("LoginPannel");
        alert.SetActive(false);
        alert2.SetActive(false);
        SignUp_Pannel.SetActive(false);
    }
    void Update() 
    {
         if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 Selectable 객체를 선택
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab은 아래의 Selectable 객체를 선택
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // 엔터키를 치면 로그인 (제출) 버튼을 클릭
            if(Login_Pannel.activeInHierarchy)
            {
                submitButton.onClick.Invoke();
            }
            else if(SignUp_Pannel.activeInHierarchy)
            {
                signUpSubmitButton.onClick.Invoke();
            }
            Debug.Log("Button pressed!");
        }
    }
    public void SignUp()
    {
        StartCoroutine(SendSignUpData());
    }
    public void Back()
    {
        // goLoginPage();
        Login_Pannel.SetActive(true);
        SignUp_Pannel.SetActive(false);
    }

    public void Login()
    {
        StartCoroutine(SendLoginData());
    }
    public void goSignUp()
    {
        // goSignUpPage();
        Login_Pannel.SetActive(false);
        SignUp_Pannel.SetActive(true);
    }
    public void ExitAlert()
    {
        alert.SetActive(false);
    }
    public void ExitAlert2()
    {
        alert2.SetActive(false);
    }
    private IEnumerator SendLoginData()
    {
        // 로그인 정보를 JSON 형식으로 작성
        string json = JsonUtility.ToJson(new UserLoginData { userId = Input_id.text, userPassword = Input_password.text });

        // 웹 요청을 생성하고, URL과 HTTP 메서드를 설정합니다.
        using (UnityWebRequest request = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/user/login", "POST"))
        {
            // JSON 형식의 데이터를 전송하기 위한 헤더 설정
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();

            // 웹 요청을 보냅니다.
            yield return request.SendWebRequest();

            // 요청이 완료되면 결과를 처리합니다.
            if (request.result == UnityWebRequest.Result.Success)
            {
                 string jsonResponse = request.downloadHandler.text;

                // JSON 문자열을 JsonResponse 클래스 오브젝트로 파싱합니다.
                JsonResponse response = JsonUtility.FromJson<JsonResponse>(jsonResponse);
                // 결과값을 받아옵니다.
                Debug.Log("Message: " + response.message);
                Debug.Log("Status: " + response.status);

                UserInfo userInfo = response.data.userInfo;
                Debug.Log("Idx: " + userInfo.idx);
                
                PlayerPrefs.SetInt("Idx", userInfo.idx);
                // PlayerPrefs.GetString("PlayerName",userInfo.userNickname);            
                // PlayerPrefs.SetInt("CharacterNum",Int32.Parse(userInfo.worldCharacter));
                Debug.Log("UserId: " + userInfo.userId);
                Debug.Log("UserNickname: " + userInfo.userNickname);
                Debug.Log("WorldCharacter: " + userInfo.worldCharacter);
                Debug.Log("HealingCharacter: " + userInfo.healingCharacter);
                Debug.Log("UserTitle : " + userInfo.userTitle);
                // string result = request.downloadHandler.text.data.userInfo.idx;
                
                
                if(userInfo.userNickname=="")
                {
                    PlayerPrefs.SetString("PlayerTitle", userInfo.userTitle);
                    PlayerPrefs.SetString("HealingCharacterNum",userInfo.healingCharacter);
                    goSelectCharacter();
                }
                else
                {
                    // PlayerPrefs.SetString("PlayerName", "TEST");
                    // LoadGameScene();
                    // PlayerPrefs.SetString("CharacterNum", result);
                    PlayerPrefs.SetString("PlayerName", userInfo.userNickname);
                    PlayerPrefs.SetString("PlayerTitle", userInfo.userTitle);

                    // PlayerPrefs.SetString("CharacterNum",userInfo.worldCharacter);
                    PlayerPrefs.SetInt("CharacterNum",Int32.Parse(userInfo.worldCharacter));
                    PlayerPrefs.SetString("HealingCharacterNum",userInfo.healingCharacter);
                    goWorld();
                }
                // Debug.Log(result);
                // 결과값에 따라 다음 작업을 수행합니다. 예를 들어, 게임 씬을 로드하거나 오류 메시지를 표시합니다.
                // if (result == "login success")
                // {
                //     LoadGameScene();
                // }
                // else
                // {
                //     Debug.Log("로그인에 실패했습니다.");
                // }
            }
            else
            {
                // Debug.Log("웹 요청에 실패했습니다: " + request.error);
                // alert = GameObject.Find("Alert");
                alert.SetActive(true);
                
            }
        }
    }


    private IEnumerator SendSignUpData()
    {
        // 로그인 정보를 JSON 형식으로 작성
        string json = JsonUtility.ToJson(new UserSignUpData { userId = Signup_Input_id.text, userPassword = Signup_Input_password.text });

        // 웹 요청을 생성하고, URL과 HTTP 메서드를 설정합니다.
        using (UnityWebRequest request = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/user/signup", "POST"))
        {
            // JSON 형식의 데이터를 전송하기 위한 헤더 설정
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();

            // 웹 요청을 보냅니다.
            yield return request.SendWebRequest();


            string jsonResponse = request.downloadHandler.text;

            // JSON 문자열을 JsonResponse 클래스 오브젝트로 파싱합니다.
            JsonResponse response = JsonUtility.FromJson<JsonResponse>(jsonResponse);
            // 결과값을 받아옵니다.
            Debug.Log("Message: " + response.message);
            // Debug.Log("Status: " + response.status);

            UserInfo userInfo = response.data.userInfo;


            // Debug.Log("UserId: " + userInfo.userId);
            // Debug.Log("UserNickname: " + userInfo.userNickname);
            // Debug.Log("WorldCharacter: " + userInfo.worldCharacter);
            // Debug.Log("HealingCharacter: " + userInfo.healingCharacter);


            // 요청이 완료되면 결과를 처리합니다.
            if (request.result == UnityWebRequest.Result.Success)
            {
                // 결과값을 받아옵니다.
                string result = request.downloadHandler.text;

                // 결과값에 따라 다음 작업을 수행합니다. 예를 들어, 게임 씬을 로드하거나 오류 메시지를 표시합니다.
                if (response.message == "Success")
                {
                                // Debug.Log("Idx: " + userInfo.idx);
                    goLoginPage();
                }
                else
                {
                    Debug.Log("로그인에 실패했습니다.");
                }
            }
            else
            {
                Debug.Log("웹 요청에 실패했습니다: " + request.error);
                alert2.SetActive(true);
            }
        }
    }

    private void goLoginPage()
    {
        SceneManager.LoadScene("MainUI");
    }
    private void goSelectCharacter()
    {
        SceneManager.LoadScene("SelectChar_main");
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene("SetNickName");
    }
    private void goSignUpPage()
    {
        SceneManager.LoadScene("SignUp");
    }
    private void goWorld()
    {
        LoadingSceneController.LoadScene("04 - City");
        // SceneManager.LoadScene("04 - City");
    }
}
[System.Serializable]
public class UserLoginData
{
    public string userId;
    public string userPassword;
}

[System.Serializable]
public class UserSignUpData
{
    public string userId;
    public string userPassword;
}

[System.Serializable]
public class UserInfo
{
    public int idx;
    public string userId;
    public string userNickname;
    public string worldCharacter;
    public string healingCharacter;
    public string userTitle;
}

[System.Serializable]
public class UserData
{
    public UserInfo userInfo;
}

[System.Serializable]
public class JsonResponse
{
    public string message;
    public int status;
    public UserData data;
}