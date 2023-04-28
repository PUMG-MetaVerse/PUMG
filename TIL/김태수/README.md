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
-Spring Todolist API 명세 구현

2023-04-25
-Healing Section 낮과 밤 테스트
-Healing Section 까지의 포탈 버그 픽스 및 메인 월드와의 연결

2023-04-26
-Healing Section 낮과 밤 오브젝트 분리 및 렌더링 개선
-Healing Section 바다 물리엔진 추가 및 수영 컨텐츠 도입

2023-04-27
-Healing Section 바다 Post-Proccessing 구현
-Healing Section 수영 모션 애미매이션 추가
-Healing Section 수영 모션 애미매이션 테스트
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
-HealingSecion Swimming 상호작용 추가 및 구현
-HealingSecion Swimming 조건 구성 및 개조

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