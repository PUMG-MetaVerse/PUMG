using UnityEngine;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.Networking;

public class DanceZone : MonoBehaviour
{
    private Animator playerAnimator;
    private bool playerInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogError("Player Animator component is not found.");
            }
            Debug.Log("Player entered the dance zone.");
        }
    }

    private IEnumerator PostRequest()
    {
        string json = JsonUtility.ToJson(
            new HealingInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                titleIdx = 11,
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


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            playerAnimator = null;
        }
    }

    void Update()
    {
        if (playerInZone)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerAnimator.SetTrigger("Dance1");
                Debug.Log("Player pressed key 1 to dance.");
                StartCoroutine(PostRequest());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerAnimator.SetTrigger("Dance2");
                StartCoroutine(PostRequest());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerAnimator.SetTrigger("Dance3");
                StartCoroutine(PostRequest());
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
