
using Photon.Pun;
using UnityEngine;
using TMPro;
public class MiniMap : MonoBehaviour
{
    public RectTransform minimap; // 미니맵의 RectTransform
    public RectTransform playerMarker; // 플레이어 마커의 RectTransform
    public Camera minimapCamera; // 미니맵을 보여주는 카메라
    private Transform player; // 플레이어의 Transform
    private PhotonView pv;
    public TMP_InputField ChatInput;

    private void Start()
    {
        minimap.gameObject.SetActive(false); // 미니맵 비활성화
        GameObject inputFieldObject = GameObject.Find("ChatBox"); // InputField GameObject의 이름을 지정합니다.
        if (inputFieldObject != null)
        {
            ChatInput = inputFieldObject.GetComponent<TMP_InputField>(); // InputField 컴포넌트를 가져옵니다.
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        PhotonView pv = playerTransform.GetComponent<PhotonView>();
        if (pv != null && pv.IsMine)
        {
            player = playerTransform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !ChatInput.isFocused)
        {
            ToggleMiniMap();
        }

        if (player != null)
        {
            // 플레이어의 월드 위치를 미니맵 상의 위치로 변환합니다.
            Vector3 playerPosition = minimapCamera.WorldToViewportPoint(player.position);
            playerMarker.anchorMin = playerPosition;
            playerMarker.anchorMax = playerPosition;
        }
    }

    private void ToggleMiniMap()
    {
        minimap.gameObject.SetActive(!minimap.gameObject.activeSelf); // 미니맵 토글
    }
}
