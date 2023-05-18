// using UnityEngine;

// public class ClubEntrance : MonoBehaviour
// {
//     public AudioSource mapAudioSource;
//     public AudioSource clubAudioSource;
//     public float maxDistance = 10.0f;

//     private Transform playerTransform;
//     private bool isPlayerInside = false;

//     private void Awake()
//     {
//         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         if (playerObj != null)
//         {
//             playerTransform = playerObj.transform;
//         }
//     }

//     private void Update()
//     {
//         if (playerTransform != null)
//         {
//             float distance = Vector3.Distance(playerTransform.position, transform.position);
//             float volume = Mathf.Clamp01(1.0f - (distance / maxDistance));
            
//             if (isPlayerInside)
//             {
//                 clubAudioSource.volume = volume;
//             }
//             else
//             {
//                 mapAudioSource.volume = volume;
//             }
//         }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             isPlayerInside = true;
//             mapAudioSource.Pause();
//             clubAudioSource.Play();
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             isPlayerInside = false;
//             mapAudioSource.Play();
//             clubAudioSource.Pause();
//         }
//     }
// }

using UnityEngine;
using Photon.Pun;

public class ClubEntrance : MonoBehaviour
{
    public AudioSource mapAudioSource;
    public AudioSource clubAudioSource;
    public float maxDistance = 10.0f;
    public float maxVolume = 0.5f; // 추가한 부분

    private void Awake()
    {
        clubAudioSource = GetComponent<AudioSource>();
        mapAudioSource = GetComponent<AudioSource>();
        clubAudioSource.spatialBlend = 1.0f;
        clubAudioSource.minDistance = 3.0f;
        clubAudioSource.maxDistance = maxDistance;
        clubAudioSource.volume = 0.0f;
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
                float volume = Mathf.Clamp01(1.0f - (distance - clubAudioSource.minDistance) / (clubAudioSource.maxDistance - clubAudioSource.minDistance)) * maxVolume; // 수정한 부분
                clubAudioSource.volume = volume;

                if (!clubAudioSource.isPlaying)
                {
                    mapAudioSource.Pause();
                    clubAudioSource.Play();
                }
            }
        }

        if (!isLocalPlayerInRange && clubAudioSource.isPlaying)
        {
            mapAudioSource.Play();
            clubAudioSource.Stop();
        }
    }
}