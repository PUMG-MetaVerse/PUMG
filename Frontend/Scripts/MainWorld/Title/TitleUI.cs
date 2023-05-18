using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class TitleUI : MonoBehaviourPun
{
    public GameObject titleUI; // 칭호 UI 패널
    public GameObject scrollViewContent; // ScrollView의 Content 객체
    public GameObject titleButtonPrefab; // 칭호 버튼의 프리팹
    public RawImage titleImage; // 선택한 칭호의 이미지를 표시하는 Image 컴포넌트
    public TMP_Text titleDescription; // 칭호 설명을 표시하는 Text 컴포넌트
    public Button equipButton; // 칭호를 착용하는 버튼
    private string selectedTitle; // 현재 선택한 칭호
    public TMP_InputField ChatInput;
    public TMP_InputField PartyMemberInput;

    void Start()
    {
        GameObject inputFieldObject = GameObject.Find("ChatBox"); // InputField GameObject의 이름을 지정합니다.
        if (inputFieldObject != null)
        {
            ChatInput = inputFieldObject.GetComponent<TMP_InputField>(); // InputField 컴포넌트를 가져옵니다.
        }
        GameObject partyInputObject = GameObject.Find("PartyInput");
        if (partyInputObject != null)
        {
            PartyMemberInput = partyInputObject.GetComponent<TMP_InputField>(); // InputField 컴포넌트를 가져옵니다.
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !ChatInput.isFocused && (PartyMemberInput == null || !PartyMemberInput.isFocused))
        {
            if (titleUI.activeSelf)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                OFFSetTextOrigin();
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(getTitleData());
                OnUIPanelSetImage();
            }
            // E 키를 누를 때마다 UI를 토글합니다.
            titleUI.SetActive(!titleUI.activeSelf);
        }
    }

    // 종료 버튼 누르면 나가지기
    public void ExitButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        titleUI.SetActive(!titleUI.activeSelf);
        OFFSetTextOrigin();
    }

    public void OFFSetTextOrigin()
    {
        titleDescription.text = "칭호설명";
    }

    // 처음 켰을 때 본인이 착용하고 있는 이미지 띄우기
    public void OnUIPanelSetImage()
    {
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;
        string title = (string)player.CustomProperties["Title"];
        Texture titleTexture = Resources.Load<Texture>(title);
        titleImage.texture = titleTexture;
    }

    public void SetImage(string titleName)
    {
        Texture titleTexture = Resources.Load<Texture>(titleName);
        titleImage.texture = titleTexture;
    }

    public void OnEquipButtonClicked()
    {
        // 착용 버튼이 클릭될 때 호출되는 메서드입니다.
        // 현재 착용중인 칭호를 선택한 칭호로 변경합니다.
        EquipTitle(selectedTitle);
    }
public void EquipTitle(string title)
{
    // 현재 착용중인 칭호를 선택한 칭호로 변경하는 메서드입니다.
    PhotonNetwork.LocalPlayer.CustomProperties["Title"] = title;
    PlayerPrefs.SetString("PlayerTitle", title);
    Sprite titleImage = Resources.Load<Sprite>(title);

    PlayerSetup[] allPlayerSetups = FindObjectsOfType<PlayerSetup>();
    foreach (PlayerSetup playerSetup in allPlayerSetups)
    {
        if (playerSetup.photonView.IsMine)
        {
            playerSetup.spriteRenderer.sprite = titleImage;
            playerSetup.photonView.RPC("SetTitle", RpcTarget.Others, title);
            break;
        }
    }
}
    // public void EquipTitle(string title)
    // {
    //     // 현재 착용중인 칭호를 선택한 칭호로 변경하는 메서드입니다.
    //     // 이 메서드를 적절히 구현해야 합니다.
    //     // 예를 들어, PhotonNetwork의 CustomProperties를 사용하여
    //     // 착용중인 칭호를 저장할 수 있습니다.
    //     PhotonNetwork.LocalPlayer.CustomProperties["Title"] = title;
    //     PlayerSetup myPlayerSetup = null;
    // Sprite titleImage = Resources.Load<Sprite>(title);

    //     PlayerSetup[] allPlayerSetups = FindObjectsOfType<PlayerSetup>();
    //     foreach (PlayerSetup playerSetup in allPlayerSetups)
    //     {
    //         if (playerSetup.GetComponent<PhotonView>().IsMine)
    //         {
    //             myPlayerSetup = playerSetup;
    //             break;
    //         }
    //     }

    //     // Now you can use myPlayerSetup
    //     if (myPlayerSetup != null)
    //     {
    //         PhotonView photonView = myPlayerSetup.GetComponent<PhotonView>();
    //         myPlayerSetup.spriteRenderer.sprite = titleImage;
    //         photonView.RPC("SetTitle", RpcTarget.Others, title);

    //     }
    // }

    //    [PunRPC]
    // public void SetTitle(string newTitle, PlayerSetup myPlayerSetup)
    // {
    //     // 칭호를 변경하는 로직을 여기에 작성합니다.
    //     // 예를 들어, 이미지를 로드하거나 텍스트를 변경하는 등의 작업을 수행할 수 있습니다.

    //     string title = newTitle;
    //     Sprite titleImage = Resources.Load<Sprite>(title);
    //     myPlayerSetup.spriteRenderer.sprite = titleImage;
    // }

    private IEnumerator getTitleData()
    {
        // 먼저 Content 내부의 모든 칭호 버튼을 제거합니다.
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        string url = "http://k8b108.p.ssafy.io:6999/api/v1/title/list/" + PlayerPrefs.GetInt("Idx");
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
            // Debug.Log(response.data.TitleListInfo.);
            Debug.Log(response.data.titleInfo.Count);
            for (int i = 0; i < response.data.titleInfo.Count; i++)
            {
                TitleListData titleListData = response.data.titleInfo[i];

                // 칭호 버튼 프리팹을 인스턴스화합니다.
                GameObject titleButton = Instantiate(titleButtonPrefab);

                // titleButton의 부모를 scrollViewContent로 설정합니다.
                titleButton.transform.SetParent(scrollViewContent.transform, false);

                // titleButton에 데이터를 설정합니다.
                // 예를 들어, titleButton에 TitleButton 스크립트가 붙어있다고 가정하면, 이 스크립트에 접근하여 데이터를 설정할 수 있습니다.
                TitleButton titleButtonScript = titleButton.GetComponent<TitleButton>();
                titleButtonScript.titleUI = this; // TitleUI 참조를 설정합니다.
                titleButtonScript.SetData(titleListData);
            }
        }
        else
        {
            Debug.LogError($"GET 요청 실패: {request.error}");
        }
    }

    [System.Serializable]
    public class TitleListData
    {
        public int idx;
        public string title;
        public string description;
        public string getTime;
    }

    [System.Serializable]
    public class DataWrapper
    {
        public List<TitleListData> titleInfo;
    }

    [System.Serializable]
    public class JsonResponse
    {
        public string message;
        public int status;
        public DataWrapper data;
    }
}
