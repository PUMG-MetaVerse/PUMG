using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System.Text;
using UnityEngine.Networking;


public class BalloonPickup : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float fallSpeed = 0.5f;
    private GameObject[] balloonInstances = new GameObject[2];
    private bool canPickup = false;
    private Rigidbody playerRigidbody;
    // private Transform playerHands;
    private Transform playerLeftHand;
    private Transform playerRightHand;
    private string leftHandBone = "Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_L/Shoulder_L/Elbow_L/Wrist_L";
    private string rightHandBone = "Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_R/Shoulder_R/Elbow_R/Wrist_R";
    private string skeleton_leftHandBone = "PT_Hips/PT_Spine/PT_Spine2/PT_Spine3/PT_LeftShoulder/PT_LeftArm/PT_LeftForeArm/PT_LeftHand/PT_Left_Hand_Weapon_slot";
    private string skeleton_rightHandBone = "PT_Hips/PT_Spine/PT_Spine2/PT_Spine3/PT_RightShoulder/PT_RightArm/PT_RightForeArm/PT_RightHand/PT_Right_Hand_Weapon_slot";
    private string med_leftHandBone = "PT_NPC_Hips/PT_Spine/PT_Spine2/PT_Spine3/PT_LeftShoulder/PT_LeftArm/PT_LeftForeArm/PT_Left_Hand_Weapon_slot";
    private string med_rightHandBone = "PT_NPC_Hips/PT_Spine/PT_Spine2/PT_Spine3/PT_RightShoulder/PT_RightArm/PT_RightForeArm/PT_RightHand/PT_Right_Hand_Weapon_slot";
    private bool isFalling = false;
    private ThirdPersonController_Healing thirdPersonController;
    private float originalDrag;
    private float originalGravity;
    private Vector3 originalGlobalGravity;

    private void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            AttachBalloon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            thirdPersonController = other.GetComponent<ThirdPersonController_Healing>();
            playerRigidbody = other.GetComponent<Rigidbody>();

            string leftHands = leftHandBone;
            string rightHands = rightHandBone;

            if(PlayerPrefs.GetString("HealingCharacterNum") == "8") 
            {
                leftHands = skeleton_leftHandBone;
                rightHands = skeleton_rightHandBone;
            }
            else if(PlayerPrefs.GetString("HealingCharacterNum") == "6")
            {
                leftHands = med_leftHandBone;
                rightHands = med_rightHandBone;
            }

            playerLeftHand = other.transform.Find(leftHands);
            playerRightHand = other.transform.Find(rightHands);
            originalDrag = playerRigidbody.drag;
            Debug.Log("Player entered trigger area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }

    private void AttachBalloon()
    {
        if (balloonInstances[0] == null && balloonInstances[1] == null)
        {
            originalGlobalGravity = Physics.gravity; // Save the original gravity
            StartCoroutine(ChangePlayerStats());
            StartCoroutine(PostRequest());
        }
    }

    private IEnumerator PostRequest()
    {
        string json = JsonUtility.ToJson(
            new HealingInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                titleIdx = 12,
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

    private IEnumerator ChangePlayerStats()
    {
    
        Debug.Log("Giving player balloons and changing gravity.");

        balloonInstances[0] = Instantiate(balloonPrefab, playerLeftHand.position, Quaternion.identity);
        balloonInstances[0].transform.SetParent(playerLeftHand);
        balloonInstances[0].transform.localPosition = Vector3.zero;

        balloonInstances[1] = Instantiate(balloonPrefab, playerRightHand.position, Quaternion.identity);
        balloonInstances[1].transform.SetParent(playerRightHand);
        balloonInstances[1].transform.localPosition = Vector3.zero;

        float originalGravity = thirdPersonController.Gravity;
        thirdPersonController.Gravity = -0.01f;
        playerRigidbody.drag = 15f;

        Physics.gravity *= 0.1f;

        isFalling = true;

        while (isFalling)
        {
            Vector3 motion = new Vector3(0, fallSpeed , 0) * Time.deltaTime;
            playerRigidbody.AddForce(motion, ForceMode.VelocityChange);

            if (balloonInstances[0] != null)
            {
                balloonInstances[0].transform.position = playerLeftHand.position;
            }
            if (balloonInstances[1] != null)
            {
                balloonInstances[1].transform.position = playerRightHand.position;
            }

            if (IsPlayerTouchingGroundOrWater())
            {
                DetachBalloon();
                break;
            }

            if (!isFalling)
            {
                ResetPlayerStats();
                yield break;
            }

            yield return null;
        }

        thirdPersonController.Gravity = originalGravity;
        if (playerRigidbody != null)
        {
            playerRigidbody.drag = originalDrag;
            playerRigidbody.useGravity = true;
        }
                    Debug.Log("Player gravity and drag returned to original values.");

        if (balloonInstances[0] != null)
        {
            Destroy(balloonInstances[0]);
            balloonInstances[0] = null;
        }
        if (balloonInstances[1] != null)
        {
            Destroy(balloonInstances[1]);
            balloonInstances[1] = null;
        }

    }

    private void DetachBalloon()
    {
        if (balloonInstances[0] != null)
        {
            Destroy(balloonInstances[0]);
            balloonInstances[0] = null;
        }
        if (balloonInstances[1] != null)
        {
            Destroy(balloonInstances[1]);
            balloonInstances[1] = null;
        }
        isFalling = false;
        ResetPlayerStats();
    }

    private void ResetPlayerStats()
    {
        thirdPersonController.Gravity = originalGravity;
        if (playerRigidbody != null)
        {
            playerRigidbody.drag = originalDrag;
            playerRigidbody.useGravity = true;
        }
        Physics.gravity = originalGlobalGravity; // Restore the original gravity
        Debug.Log("Player gravity and drag returned to original values.");
    }
    
    private bool IsPlayerTouchingGroundOrWater()
    {
        float distanceToGround = 0.5f; 
        int groundAndWaterLayerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Water"));

        RaycastHit hit;
        if (Physics.Raycast(playerRigidbody.transform.position, Vector3.down, out hit, distanceToGround, groundAndWaterLayerMask))
        {
            return true;
        }
        return false;
    }
    [System.Serializable]
    public class HealingInfo
    {
        public int userIdx;
        public int titleIdx;
    }
}