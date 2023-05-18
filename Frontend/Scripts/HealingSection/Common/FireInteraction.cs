using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using System.Text;
using System;
public class FireInteraction : MonoBehaviourPunCallbacks
{
    public GameObject fireEffect;
    public float interactionDistance = 3f;
    private ParticleSystem fireParticleSystem;
    public Light firePointLight;
    private AudioSource audioSource;
    public float maxDistance = 10.0f;

    private void Start()
    {
        fireParticleSystem = fireEffect.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = 3.0f;
        audioSource.maxDistance = maxDistance;
        audioSource.volume = 0.0f;
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, LayerMask.GetMask("Player"));

        bool isLocalPlayerInRange = false;

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetPhotonView().IsMine)
            {
                isLocalPlayerInRange = true;
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                float volume = Mathf.Clamp01(1.0f - (distance - audioSource.minDistance) / (audioSource.maxDistance - audioSource.minDistance));
                audioSource.volume = volume;

                if (fireParticleSystem.isPlaying)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
            }
        }

        if (isLocalPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            photonView.RPC("ToggleFire", RpcTarget.All);
        }
    }

    private IEnumerator PostRequest()
    {
        string json = JsonUtility.ToJson(
            new HealingInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                titleIdx = 9,
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

    [PunRPC]
    private void ToggleFire()
    {
        if (fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Stop();
            firePointLight.enabled = false;
        }
        else
        {
            fireParticleSystem.Play();
            firePointLight.enabled = true;
            StartCoroutine(PostRequest());
        }
    }

    public bool IsFireOn()
    {
        return fireParticleSystem.isPlaying;
    }
    
    [System.Serializable]
    public class HealingInfo
    {
        public int userIdx;
        public int titleIdx;
    }
}