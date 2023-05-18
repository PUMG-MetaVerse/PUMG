using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
public class TitleButton : MonoBehaviour
{
    public TMP_Text titleText; // 칭호 텍스트를 표시하는 Text 컴포넌트
    public TMP_Text descriptionText; // 칭호 설명을 표시하는 Text 컴포넌트
    public TitleUI titleUI; // TitleUI 인스턴스에 대한 참조를 저장합니다.

    private string title; // 이 버튼이 대표하는 칭호
    private string description;
    private int titleIdx;

    private void Awake()
    {
        // 자식 오브젝트인 TitleDescriptionView를 찾은 후 그 아래에 있는 TitleDescriptionText 컴포넌트를 가져옵니다.
        descriptionText = GameObject
            .Find("TitleDescriptionText")
            .GetComponent<TMP_Text>();
    }

    public void SetData(TitleUI.TitleListData titleListData)
    {
        // 이 메서드는 TitleUI 스크립트에서 호출되어, 이 버튼이 표시해야 할 칭호 데이터를 설정합니다.
        titleIdx = titleListData.idx;
        title = titleListData.title;
        titleText.text = title;
        description = titleListData.description;
    }

    public void OnTitleButtonClicked()
    {
        // 이 메서드는 버튼이 클릭될 때 호출됩니다.
        // 예를 들어, TitleUI에 현재 선택된 칭호를 알려주는 코드를 여기에 추가할 수 있습니다.
        Debug.Log($"칭호 {title} 버튼이 클릭되었습니다.");
        // descriptionText.text = description;
        titleUI.SetImage(title);
        //TitleUI.Instance.SelectTitle(title);
        descriptionText.text = description;
    }
    public void OnSelectButtonClicked()
    {
        // 이 메서드는 버튼이 클릭될 때 호출됩니다.
        // 예를 들어, TitleUI에 현재 선택된 칭호를 알려주는 코드를 여기에 추가할 수 있습니다.
        Debug.Log($"칭호 {title} 버튼이 장착되었습니다.");
        //TitleUI.Instance.SelectTitle(title);
        descriptionText.text = description;
        titleUI.SetImage(title);
        titleUI.EquipTitle(title);
        StartCoroutine(TitleSetRequest());

    }
    private IEnumerator TitleSetRequest()
    {
        string json = JsonUtility.ToJson(new TitleSetData { userIdx = PlayerPrefs.GetInt("Idx"), titleIdx = titleIdx});
        using (UnityWebRequest webRequest = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/title/set/"+PlayerPrefs.GetInt("Idx"), "PUT"))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // 요청 보내기
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {

            } else {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }
    [System.Serializable]
public class TitleSetData
{
    public int userIdx;
    public int titleIdx;
}
}
