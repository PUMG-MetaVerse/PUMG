using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // 이 부분 추가


public class MapController : MonoBehaviour
{
    private RawImage mapImage;
    private bool mapVisible = false;
    private GameObject player;
    private RectTransform playerIcon;
    public Sprite redCircleSprite; // 빨간색 동그라미 이미지를 대표하는 스프라이트
    private Vector3 playerStartPos; // 플레이어 초기 위치
    private GameObject spawnPointGroup; // 스폰 포인트 그룹
    public Sprite spawnPointSprite; // 스폰 포인트를 표시하는 스프라이트
    private List<RectTransform> spawnPointIcons; // 스폰 포인트 아이콘 목록

    void Start()
    {
        // 맵 이미지 오브젝트 찾기
        GameObject mapImageObject = GameObject.Find("MapImage");

        // 맵 이미지 오브젝트가 존재하는 경우 컴포넌트 가져오기 및 비활성화
        if (mapImageObject != null)
        {
            mapImage = mapImageObject.GetComponent<RawImage>();

            // 맵 이미지가 null인 경우 예외 메시지 출력
            if (mapImage == null)
            {
                Debug.LogError("MapImage 컴포넌트가 존재하지 않습니다.");
            }
            else
            {
                mapImage.enabled = false;
            }
        }
        else
        {
            Debug.LogError("MapImage 오브젝트가 존재하지 않습니다.");
        }

        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        // player가 null일 경우 예외 메시지 출력
        if (player == null)
        {
            Debug.LogError("플레이어 오브젝트를 찾을 수 없습니다.");
        }

        // 스폰 포인트 그룹 찾기
        spawnPointGroup = GameObject.Find("SpawnPointGroup");

        // 플레이어가 존재하는 경우에만 아이콘 생성 및 초기화
        if (player != null)
        {
            // 플레이어의 초기 위치를 설정
            playerStartPos = new Vector3(-620, -160, 0);

            // 플레이어 아이콘 생성 및 설정
            GameObject playerIconObject = new GameObject("PlayerIcon");
            playerIcon = playerIconObject.AddComponent<RectTransform>();
            playerIconObject.transform.SetParent(mapImageObject.transform);

            // Image 컴포넌트 추가 및 설정
            Image playerIconImage = playerIconObject.AddComponent<Image>();
            playerIconImage.sprite = redCircleSprite; 

            // 플레이어 아이콘 크기 조절
            float iconSize = 25f; // 아이콘 크기 조정을 위한 변수 설정
            playerIcon.sizeDelta = new Vector2(iconSize, iconSize);

            // 플레이어 아이콘 초기 위치 설정
            Vector3 playerPosition = player.transform.position;
            float normalizedX = Mathf.InverseLerp(-500f, 500f, playerPosition.x - playerStartPos.x);
            float normalizedY = Mathf.InverseLerp(-500f, 500f, playerPosition.z - playerStartPos.z);
            float pixelX = normalizedX * mapImage.rectTransform.rect.width;
            float pixelY = normalizedY * mapImage.rectTransform.rect.height;
            Vector2 playerIconPosition = new Vector2(pixelX, pixelY);
            playerIcon.anchoredPosition = playerIconPosition;

            // 플레이어 아이콘 비활성화
            playerIconObject.SetActive(false);
        }

            // 스폰 포인트 아이콘을 위한 리스트 생성
        spawnPointIcons = new List<RectTransform>();

        // 스폰 포인트 그룹에서 모든 스폰 포인트를 찾고 아이콘을 생성
        if (spawnPointGroup != null)
        {
            foreach (Transform spawnPoint in spawnPointGroup.transform)
            {
                // 스폰 포인트 아이콘 생성 및 설정
                GameObject spawnPointIconObject = new GameObject("SpawnPointIcon");
                RectTransform spawnPointIcon = spawnPointIconObject.AddComponent<RectTransform>();
                spawnPointIconObject.transform.SetParent(mapImageObject.transform);

                // Image 컴포넌트 추가 및 설정
                Image spawnPointIconImage = spawnPointIconObject.AddComponent<Image>();
                spawnPointIconImage.sprite = redCircleSprite; // 아이콘에 빨간색 동그라미 이미지 설정

                // 스폰 포인트 아이콘 크기 조절
                float iconSize = 15f; // 아이콘 크기 조정을 위한 변수 설정
                spawnPointIcon.sizeDelta = new Vector2(iconSize, iconSize);

                // 스폰 포인트 아이콘 위치 설정
                Vector3 spawnPointPosition = spawnPoint.position;
                float normalizedX = Mathf.InverseLerp(-500f, 500f, spawnPointPosition.x - playerStartPos.x);
                float normalizedY = Mathf.InverseLerp(-500f, 500f, spawnPointPosition.z - playerStartPos.z);
                float pixelX = normalizedX * mapImage.rectTransform.rect.width;
                float pixelY = normalizedY * mapImage.rectTransform.rect.height;
                Vector2 spawnPointIconPosition = new Vector2(pixelX, pixelY);
                spawnPointIcon.anchoredPosition = spawnPointIconPosition;

                // 스폰 포인트 아이콘 비활성화
                spawnPointIconObject.SetActive(false);

                // 생성된 스폰 포인트 아이콘을 리스트에 추가
                spawnPointIcons.Add(spawnPointIcon);
            }
        }
        else
        {
            Debug.LogError("SpawnPointGroup 오브젝트를 찾을 수 없습니다.");
        }

        // 씬이 변경될 때마다 호출될 콜백 설정
        SceneManager.sceneLoaded += OnSceneLoaded;
    }



