using UnityEngine;
using StarterAssets;
using Photon.Pun;
using System.Collections;
using System.Text;
using UnityEngine.Networking;


public class HealingInteraction : MonoBehaviourPunCallbacks
{
    [Tooltip("Common")]
    private Animator animator;
    private ThirdPersonController_Healing thirdPersonController;
    private CharacterController characterController;

    [Tooltip("Water")]
    public LayerMask waterLayer;
    private bool isSwimming = false;
    private bool wasTreadingWater = false;

    [Tooltip("SunBed")]
    public LayerMask sunbedLayer; // 썬베드 레이어
    private Sunbed currentSunbed;
    private bool isLayDown = false;
    private Vector3 originalPosition; // 썬베드에 누워있기 전의 원래 위치
    private Quaternion originalRotation; // 썬베드에 누워있기 전의 원래 회전
    public int currentSunbedID = -1; // 현재 누운 썬베드의 PhotonView ID

    [Tooltip("Fishing")]
    public LayerMask fishingLayer; // 썬베드 레이어
    private bool isFishing = false;
    private FishingItem currrentFisingItem;
    public int currentFinshingID = -1; // 현재 누운 썬베드의 PhotonView ID
    // PlayerOutlineControl 참조를 위한 필드 추가
    private PlayerOutlineControl playerOutlineControl;

