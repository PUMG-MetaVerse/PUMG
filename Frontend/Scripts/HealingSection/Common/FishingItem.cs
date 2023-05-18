using Photon.Pun;
using UnityEngine;

public class FishingItem : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    public bool isSitting = false;

    [SerializeField]
    private int currentPlayerViewID = 0; // 현재 소유자의 PhotonView ID를 저장하는 변수

    public bool IsSitting
    {
        get { return isSitting; }
    }

    public int GetCurrentPlayerViewID
    {
        get { return currentPlayerViewID; }
    }
    public int finshingID;
    private Transform fishingPole;
    private string rightHandBone = "Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_L/Shoulder_L/Elbow_L/Wrist_L";
    private string skeleton_rightHandBone = "PT_Hips/PT_Spine/PT_Spine2/PT_Spine3/PT_LeftShoulder/PT_LeftArm/PT_LeftForeArm/PT_LeftHand/PT_Left_Hand_Weapon_slot";
    private string med_rightHandBone = "PT_NPC_Hips/PT_Spine/PT_Spine2/PT_Spine3/PT_LeftShoulder/PT_LeftArm/PT_LeftForeArm/PT_Left_Hand_Weapon_slot";
    private Vector3 fishingOriginalPosition;
    private Quaternion fishingOriginalRotation;

    private void Awake()
    {
        finshingID = photonView.ViewID;
    }

    private void Update()
    {
        // 현재 소유자의 PhotonView ID가 존재하는지 확인합니다.
        if (currentPlayerViewID != 0 && PhotonNetwork.GetPhotonView(currentPlayerViewID) == null)
        {
            // 소유자를 찾을 수 없으므로 Occupy()를 실행하고 currentOwnerViewID를 업데이트합니다.
            photonView.RPC("Vacate", RpcTarget.All);
            currentPlayerViewID = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isSitting);
            stream.SendNext(currentPlayerViewID);
        }
        else
        {
            isSitting = (bool)stream.ReceiveNext();
            currentPlayerViewID = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Occupy(int playerId)
    {
        isSitting = true;
        currentPlayerViewID = playerId;
    }

    [PunRPC]
    public void Vacate()
    {
        isSitting = false;
        currentPlayerViewID = 0;
    }
    private void Start()
    {
        fishingPole = transform.Find("Fishing-rod");
        if (fishingPole == null)
        {
            Debug.LogError("Fishing-pole not found as a child of the chair object.");
        }
    }

    [PunRPC]
    public void PickUp(int playerId)
    {
        isSitting = true;
        // 낚시대 객체를 쥔 플레이어의 ID를 저장합니다.
        currentPlayerViewID = playerId;

        Transform fishingRod = transform.Find("Fishing-rod");
        fishingOriginalPosition = fishingRod.position;
        fishingOriginalRotation = fishingRod.rotation;

        string hands = rightHandBone;

        // 낚시대 객체를 쥔 플레이어의 손에 낚시대 객체를 부착합니다.
        if(PlayerPrefs.GetString("HealingCharacterNum") == "8") 
        {
            hands = skeleton_rightHandBone;
        }
        else if(PlayerPrefs.GetString("HealingCharacterNum") == "6")
        {
            hands = med_rightHandBone;
        }
        Transform playerHands = PhotonView.Find(playerId).transform.Find(hands);
        if (playerHands != null)
        {
            fishingPole.SetParent(playerHands);
            fishingPole.localPosition = new Vector3(0.162f, -0.058f, 0.069f);
            fishingPole.localRotation = Quaternion.Euler(31.556f, -107.355f, -178.365f);
        }
    }

    [PunRPC]
    public void Drop()
    {
        isSitting = false;
        // 낚시대 객체를 쥔 플레이어의 손에서 낚시대 객체를 분리합니다.
        Transform fishingPoleInHand = fishingPole.parent.Find("Fishing-rod");
        if (fishingPoleInHand != null)
        {
            fishingPoleInHand.SetParent(transform);
            fishingPoleInHand.position = fishingOriginalPosition;
            fishingPoleInHand.rotation = fishingOriginalRotation;
        }

        // 낚시대 객체를 쥔 플레이어의 ID를 초기화합니다.
        currentPlayerViewID = 0;
    }
}