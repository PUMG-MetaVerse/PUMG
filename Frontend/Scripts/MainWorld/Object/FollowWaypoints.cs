using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
public class FollowWaypoints : MonoBehaviour
{
    public GameObject effectPrefab; // 이펙트 프리팹을 할당합니다.
    public float forceAmount = 1000f; 
    public float upwardForce = 300f;
    public GameObject waypointParent;
    public float waypointThreshold = 1.0f;
    private NavMeshAgent agent;
    private List<Transform> waypoints;
    private int currentWaypoint = 0;
    public AudioClip EffectSound;
    private AudioSource audioSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource =  GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = EffectSound;
        // Check if the waypointParent is assigned
        if (waypointParent == null)
        {
            Debug.LogError("Waypoint Parent is not assigned. Please assign it in the Inspector.");
            return;
        }

        // Initialize the waypoints list and populate it with the children transforms
        waypoints = new List<Transform>();
        foreach (Transform child in waypointParent.transform)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints found under the Waypoint Parent. Please add waypoint objects as children.");
            return;
        }

        // Set the initial destination to the first waypoint
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void Update()
    {
        // Check if the agent is close enough to the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) <= waypointThreshold)
        {
            // Set the next waypoint as the destination
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             GameObject effectInstance = Instantiate(effectPrefab, other.transform.position, Quaternion.identity);
            audioSource.Play();
            // 필요한 경우 이펙트 인스턴스의 수명을 설정하고, 시간이 지난 후 자동으로 제거할 수 있습니다.
            Destroy(effectInstance, 1f);

            // 플레이어의 Character Controller 컴포넌트를 가져옵니다.
            // CharacterController playerCharacterController = collision.collider.GetComponent<CharacterController>();
        }
    }
    // IEnumerator DestroyParticleAfterSeconds(GameObject instance, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     PhotonNetwork.Destroy(instance);
    // }

    private void OnCollisionEnter(Collision collision)
    {
        // 부딛힌 상대가 'Player' 태그를 가지고 있는지 확인합니다.
        if (collision.collider.CompareTag("Player"))
        {
            // 충돌 위치에서 이펙트 프리팹을 인스턴스화합니다.
            GameObject effectInstance = PhotonNetwork.Instantiate(effectPrefab.name, collision.contacts[0].point, Quaternion.identity);

            // 필요한 경우 이펙트 인스턴스의 수명을 설정하고, 시간이 지난 후 자동으로 제거할 수 있습니다.
            PhotonNetwork.Destroy(effectInstance);

            // 플레이어의 Character Controller 컴포넌트를 가져옵니다.
            CharacterController playerCharacterController = collision.collider.GetComponent<CharacterController>();

            // Character Controller가 있는지 확인합니다.
            if (playerCharacterController != null)
            {
                // 플레이어를 날려보내기 위한 힘을 계산합니다.
                Vector3 forceDirection = (collision.transform.position - transform.position).normalized;
                forceDirection.y = 1; // 대각선 방향으로 날리기 위해 Y값을 1로 설정합니다.

                // 플레이어에게 힘을 가합니다.
                playerCharacterController.Move(forceDirection * forceAmount * Time.deltaTime);
            }
        }
    }
}
