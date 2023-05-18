using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;
// UI : UI 기능 사용을 위한 것
using UnityEngine.SceneManagement;
// SceneManagement : 씬 전환을 위한 것

public class TurnOnTheStage_Healing : MonoBehaviour {
    bool bTurnLeft = false;
    bool bTurnRight = false;
    private Quaternion turn = Quaternion.identity;
    // 정의
    public static int charactorNum = 5;
    string playerName;
    
    int value = 0;
	// Use this for initialization
	void Start () {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        turn.eulerAngles = new Vector3(0, value, 0);
        // 각을 초기화합니다.
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left");
            charactorNum++;
            if (charactorNum == 9)
                charactorNum = 5;
            value -= 90;
            // 각도를 90도 뺍니다.
            bTurnLeft = false;
            // 부울 변수를 취소합니다.
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right");
            charactorNum--;
            if (charactorNum == 4)
                charactorNum = 8;
            value += 90;
            // 각도를 90도 더합니다.
            bTurnRight = false;
            // 부울 변수를 취소합니다.
        }
		if(bTurnLeft)
        {
            Debug.Log("Left");
            charactorNum++;
            if (charactorNum == 9)
                charactorNum = 5;
            value -= 90;
            // 각도를 90도 뺍니다.
            bTurnLeft = false;
            // 부울 변수를 취소합니다.
        }
        if(bTurnRight)
        {
            Debug.Log("Right");
            charactorNum--;
            if (charactorNum == 4)
                charactorNum = 8;
            value += 90;
            // 각도를 90도 더합니다.
            bTurnRight = false;
            // 부울 변수를 취소합니다.
        }
        turn.eulerAngles = new Vector3(0, value, 0);
        // 각도를 잽니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * 5.0f);
        // 돌립니다.
	}

    public void turnLeft()
    {
        bTurnLeft = true;
        bTurnRight = false;
        // 다른 버튼을 누를때의 컨트롤
    }

    public void turnRight()
    {
        bTurnRight = true;
        bTurnLeft = false;
        // 다른 버튼을 누를때의 컨트롤
    }

    public void turnStage()
    {
        // 스테이지 전환을 위한 함수
        Debug.Log($"CharNum : {charactorNum}");
        StartCoroutine(sendMessage());
        // 로그인 정보를 JSON 형식으로 작성
       
    }
    public IEnumerator sendMessage()
    {
        Debug.Log($"유저 아이디엑스 턴 온 더 스테이지 : {PlayerPrefs.GetInt("Idx")}");
        string json = JsonUtility.ToJson(new HealingCharactorData { userIdx = PlayerPrefs.GetInt("Idx"), characterIdx = charactorNum});

        // 웹 요청을 생성하고, URL과 HTTP 메서드를 설정합니다.
        using (UnityWebRequest request = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/user/set-healing-character", "POST"))
        {
            // JSON 형식의 데이터를 전송하기 위한 헤더 설정
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            Debug.Log("요청 보내기 전");
            // 웹 요청을 보냅니다.
            yield return request.SendWebRequest();
            Debug.Log("요청 보낸 후");
            // 요청이 완료되면 결과를 처리합니다.
            if (request.result == UnityWebRequest.Result.Success)
            {
                // 결과값을 받아옵니다.
                // string result = request.downloadHandler.text;
                string jsonResponse = request.downloadHandler.text;
                JsonResponse response = JsonUtility.FromJson<JsonResponse>(jsonResponse);
                // 결과값을 받아옵니다.
                Debug.Log("Message: " + response.message);
                Debug.Log("Status: " + response.status);

                UserInfo userInfo = response.data.userInfo;
                // Debug.Log("Idx: " + userInfo.idx);
                PlayerPrefs.SetInt("Idx", userInfo.idx);
                PlayerPrefs.SetString("HealingCharacterNum", charactorNum.ToString());

                // PlayerPrefs.SetString("PlayerName",playerName);
                // Debug.Log("UserId: " + userInfo.userId);
                // Debug.Log("UserNickname: " + userInfo.userNickname);
                // Debug.Log("WorldCharacter: " + userInfo.worldCharacter);
                // Debug.Log("HealingCharacter: " + userInfo.healingCharacter);
                // 결과값에 따라 다음 작업을 수행합니다. 예를 들어, 게임 씬을 로드하거나 오류 메시지를 표시합니다.
                if (response.message == "Success")
                {
                    LoadGameScene();
                }
                else
                {
                    Debug.Log("캐릭터 생성에 실패했습니다.");
                }
            }
            else
            {
                Debug.Log("웹 요청에 실패했습니다: " + request.error);
            }
        }
    }
    private void LoadGameScene()
    {
        LoadingSceneController.LoadScene("HealingScene");
        // SceneManager.LoadScene("HealingScene");
    }
}
public class HealingCharactorData
{
    public int userIdx;
    public int characterIdx;
}