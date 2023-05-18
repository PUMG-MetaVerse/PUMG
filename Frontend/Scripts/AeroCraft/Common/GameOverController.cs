using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Text;

public class GameOverController : MonoBehaviour
{
    public GameObject loadingScreen; // 로딩 화면 UI를 연결할 변수

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject targetCountObject = GameObject.Find("TargetManager");
        TargetScoreManager targetCnt = targetCountObject.GetComponent<TargetScoreManager>();
        if(targetCnt.count >= 600) {
            StartCoroutine(GoodTitlePostRequest());
        }
        if(targetCnt.count >= 1000) {
            StartCoroutine(PerfectTitlePostRequest());
        }
    }

    // Update is called once per frame
    void Update() { }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string targetSceneName)
    {
        LoadingSceneController.LoadScene(targetSceneName);
        // StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private IEnumerator GoodTitlePostRequest()
    {
        
        string json = JsonUtility.ToJson(
            new TitleInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
            }
        );
        using (UnityWebRequest webRequest = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/title/aerocraft/lockon/good", "POST"))
        {
            // Content-Type 헤더를 설정합니다.
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 데이터를 업로드 핸들러에 할당합니다.
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));

            // 다운로드 핸들러를 할당합니다. 이것은 서버로부터의 응답을 처리합니다.
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // Debug.Log("진짜 여기까지는 옴??");

            // 요청 보내기
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }

        }
    }
    private IEnumerator PerfectTitlePostRequest()
    {
        
        string json = JsonUtility.ToJson(
            new TitleInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
            }
        );
        using (UnityWebRequest webRequest = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/title/aerocraft/lockon/perfect", "POST"))
        {
            // Content-Type 헤더를 설정합니다.
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 데이터를 업로드 핸들러에 할당합니다.
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));

            // 다운로드 핸들러를 할당합니다. 이것은 서버로부터의 응답을 처리합니다.
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // 요청 보내기
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }

        }
    }

    [System.Serializable]
    public class TitleInfo
    {
        public int userIdx;
    }
}
