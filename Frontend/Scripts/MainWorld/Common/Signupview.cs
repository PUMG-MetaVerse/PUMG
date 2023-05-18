using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;
using TMPro;
public class Signupview : MonoBehaviour
{
    public TMP_InputField Input_id;
    public TMP_InputField Input_password;
    

    public void SignUp()
    {
        StartCoroutine(SendSignUpData());
    }
    public void Back()
    {
        goLoginPage();
    }

    private IEnumerator SendSignUpData()
    {
        // 로그인 정보를 JSON 형식으로 작성
        string json = JsonUtility.ToJson(new LoginData { email = Input_id.text, password = Input_password.text });

        // 웹 요청을 생성하고, URL과 HTTP 메서드를 설정합니다.
        using (UnityWebRequest request = new UnityWebRequest("http://52.78.182.65:7999/user/signup", "POST"))
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
                // 결과값을 받아옵니다.
                string result = request.downloadHandler.text;

                // 결과값에 따라 다음 작업을 수행합니다. 예를 들어, 게임 씬을 로드하거나 오류 메시지를 표시합니다.
                if (result == "signup failure")
                {
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
            }
        }
    }

    private void goLoginPage()
    {
        SceneManager.LoadScene("MainUI");
    }
}
