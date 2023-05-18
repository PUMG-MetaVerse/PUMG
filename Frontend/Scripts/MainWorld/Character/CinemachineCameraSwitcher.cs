using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    public GameObject player;
    private PhotonView photonView;
    private bool isFirstPersonView;

    void Start()
    {
        // Find the player object
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            thirdPersonCamera = GameObject.FindGameObjectWithTag("ThirdPersonCamera").GetComponent<CinemachineVirtualCamera>();
            firstPersonCamera = GameObject.FindGameObjectWithTag("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
            StartCoroutine(FindPlayerAndSetUpCameras());
            SetupCameras();
        }
    }

    private IEnumerator FindPlayerAndSetUpCameras()
    {
        while (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                PhotonView pv = p.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    Debug.Log($"Player.name : {pv.name}");
                    player = p;
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

        if (player != null)
        {
            Debug.Log("Player found: " + player.name);
            SetupCameras();
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.V))
        {
            SwitchCameraView();
        }
    }

    private void SwitchCameraView()
    {
        isFirstPersonView = !isFirstPersonView;

        if (isFirstPersonView)
        {
            StartCoroutine(SmoothCameraPriorityChange(firstPersonCamera, thirdPersonCamera, 0.1f));
        }
        else
        {
            StartCoroutine(SmoothCameraPriorityChange(thirdPersonCamera, firstPersonCamera, 0.1f));
        }
    }

    private void SetupCameras()
    {
        if (player != null)
        {
            Transform cameraRoot = player.transform.Find("CameraRoot");
            if (cameraRoot != null)
            {
                thirdPersonCamera.Follow = cameraRoot;
                thirdPersonCamera.LookAt = cameraRoot;
                firstPersonCamera.Follow = cameraRoot;
                firstPersonCamera.LookAt = cameraRoot;
                Debug.Log("hey!");
            }
            else
            {
                Debug.LogError("CameraRoot not found. Make sure the player has a child object named 'CM vcam1' with a child object named 'CameraRoot'.");
            }
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    private IEnumerator SmoothCameraPriorityChange(CinemachineVirtualCamera toCamera, CinemachineVirtualCamera fromCamera, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            toCamera.Priority = Mathf.RoundToInt(Mathf.Lerp(0, 100, t));
            fromCamera.Priority = Mathf.RoundToInt(Mathf.Lerp(100, 0, t));
            yield return null;
        }
        toCamera.Priority = 100;
        fromCamera.Priority = 0;
    }
}
