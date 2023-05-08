# TIL
2023-04-13
- Blender를 이용한 Model 생성 튜토리얼
- Unity 설치 및 튜토리얼
- 생성한 Blender 오브젝트 Unity에 적용

2023-04-14
- Blender 모델 생성 및 rigging 학습
- 나만의 캐릭터 fbx 추출
- .fbx파일 Unity서버에서의 동작 확인 및 테스트
-Unity 학습 진행

2023-04-19
- Photon Network 학습
- Photon Network 구현
- Photon Network 테스트

2023-04-20
- Photon Network 멀티플레이 구현
- Photon Network 멀티플레이 info 동기화

2023-04-21
- Plastic SCM 테스트
- Plastic SCM INIT 및 공유

2023-04-23
- Plastic SCM 머지 및 팀원 추가
- Plastic SCM 형상관리 전달

2023-04-24
- Spring Todolist API 명세 구현

2023-04-25
- Healing Section 낮과 밤 테스트
- Healing Section 까지의 포탈 버그 픽스 및 메인 월드와의 연결

2023-04-26
- Healing Section 낮과 밤 오브젝트 분리 및 렌더링 개선
- Healing Section 바다 물리엔진 추가 및 수영 컨텐츠 도입

2023-04-27
- Healing Section 바다 Post-Proccessing 구현
- Healing Section 수영 모션 애미매이션 추가
- Healing Section 수영 모션 애미매이션 테스트
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingWater : MonoBehaviour
{
    public static bool isWater = false;

    [SerializeField] private float waterDrag;   // 물속 중력
    private float originDrag;

    [SerializeField] private Color waterColor;  // 물속 색깔
    [SerializeField] private float waterFogDensity;   // 물 탁함 정도;

    private Color originColor;  // 원래의 색깔
    private float originFogDensity;  // 원래의 밀도

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter the Ocean");
        if (other.transform.tag == "Player")
        {
            GetInWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit the Ocean");
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetInWater(Collider _player)
    {
        isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;

        RenderSettings.fogColor = waterColor;
        RenderSettings.fogDensity = waterFogDensity;
    }

    private void GetOutWater(Collider _player)
    {
        if(isWater) 
        {
            isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;
        
            RenderSettings.fogColor = originColor;
            RenderSettings.fogDensity = originFogDensity;
        }
    }
}
```

2023-04-28
- HealingSecion Swimming 상호작용 추가 및 구현
- HealingSecion Swimming 조건 구성 및 개조

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingInteraction : MonoBehaviour
{
    private Animator animator;
    public LayerMask waterLayer;
    private bool isSwimming = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && other is BoxCollider)
        {
            OnSwimmingEnter();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && other is BoxCollider)
        {
            OnSwimmingExit();
        }
    }

    void OnSwimmingEnter()
    {
        isSwimming = true;
        animator.SetBool("IsSwimming", true);
    }

    void OnSwimmingExit()
    {
        isSwimming = false;
        animator.SetBool("IsSwimming", false);
    }
}
```


2023-05-01
- HealingSecion Fishing 상호작용 추가 및 구현
- HealingSecion 랜더링 개선
- 오큘러전 컬링 그룹군 적용 및 개선
- 수중 부력 물리개념 추가

2023-05-04
- HealingSection 캠프파이어 추가
- HealingSection 캠프파이어 오디오 
    - Player와의 거리에따라 오디오 음성 불룜 조절
- HealingSection 키고 끄기 상호작용

```C#
using UnityEngine;
using Photon.Pun;

public class FireInteraction : MonoBehaviourPunCallbacks
{
    public GameObject fireEffect; // 불 효과를 참조할 GameObject 변수
    public float interactionDistance = 3f; // 상호작용 거리
    private ParticleSystem fireParticleSystem; // Particle System 컴포넌트를 저장할 변수
    public Light firePointLight; // 불 주변의 조명을 담당하는 포인트 라이트
    private AudioSource audioSource; // 오디오 소스 컴포넌트를 참조하는 변수

    public float minDistance = 3.0f; // 오디오가 최대 볼륨으로 들리기 시작하는 거리
    public float maxDistance = 10.0f; // 오디오가 완전히 들리지 않게 되는 거리

    private void Start()
    {
        fireParticleSystem = fireEffect.GetComponent<ParticleSystem>(); // 불 효과의 Particle System을 참조
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 가져오기
        // audioSource.volume = 0.0f;
    }

    private void Update()
    {
        // 로컬 플레이어만 조작 가능하도록 함
        if (!photonView.IsMine) return;

        // 원 범위 내의 모든 콜라이더를 가져옵니다 (레이어 마스크를 사용하여 Player 레이어만 탐지하도록 함)
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance, layerMask);

        // 가장 가까운 플레이어를 찾습니다
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestPlayer = collider.gameObject;
                closestDistance = distance;
            }
        }

        if (closestPlayer != null)
        {
            Debug.Log("Closest Player");

            float distance = Vector3.Distance(transform.position, closestPlayer.transform.position);

            // 거리에 따라 오디오 소스의 볼륨을 조절합니다.
            if (distance <= minDistance)
            {
                audioSource.volume = 1.0f;
            }
            else if (distance <= maxDistance)
            {
                float t = (distance - minDistance) / (maxDistance - minDistance);
                audioSource.volume = 1.0f - t;
            }
            else
            {
                audioSource.volume = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Toggle Fire");
                photonView.RPC("ToggleFire", RpcTarget.All); // 모든 클라이언트에서 불 효과를 끄거나 켭니다
            }
        }
        else
        {
            audioSource.volume = 0.0f; // 플레이어가 멀리 떨어진 경우 볼륨을 0으로 설정합니다.
        }
    }

    [PunRPC]
    private void ToggleFire()
    {
        if (fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Stop(); // 불 효과 중지      
        }
        else
        {
            fireParticleSystem.Play(); // 불 효과 재생
        }
    }
}
```
2023-05-05 ~ 07
- HealingSection Rendering 문제
- 오큘러전 컬링 
- 오브젝트 풀링
- LOD 설정
- Light 새도우
- Player Loop
- 학습 및 도입
- Point Light 반사 및 그림자에서 프레임 드랍 확인
- Night Scene의 프레임 드랍 해결
- Camp Fire 상호작용 기능 및 오류 해결
    - 가까이 갔을 때 BGM 볼륨 조절
    - F 상호작용
    - 밤에만 Light 켜짐

```C#
using UnityEngine;
using Photon.Pun;

public class FireInteraction : MonoBehaviourPunCallbacks
{
    public GameObject fireEffect; // 불 효과를 참조할 GameObject 변수
    public float interactionDistance = 3f; // 상호작용 거리
    private ParticleSystem fireParticleSystem; // Particle System 컴포넌트를 저장할 변수
    public Light firePointLight; // 불 주변의 조명을 담당하는 포인트 라이트
    private AudioSource audioSource; // 오디오 소스 컴포넌트를 참조하는 변수

    public float minDistance = 3.0f; // 오디오가 최대 볼륨으로 들리기 시작하는 거리
    public float maxDistance = 10.0f; // 오디오가 완전히 들리지 않게 되는 거리

    private void Start()
    {
        fireParticleSystem = fireEffect.GetComponent<ParticleSystem>(); // 불 효과의 Particle System을 참조
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 가져오기
        // audioSource.volume = 0.0f;
    }

    private void Update()
    {
        // 로컬 플레이어만 조작 가능하도록 함
        if (!photonView.IsMine) return;

        // 원 범위 내의 모든 콜라이더를 가져옵니다 (레이어 마스크를 사용하여 Player 레이어만 탐지하도록 함)
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance, layerMask);

        // 가장 가까운 플레이어를 찾습니다
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestPlayer = collider.gameObject;
                closestDistance = distance;
            }
        }

        if (closestPlayer != null)
        {
            Debug.Log("Closest Player");

            float distance = Vector3.Distance(transform.position, closestPlayer.transform.position);

            // 거리에 따라 오디오 소스의 볼륨을 조절합니다.
            if (distance <= minDistance)
            {
                audioSource.volume = 1.0f;
            }
            else if (distance <= maxDistance)
            {
                float t = (distance - minDistance) / (maxDistance - minDistance);
                audioSource.volume = 1.0f - t;
            }
            else
            {
                audioSource.volume = 0.0f;
            }

            // 볼륨 조절 후, 오디오 소스의 재생 여부를 확인하고 필요한 경우 재생 또는 정지합니다.
            if (fireParticleSystem.isPlaying)
            {
                if (audioSource.volume > 0.0f && !audioSource.isPlaying)
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

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Toggle Fire");
                photonView.RPC("ToggleFire", RpcTarget.All); // 모든 클라이언트에서 불 효과를 끄거나 켭니다
            }
        }
        else
        {
            audioSource.volume = 0.0f; // 플레이어가 멀리 떨어진 경우 볼륨을 0으로 설정합니다.
        }
    }

    [PunRPC]
    private void ToggleFire()
    {
        if (fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Stop(); // 불 효과 중지
            firePointLight.enabled = false; // 불이 꺼져있을 때 조명 끄기
        }
        else
        {
            fireParticleSystem.Play(); // 불 효과 재생
            firePointLight.enabled = true; // 불이 켜져있을 때 조명 켜기
        }
    }
}
```