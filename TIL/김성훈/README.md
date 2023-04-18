# TIL

## 4/13

- 초기 기획

- 유니티 셋업 학습

## 4/14

- 뱡향, 점프, 회피 만들기

- 아이템 만들기



## 4/18

# 댄싱 존에 도착했을 때만 춤출 수 있게 하는 거

1. 댄싱존을 만듬

```csharp
using UnityEngine;
using UnityEngine.UI;
// TextMeshPro를 사용하는 경우 추가
using TMPro;

public class DanceZone : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject hintText; // 텍스트 요소를 참조할 변수 추가
    private bool playerInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            hintText.SetActive(true); // 플레이어가 댄싱 존에 들어오면 텍스트 요소를 활성화
            Debug.Log("Player entered the dance zone."); // 콘솔에 메시지 출력
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            hintText.SetActive(false); // 플레이어가 댄싱 존에서 나가면 텍스트 요소를 비활성화
        }
    }

    void Update()
    {
        if (playerInZone)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                playerAnimator.SetTrigger("Dance1");
                Debug.Log("Player pressed key 1 to dance."); // 콘솔에 메시지 출력
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                playerAnimator.SetTrigger("Dance2");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                playerAnimator.SetTrigger("Dance3");
            }
        }
    }
}
```

이 스크립트는 댄싱존에 왔을 때 1, 2, 3을 누르면 춤을 출 수 있게 해줌

근데 이거 mesh Renderer를 꺼야댐 is Trigger(충돌 이벤트 감지)을 체크해줘야 되기 때문임 저걸 체크해야 OnTrigger 이벤트가 발생함.

이게 충돌 이벤트가 발생해야함.

댄싱존과 플레이어 모두 콜라이더 컴포넌트를 가지고 있어야하고

한 쪽에는 Rigidbody가 있어야 함

플레이어 게임 오브젝트에 "Player" 태그가 지정되어 있는지 확인해야함.

애니메이션은 플레이어 애니메이터에서 idle랑 Transition 연결 해주면 될꺼 같음

이 때 Parameters에 코드와 일치하는 Trigger을 만들어 주고 Transition 에 Conditions에 추가해줘야 함