    void Start()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController_Healing>();
        characterController = GetComponent<CharacterController>();
        // 성훈이가 추가한 부분
        MiniMap miniMap = FindObjectOfType<MiniMap>();
        if (miniMap != null)
        {
            miniMap.SetPlayer(transform);
        }
        playerOutlineControl = GetComponent<PlayerOutlineControl>(); // dkdntfkdls
        // 성훈이가 추가한 부분
    }

    void Update()
    {
        if (!photonView.IsMine) return; // 추가: 로컬 플레이어만 입력을 처리하도록 변경

        bool isTreadingWater = isSwimming && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0;
        if (isTreadingWater != wasTreadingWater)
        {
            animator.SetBool("IsTreadingWater", isTreadingWater);
            wasTreadingWater = isTreadingWater;
        }

        if (Input.GetMouseButtonDown(0) && animator.GetBool("Grounded"))
        {
            animator.SetTrigger("Jab");
        }

        // F 키가 눌렸을 때 처리
        if (Input.GetKeyDown(KeyCode.F))
        {   // 썬 베드 상호작용
            if (currentSunbed != null)
            {
                if (!currentSunbed.IsOccupied)
                {
                    OnLayDownEnter();
                }
                else if(isLayDown)
                {
                    OnLayDownExit();
                }
            }
            // 낚시 상호작용
            if (currrentFisingItem != null)
            {
                if (!currrentFisingItem.IsSitting)
                {
                    OnFishingEnter();
                }
                else if(isFishing)
                {
                    OnFishingExit();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && other is BoxCollider)
        {
            OnSwimmingEnter();
        }

        if (((1 << other.gameObject.layer) & sunbedLayer) != 0)
        {
            currentSunbed = other.GetComponent<Sunbed>();
        }

        if (((1 << other.gameObject.layer) & fishingLayer) != 0)
        {
            currrentFisingItem = other.GetComponent<FishingItem>();
            currentFinshingID = currrentFisingItem.finshingID;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && other is BoxCollider)
        {
            OnSwimmingExit();
        }

        if (((1 << other.gameObject.layer) & sunbedLayer) != 0)
        {
            currentSunbed = null;
        }

        if (((1 << other.gameObject.layer) & fishingLayer) != 0)
        {
            currrentFisingItem = null;
            currentFinshingID = -1;
        }
    }

    void OnSwimmingEnter()
    {
        isSwimming = true;
        thirdPersonController.IsSwimming = true;
        animator.SetBool("IsSwimming", true);
        StartCoroutine(PostRequest());
    }

    void OnSwimmingExit()
    {
        isSwimming = false;
        thirdPersonController.IsSwimming = false;
        animator.SetBool("IsSwimming", false);
        animator.SetBool("IsTreadingWater", false);
    }

    private void OnLayDownEnter()
    {
        currentSunbed.photonView.RPC("Occupy", RpcTarget.All, photonView.ViewID);
        thirdPersonController.isSitting = true;
        characterController.enabled = false;
        originalRotation = transform.rotation;
        originalPosition = transform.position;
        isLayDown = true;

        animator.SetBool("IsLayDown", true);
        // sunbed의 LoungerPos의 위치로 이동

        // LoungerPos 오브젝트를 찾습니다.
        Transform loungerPos = currentSunbed.transform.Find("LoungerPos");
        if (loungerPos != null)
        {
            transform.position = loungerPos.position;
            transform.rotation = loungerPos.rotation;
        }
        // 아웃라인과 텍스트를 숨깁니다.
        playerOutlineControl.ClearOutline();
        
    }
    
    private void OnLayDownExit()
    {
        currentSunbed.photonView.RPC("Vacate", RpcTarget.All);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isLayDown = false;
        animator.SetBool("IsLayDown", false);
        // 원래 위치로 이동

        thirdPersonController.isSitting = false;
        characterController.enabled = true;

        // 아웃라인과 텍스트를 다시 표시합니다.
        playerOutlineControl.ShowActionText(transform);
    }

    private void OnFishingEnter()
    {

        currrentFisingItem.photonView.RPC("PickUp", RpcTarget.All, photonView.ViewID);
        
        
        // playerHands = transform.Find(rightHandBone);
        thirdPersonController.isSitting = true;
        characterController.enabled = false;
        
        isFishing = true;

        animator.SetTrigger("IsSitting");

        Transform fixedPos = currrentFisingItem.transform.Find("FixedPos");
        if (fixedPos != null)
        {
            transform.position = fixedPos.position;
            transform.rotation = fixedPos.rotation;
        }

        animator.SetBool("IsThrowing", true);
        // StartCoroutine(ThrowFishingPole());
        // 아웃라인과 텍스트를 숨깁니다.
        playerOutlineControl.ClearOutline();
    }
    
    private void OnFishingExit()
    {
        currrentFisingItem.photonView.RPC("Drop", RpcTarget.All);

        isFishing = false;
        animator.SetBool("IsThrowing", false);

        thirdPersonController.isSitting = false;
        characterController.enabled = true;

        animator.SetTrigger("IsStanding");

        currrentFisingItem = null;
        // 아웃라인과 텍스트를 다시 표시합니다.
        playerOutlineControl.ShowActionText(transform);
    }

    // private IEnumerator ThrowFishingPole()
    // {
    //     yield return new WaitForSeconds(1f); // Adjust the delay if needed
    //     bool isThrowing = animator.GetBool("IsThrowing");

    //     if (!isThrowing)
    //     {
    //         animator.SetBool("IsThrowing", true);
    //         yield return new WaitForSeconds(10000f);
    //         animator.SetBool("IsThrowing", false);
    //     }
    // }

    public override void OnDisable()
    {
        base.OnDisable();

        if(isLayDown)
        {
            PhotonView pv = PhotonView.Find(currentSunbedID);
            pv.RPC("Vacate", RpcTarget.All);
        }

        if(isFishing)
        {
            PhotonView pv = PhotonView.Find(currentFinshingID);
            pv.RPC("Vacate", RpcTarget.All);
            currentFinshingID = -1;
        }
    }

    public override void OnLeftRoom()
    {
        if (isLayDown)
        {
            currentSunbed.photonView.RPC("Vacate", RpcTarget.All);
        }

        if (isFishing)
        {
            currrentFisingItem.photonView.RPC("Vacate", RpcTarget.All);
        }
    }

   private IEnumerator PostRequest()
    {
        string json = JsonUtility.ToJson(
            new HealingInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                titleIdx = 10,
            }
        );

        using (UnityWebRequest webRequest = new UnityWebRequest("http://k8b108.p.ssafy.io:6999/api/v1/title/earn", "POST"))
        {    
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class HealingInfo
    {
        public int userIdx;
        public int titleIdx;
    }
}