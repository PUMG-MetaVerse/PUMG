using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

public class AeroCraftRankingController : MonoBehaviour
{
    public GameObject rankingUIPanel;
    public Text[] rankingRows;
    private PhotonView photonView;
    public Text rankingScoreText;
    public void ToggleUIPanel()
    {
        rankingUIPanel.SetActive(!rankingUIPanel.activeSelf);
    }

    public void GetTargetRanking()
    {
        rankingScoreText.text = "점수"; 
        StartCoroutine(FetchRankingData());
    }

    public void GetRaceRanking()
    {
        rankingScoreText.text = "클리어 횟수"; 
        StartCoroutine(FetchRacingRankingData());
    }
    public void callRankingUI()
    {
        StartCoroutine(FetchRankingData());
        ToggleUIPanel();
    }

    private void OnTriggerEnter(Collider other)
    {
        photonView = GetComponent<PhotonView>();
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine) // "Player" 태그가 있는 오브젝트와 충돌했을 때
        {
            Debug.Log("부딪힘 ");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(FetchRankingData());
            ToggleUIPanel();
        }
    }

    private IEnumerator FetchRankingData()
    {
        string url = "http://k8b108.p.ssafy.io:6999/api/v1/flight/get-rank";
        Debug.Log("서버 요청 들어옴");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            // jsonResponse를 파싱하여 랭킹 정보를 처리합니다.
            // JSON 데이터를 파싱하여 랭킹 정보를 가져옵니다.
            JsonResponse response = JsonUtility.FromJson<JsonResponse>(jsonResponse);

            Debug.Log(jsonResponse);
            Debug.Log(response.data);
            if (response.data.rankingInfo.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    RankingData rankingData = response.data.rankingInfo[i];

                    int index = i * 4;
                    rankingRows[index].text = (i + 1).ToString(); // 등수
                    rankingRows[index + 1].text = rankingData.nickName; // 닉네임
                    rankingRows[index + 2].text = rankingData.score.ToString(); // 점수
                    rankingRows[index + 3].text = rankingData.clearTime; // 클리어 시간
                }
            }
            else
            {
                for (int i = 0; i < response.data.rankingInfo.Count; i++)
                {
                    RankingData rankingData = response.data.rankingInfo[i];

                    int index = i * 4;
                    rankingRows[index].text = (i + 1).ToString(); // 등수
                    rankingRows[index + 1].text = rankingData.nickName; // 닉네임
                    rankingRows[index + 2].text = rankingData.score.ToString(); // 점수
                    rankingRows[index + 3].text = rankingData.clearTime; // 클리어 시간
                }
                for (int i = response.data.rankingInfo.Count; i < 10; i++)
            {
                // RankingData rankingData = response.data.rankingInfo[i];

                int index = i * 4;
                rankingRows[index].text = (i + 1).ToString(); // 등수
                rankingRows[index + 1].text = ""; // 닉네임
                rankingRows[index + 2].text = ""; // 점수
                rankingRows[index + 3].text = ""; // 클리어 시간
            }
            }
        }
        else
        {
            Debug.LogError($"GET 요청 실패: {request.error}");
        }
    }

    private IEnumerator FetchRacingRankingData()
    {
        string url = "http://k8b108.p.ssafy.io:6999/api/v1/flight/get-rank/race";
        Debug.Log("레이스 서버 요청 들어옴");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            // jsonResponse를 파싱하여 랭킹 정보를 처리합니다.
            // JSON 데이터를 파싱하여 랭킹 정보를 가져옵니다.
            JsonResponse response = JsonUtility.FromJson<JsonResponse>(jsonResponse);

            Debug.Log(jsonResponse);
            Debug.Log(response.data);
            if (response.data.rankingInfo.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    RankingData rankingData = response.data.rankingInfo[i];

                    int index = i * 4;
                    rankingRows[index].text = (i + 1).ToString(); // 등수
                    rankingRows[index + 1].text = rankingData.nickName; // 닉네임
                    rankingRows[index + 2].text = rankingData.score.ToString(); // 점수
                    rankingRows[index + 3].text = rankingData.clearTime; // 클리어 시간
                }
            }
            else
            {
                for (int i = 0; i < response.data.rankingInfo.Count; i++)
                {
                    RankingData rankingData = response.data.rankingInfo[i];

                    int index = i * 4;
                    rankingRows[index].text = (i + 1).ToString(); // 등수
                    rankingRows[index + 1].text = rankingData.nickName; // 닉네임
                    rankingRows[index + 2].text = rankingData.score.ToString(); // 점수
                    rankingRows[index + 3].text = rankingData.clearTime; // 클리어 시간
                }
                for (int i = response.data.rankingInfo.Count; i < 10; i++)
            {
                // RankingData rankingData = response.data.rankingInfo[i];

                int index = i * 4;
                rankingRows[index].text = (i + 1).ToString(); // 등수
                rankingRows[index + 1].text = ""; // 닉네임
                rankingRows[index + 2].text = ""; // 점수
                rankingRows[index + 3].text = ""; // 클리어 시간
            }
            }
        }
        else
        {
            Debug.LogError($"GET 요청 실패: {request.error}");
        }
    }

    [System.Serializable]
    public class RankingData
    {
        public int idx;
        public int userIdx;
        public string nickName;
        public int score;
        public string clearTime;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public List<RankingData> rankingInfo;
    }

    [System.Serializable]
    public class JsonResponse
    {
        public string message;
        public int status;
        public DataWrapper data;
    }
}
