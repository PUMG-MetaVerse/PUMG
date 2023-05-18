using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class FirstFloorClear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !ActionController.isFirstFloorClear)
        {
            Debug.Log(PlayerPrefs.GetInt("Idx") + "번 플레이어가 1층을 탈출했습니다!!");

            // 칭호 API 쏘기
            StartCoroutine(PostRequest());

            ActionController.isFirstFloorClear = true;
        }
    }

    private IEnumerator PostRequest()
    {
        Debug.Log("1층 클리어 칭호 저장 요청");

        string json = JsonUtility.ToJson(
            new RoomEscapeInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                titleIdx = 7,
            }
        );

        using (UnityWebRequest webRequest = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/title/earn", "POST"))
        {
            // Content-Type 헤더를 설정합니다.
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 데이터를 업로드 핸들러에 할당합니다.
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));

            // 다운로드 핸들러를 할당합니다. 이것은 서버로부터의 응답을 처리합니다.
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 요청 보내기
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                // 오류 처리
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // 응답 처리
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }
}