    void Update()
    {
        // 'M' 키가 눌렸을 때 맵 표시를 토글
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }

        // 플레이어의 위치를 맵에 업데이트
        if (player != null && playerIcon != null && mapVisible)
        {
            UpdateIconPosition(player, playerIcon);
        }

        // 모든 스폰 포인트의 위치를 맵에 업데이트
        if (spawnPointGroup != null && mapVisible)
        {
            for (int i = 0; i < spawnPointGroup.transform.childCount; i++)
            {
                Transform spawnPoint = spawnPointGroup.transform.GetChild(i);
                RectTransform spawnPointIcon = spawnPointIcons[i];
                UpdateIconPosition(spawnPoint.gameObject, spawnPointIcon);
            }
        }
    }

    void UpdateIconPosition(GameObject gameObj, RectTransform icon)
    {
        Vector3 position = gameObj.transform.position;
        float normalizedX = Mathf.InverseLerp(-500f, 500f, position.x - playerStartPos.x);
        float normalizedY = Mathf.InverseLerp(-500f, 500f, position.z - playerStartPos.z);
        float pixelX = normalizedX * mapImage.rectTransform.rect.width;
        float pixelY = normalizedY * mapImage.rectTransform.rect.height;
        Vector2 iconPosition = new Vector2(pixelX, pixelY);
        icon.anchoredPosition = iconPosition;
    }

    void ToggleMap()
    {
        // 맵 이미지, 플레이어 아이콘, 스폰 포인트 아이콘의 활성화 상태를 토글
        mapVisible = !mapVisible;
        mapImage.enabled = mapVisible;
        playerIcon.gameObject.SetActive(mapVisible);
        foreach (RectTransform spawnPointIcon in spawnPointIcons)
        {
            spawnPointIcon.gameObject.SetActive(mapVisible);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬이 로드되면, 플레이어의 위치를 직접 지정한 위치로 설정
        playerStartPos = new Vector3(620, -160, 0);

        if (player != null)
        {
            player.transform.position = playerStartPos;
        }
        else
        {
            Debug.LogWarning("OnSceneLoaded: 플레이어 오브젝트를 찾을 수 없습니다.");
        }
    }
}