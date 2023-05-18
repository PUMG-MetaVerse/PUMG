using UnityEngine;
using Photon.Pun;

public class StreamSound : MonoBehaviourPunCallbacks
{
    private AudioSource audioSource;

    public float maxDistance = 10.0f;
    public float maxVolume = 0.5f; // 추가한 부분

    private void Start()
    {
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
                float volume = Mathf.Clamp01(1.0f - (distance - audioSource.minDistance) / (audioSource.maxDistance - audioSource.minDistance)) * maxVolume; // 수정한 부분
                audioSource.volume = volume;

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }

        if (!isLocalPlayerInRange && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}