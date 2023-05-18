using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HealingDayNightCycle : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform sunTransform;
    public float dayDuration;

    [Header("Groups")]
    public Transform dayGroup;
    public Transform nightGroup;

    [Header("Sun rotation thresholds")]
    public float dayToNightAngle = 175f;
    public float nightToDayAngle = 5f;

    [Header("Objects Activation Settings")]
    public int maxObjectsPerFrame = 50;
    public float waitBetweenFrames = 0.01f;

    private bool isDay = true;
    private bool isDaySync = true;
    private float sunAngle;  // The true angle of the sun, which can be greater than 360

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.T))
    //     {
    //         if (PhotonNetwork.IsMasterClient)
    //         {
    //             StopAllCoroutines();  // 현재 실행 중인 모든 코루틴을 중단
    //             isDay = !isDay;  // 낮과 밤을 바꿈
    //             StartCoroutine(SwitchGroups());
    //         }
    //     }
    // }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to Room!");
        if (PhotonNetwork.IsMasterClient)
        {
            sunAngle = sunTransform.eulerAngles.x;
            StartCoroutine(SetActiveGroup(dayGroup, true));
            StartCoroutine(SetActiveGroup(nightGroup, false));
            StartCoroutine(ChangeDayNightCycle());
        } 
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(sunTransform.eulerAngles);
            stream.SendNext(isDay);
            stream.SendNext(sunAngle);  // Add this line to sync sunAngle
        }
        else
        {
            // Network player, receive data
            sunTransform.eulerAngles = (Vector3)stream.ReceiveNext();
            isDaySync = (bool)stream.ReceiveNext();
            sunAngle = (float)stream.ReceiveNext();  // Add this line to receive sunAngle

            if(isDay != isDaySync) // 동기화가 필요하다면
            {
                isDay = isDaySync;
                StartCoroutine(SwitchGroups());
            }
        }
    }

    private IEnumerator ChangeDayNightCycle()
    {
        float rotationPerSecond = 360f / dayDuration;

        while (true)
        {
            sunTransform.Rotate(Vector3.right * Time.deltaTime * rotationPerSecond);
            sunAngle += Time.deltaTime * rotationPerSecond;
            if (sunAngle >= 360) sunAngle -= 360;
            else if (sunAngle < 0) sunAngle += 360;

            if (isDay && sunAngle >= dayToNightAngle && (sunAngle - Time.deltaTime * rotationPerSecond) % 360 < dayToNightAngle)
            {
                isDay = false;
                StartCoroutine(SwitchGroups());
            }
            else if (!isDay && sunAngle <= nightToDayAngle && (sunAngle - Time.deltaTime * rotationPerSecond + 360) % 360 > nightToDayAngle)
            {
                isDay = true;
                StartCoroutine(SwitchGroups());
            }

            yield return null;
        }
    }

    private IEnumerator SetActiveGroup(Transform group, bool active)
    {
        int objectsProcessed = 0;

        for (int i = 0; i < group.childCount; i++)
        {
            GameObject child = group.GetChild(i).gameObject;
            if (child.activeSelf != active)
            {
                child.SetActive(active);
                objectsProcessed++;

                if (objectsProcessed >= maxObjectsPerFrame)
                {
                    objectsProcessed = 0;
                    yield return new WaitForSeconds(waitBetweenFrames);
                }
            }
        }
    }

    private IEnumerator SwitchGroups()
    {
        if (isDay)
        {
            // sunTransform.eulerAngles = new Vector3(45, 0, 0);  // 낮이면 45도로 설정
            yield return StartCoroutine(SetActiveGroup(dayGroup, true));
            yield return StartCoroutine(SetActiveGroup(nightGroup, false));
        }
        else
        {
            // sunTransform.eulerAngles = new Vector3(345, 0, 0);  // 밤이면 345도로 설정
            yield return StartCoroutine(SetActiveGroup(nightGroup, true));
            yield return StartCoroutine(SetActiveGroup(dayGroup, false));
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 마스터 클라이언트가 바뀌었을 때의 로직 작성
        if (PhotonNetwork.IsMasterClient)
        {
            sunAngle = sunTransform.eulerAngles.x;
            StartCoroutine(SetActiveGroup(dayGroup, isDay));
            StartCoroutine(SetActiveGroup(nightGroup, !isDay));
            StartCoroutine(ChangeDayNightCycle());
        }
    }
}