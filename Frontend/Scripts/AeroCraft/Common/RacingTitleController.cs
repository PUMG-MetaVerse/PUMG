using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;

public class RacingTitleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private IEnumerator FastTitlePostRequest()
    {
        string json = JsonUtility.ToJson(new TitleInfo { userIdx = PlayerPrefs.GetInt("Idx"), });
        using (
            UnityWebRequest webRequest = new UnityWebRequest(
                "http://k8b108.p.ssafy.io:6999/api/v1/title/aerocraft/skyrush",
                "POST"
            )
        )
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
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }

    // 충돌 시
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("AeroCraft"))
        {
            checkTime();
        }
    }

    void checkTime()
    {
        GameObject timerObject = GameObject.Find("x-35_fbx_simulator");
        Aircraft_Racing_Controller timerInfo =
            timerObject.GetComponent<Aircraft_Racing_Controller>();
        Debug.Log(timerInfo.info.timerText.text);
        string clearTime = timerInfo.info.timerText.text;
        string compareTime = "02:10:00";

        TimeSpan clearTimeSpan = TimeSpan.Parse(clearTime);
        TimeSpan compareTimeSpan = TimeSpan.Parse(compareTime);

        if (clearTimeSpan < compareTimeSpan)
        {
            StartCoroutine(FastTitlePostRequest());
        }
    }

    [System.Serializable]
    public class TitleInfo
    {
        public int userIdx;
    }
}
