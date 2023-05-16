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

4/19

Starter Assets - Third Person Character Controller를 사용하여 Unity에서 미니맵을 만들 수 있습니다. 미니맵은 플레이어의 위치와 주변 환경을 간단하게 나타내는 작은 지도입니다. 이를 구현하기 위해 아래의 단계를 따라주세요.

1. 미니맵 카메라 설정:
   
   - Hierarchy에서 새 카메라를 생성하고 이름을 'Minimap Camera'로 지정합니다.
   
   - Minimap Camera를 플레이어 캐릭터의 자식 오브젝트로 설정합니다.
   
   - 카메라의 Projection을 'Orthographic'으로 변경하고, Clear Flags를 'Solid Color'로 변경한 후 배경색을 원하는 색상으로 설정합니다
     
     1. 카메라의 Projection 변경:
        - Hierarchy에서 'Minimap Camera'를 선택합니다.
        - Inspector 창에서 'Camera' 컴포넌트를 찾아봅니다.
        - 'Projection' 항목에서 드롭다운 메뉴를 클릭하여 'Perspective' 대신 'Orthographic'을 선택합니다.
        - Orthographic Projection은 3D 공간을 2D로 표현하기 때문에 미니맵에 적합합니다.
     2. 카메라의 Clear Flags 및 배경색 변경:
        - 'Minimap Camera'를 선택한 상태에서 Inspector 창의 'Camera' 컴포넌트를 찾습니다.
        - 'Clear Flags' 항목에서 드롭다운 메뉴를 클릭하고 'Solid Color'를 선택합니다. 이렇게 하면 카메라 뒷 배경이 단색으로 채워집니다.
        - 'Background' 항목에서 색상 박스를 클릭합니다. 새로운 색상 선택기 창이 나타납니다.
        - 원하는 배경색을 선택한 후 색상 선택기 창을 닫습니다.
   
   - 카메라의 위치와 회전을 조정하여 플레이어를 위에서 바라보도록 설정합니다.
   
   - Minimap Camera의 Culling Mask에서 미니맵에 표시할 레이어를 선택하고, 불필요한 레이어는 제외합니다.

2. 미니맵 UI 생성:
   
   - Hierarchy에서 'UI' > 'Canvas'를 생성합니다.
   
   - 새로 생성된 Canvas의 Render Mode를 'Screen Space - Overlay'로 설정합니다.
   
   - Canvas 안에 'UI' > 'Image'를 생성하고, 이를 'Minimap'으로 이름 변경합니다.
   
   - Image의 Rect Transform을 조절하여 미니맵의 크기 및 위치를 설정합니다.
     
     ![Untitled](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/c51084d1-4d0a-48e5-aaac-bce149ca04dc/Untitled.png)
   
   - Image 오브젝트 안에 'UI' > 'Raw Image'를 생성하고, 이름을 'Minimap Render'로 변경합니다.
   
   - Minimap Render의 Rect Transform을 조절하여 미니맵 이미지의 크기 및 위치를 설정합니다.
     
     1. Image의 Rect Transform 조절:
        - Hierarchy에서 'Minimap' Image를 선택합니다.
        - Inspector 창에서 'Rect Transform' 컴포넌트를 찾습니다.
        - 'Anchors', 'Pivot', 'Position', 'Width', 'Height' 등의 값을 조절하여 미니맵의 크기와 위치를 설정합니다. 예를 들어, 'Width'와 'Height'를 200으로 설정하면 미니맵의 크기가 200x200 픽셀이 됩니다.
     2. Minimap Render 생성 및 설정:
        - Hierarchy에서 'Minimap' Image를 선택하고, 'UI' > 'Raw Image'를 생성합니다.
        - 생성된 Raw Image 오브젝트의 이름을 'Minimap Render'로 변경합니다.
        - Minimap Render 오브젝트의 'Rect Transform' 컴포넌트를 조절하여 미니맵 이미지의 크기와 위치를 설정합니다. 일반적으로 Minimap Image와 동일한 크기와 위치로 설정합니다.
     3. RenderTexture 생성 및 설정:
        - 'Project' 창에서 우클릭하고 'Create' > 'RenderTexture'를 선택하여 새 RenderTexture를 생성합니다.
        - 생성된 RenderTexture의 이름을 'Minimap RenderTexture'로 변경합니다.
        - Inspector 창에서 'Size'와 'Depth Buffer' 값을 조절하여 원하는 미니맵 해상도를 설정합니다. 예를 들어, 'Size'를 512x512로 설정하면 더 높은 해상도의 미니맵이 생성됩니다.
     4. Minimap Camera에 RenderTexture 할당:
        - Hierarchy에서 'Minimap Camera'를 선택합니다.
        - Inspector 창의 'Camera' 컴포넌트에서 'Target Texture' 항목을 찾습니다.
        - 'Project' 창에서 'Minimap RenderTexture'를 클릭하고 드래그하여 'Target Texture' 항목에 놓습니다. 이렇게 하면 Minimap Camera가 RenderTexture에 출력을 보냅니다.
     5. Minimap Render에 RenderTexture 할당:
        - Hierarchy에서 'Minimap Render' 오브젝트를 선택합니다.
        - Inspector 창의 'Raw Image' 컴포넌트에서 'Texture' 항목을 찾습니다.
        - 'Project' 창에서 'Minimap RenderTexture'를 클릭하고 드래그하여 'Texture' 항목에 놓습니다. 이렇게 하면 Minimap Render에서 RenderTexture를 표시할 수 있습니다.

3. 미니맵 카메라 렌더링:
- 프로젝트에서 'RenderTexture'를 생성하고 이름을 'Minimap RenderTexture'로 변경합니다.
- RenderTexture의 크기와 해상도를 조절하여 원하는 미니맵 해상도를 설정합니다.
- Minimap Camera의 'Target Texture'에 'Minimap RenderTexture'를 할당합니다.
- Minimap Render 오브젝트의 'Raw Image' 컴포넌트에 'Minimap RenderTexture'를 할당합니다.

이제 플레이어가 움직일 때 미니맵이 함께 움직이며, 화면에 미니맵이 표시됩니다. 필요한 경우 미니맵의 디자인을 개선하거나, 스크립트를 추가하여 더 다양한 기능을 구현할 수 있습니다.

1. Hierarchy에서 'Minimap' Image를 선택합니다.
2. Inspector 창에서 'Rect Transform' 컴포넌트를 찾습니다.
3. Anchors 설정:
   - 'Rect Transform' 컴포넌트에서 'Anchors' 섹션을 찾아봅니다.
   - 'Min' 항목에서 'X' 값을 1로 설정하고, 'Y' 값을 0으로 설정합니다.
   - 'Max' 항목에서 'X' 값을 1로 설정하고, 'Y' 값을 0으로 설정합니다.
4. 미니맵의 위치를 조정합니다.
   - 'Rect Transform'에서 'Pos X' 및 'Pos Y' 값을 조절하여 미니맵이 화면 오른쪽 하단에 적절한 위치에 놓이도록 설정합니다. 예를 들어, 'Pos X'를 -20, 'Pos Y'를 20으로 설정하면 오른쪽 하단에서 20 픽셀 떨어진 위치에 미니맵이 표시됩니다.

이제 미니맵이 화면 오른쪽 하단에 위치하게 됩니다. 더 나은 사용자 경험을 위해 미니맵의 크기 및 위치를 추가로 조절할 수 있습니다.

## 4/20

# 노래 바꾸기

Unity에서 각 LP 판을 클릭하면 노래를 바꿔 틀도록 설정하려면 다음 단계를 따르세요:

1. 각 LP 판 게임 오브젝트에 콜라이더(Collider)를 추가하세요. 이를 통해 클릭 이벤트를 감지할 수 있습니다.
2. 각 LP 판에 AudioSource 컴포넌트를 추가하고, 각 AudioSource의 AudioClip 속성에 대응하는 노래를 할당하세요.
3. LP 판 게임 오브젝트에 아래와 같은 스크립트를 추가하세요. 이 스크립트는 클릭 이벤트를 처리하고, 노래를 바꿔 틀어줍니다.

```csharp
csharpCopy code
using UnityEngine;

public class LpClickHandler : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        // 다른 LP 판들의 노래를 정지
        GameObject[] lpObjects = GameObject.FindGameObjectsWithTag("LP");
        foreach (GameObject lp in lpObjects)
        {
            AudioSource lpAudioSource = lp.GetComponent<AudioSource>();
            if (lpAudioSource != audioSource && lpAudioSource.isPlaying)
            {
                lpAudioSource.Stop();
            }
        }

        // 이 LP 판의 노래를 틀거나 정지
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
}
```

1. LP 판 게임 오브젝트에 'LP' 태그를 할당하세요. 이렇게 하면 스크립트에서 다른 LP 판들의 노래를 정지할 수 있습니다.
   
   이제 각 LP 판을 클릭할 때마다 노래가 바뀌어 재생됩니다. 한 번에 한 개의 노래만 틀어지도록 설정되어 있으며, 동일한 LP 판을 다시 클릭하면 노래가 정지합니다.

## 4/21

# 캠프파이어

캠프파이어를 구현하려면, 불의 크기를 조절할 수 있는 스크립트를 작성하고, 플레이어가 장작을 불에 추가할 수 있는 인터랙션을 만들어야 합니다. 아래 예제 코드를 참고해 주세요.

1. 캠프파이어 불의 크기를 조절하는 스크립트를 작성합니다.

```
csharpCopy code
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public int maxFireLevel = 10;
    public float fireGrowthAmount = 0.1f;
    private int currentFireLevel = 0;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    public void AddFirewood()
    {
        if (currentFireLevel < maxFireLevel)
        {
            currentFireLevel++;
            transform.localScale += new Vector3(fireGrowthAmount, fireGrowthAmount, fireGrowthAmount);
        }
    }
}
```

1. 플레이어가 장작을 캠프파이어에 추가할 수 있도록 하는 스크립트를 작성합니다.

```
csharpCopy code
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public int firewoodAmount = 10;
    private Campfire campfire;

    private void Start()
    {
        campfire = FindObjectOfType<Campfire>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && firewoodAmount > 0)
        {
            firewoodAmount--;
            campfire.AddFirewood();
            Debug.Log("Firewood added to the campfire.");
        }
    }
}
```

이 코드에서 **`Campfire`** 스크립트는 캠프파이어 불 게임 오브젝트에 추가되어야 합니다. 그리고 **`PlayerInteraction`** 스크립트는 플레이어 게임 오브젝트에 추가되어야 합니다.

플레이어가 'E' 키를 누를 때마다 장작을 하나씩 사용하여 캠프파이어 불의 크기가 커집니다. **`fireGrowthAmount`** 변수를 조절하여 불이 얼마나 커지는지 조절할 수 있습니다. 또한, **`maxFireLevel`** 변수를 조절하여 최대 불의 크기 단계를 변경할 수 있습니다.

## 04/24

```csharp
//10초마다 밤 바뀌는거

using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public GameObject sunLightObject;
    public Material daySkybox;
    public Material nightSkybox;
    public float dayLength = 10f;

    private Light sunLight;
    private float currentTime;
    private float lerpValue;

    void Start()
    {
        currentTime = 0;
        sunLight = sunLightObject.GetComponent<Light>();
        RenderSettings.skybox = daySkybox;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        lerpValue = Mathf.Sin(currentTime / dayLength * Mathf.PI * 2);
        lerpValue = (lerpValue + 1) / 2;

        sunLight.transform.rotation = Quaternion.Euler(new Vector3(360 * lerpValue, 0, 0)); // Directional Light의 회전을 변경하여 낮과 밤을 전환합니다.

        if (lerpValue > 0.5f)
        {
            if (RenderSettings.skybox != daySkybox)
            {
                RenderSettings.skybox = daySkybox;
            }
        }
        else
        {
            if (RenderSettings.skybox != nightSkybox)
            {
                RenderSettings.skybox = nightSkybox;
            }
        }
        DynamicGI.UpdateEnvironment();
    }
}
```

## 4/25

```

```

## 4/26

```
# 낚시

- GameObject character = Instantiate(characterPrefab, spawnPosition, spawnRotation);
character.tag = "Player";
- yourCharacterGameObject.tag = "Player";

1. 의자와 낚시대 3D 모델 가져오기:
    - Unity 에셋 스토어에서 의자와 낚시대 3D 모델을 찾거나, 외부 3D 모델링 프로그램에서 작성한 모델을 가져옵니다.
    - Unity 에디터에서 프로젝트 패널의 'Assets' 폴더에 모델을 드래그 앤 드롭합니다. 이렇게 하면 모델이 프로젝트에 임포트됩니다.
2. 콜라이더와 리지드 바디 추가:
    - Hierarchy 패널에서 의자와 낚시대 게임 오브젝트를 선택합니다.
    - Inspector 패널에서 'Add Component' 버튼을 클릭하고, 각각에 적절한 콜라이더를 추가합니다 (예: Box Collider, Mesh Collider 등).
    - 의자에는 리지드 바디가 필요하지 않지만, 낚시대에는 리지드 바디를 추가하고 'Is Kinematic' 옵션을 체크하여 물리 시뮬레이션에서 제외합니다. 이렇게 하면 낚시대가 플레이어의 손에 부착될 때 문제가 발생하지 않습니다.
3. 플레이어 스크립트 작성 및 추가:
    - 프로젝트 패널에서 'Create > C# Script'를 선택하여 새 스크립트를 생성하고 이름을 지정합니다 (예: FishingController).
    - Hierarchy에서 플레이어 게임 오브젝트를 선택하고, Inspector 패널에서 'Add Component' 버튼을 클릭하여 새로 생성한 스크립트를 추가합니다.
4. 스크립트 기능 구현:
    - 스크립트를 더블 클릭하여 코드 에디터에서 엽니다.
    - 아래 코드를 참조하여 스크립트를 작성하고, 기능을 구현합니다. 이 코드는 의자와 충돌 감지, 'V' 키 입력 처리, 플레이어를 의자에 앉게 하고, 낚시대를 손에 부착하는 기능을 포함합니다.

    ```csharp
    using UnityEngine;

    public class FishingController : MonoBehaviour
    {
        public GameObject chair;
        public GameObject fishingRod;
        public Transform handPosition;

        private bool isNearChair = false;
        private bool isSitting = false;

        void Update()
        {
            if (isNearChair && Input.GetKeyDown(KeyCode.V))
            {
                if (!isSitting)
                {
                    SitOnChair();
                }
                else
                {
                    StandUpFromChair();
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == chair)
            {
                isNearChair = true;
            }
        }

            void OnTriggerExit(Collider other)
            {
                if (other.gameObject == chair)
                {
                    isNearChair = false;
                }
            }

            private void SitOnChair()
            {
                isSitting = true;
                transform.position = chair.transform.position; // 플레이어를 의자의 위치로 이동시킵니다.
                transform.rotation = chair.transform.rotation; // 플레이어의 회전을 의자의 회전과 일치시킵니다.

                fishingRod.transform.SetParent(handPosition); // 낚시대를 플레이어의 손 위치에 부착시킵니다.
                fishingRod.transform.localPosition = Vector3.zero; // 낚시대의 로컬 위치를 초기화합니다.
                fishingRod.transform.localRotation = Quaternion.identity; // 낚시대의 로컬 회전을 초기화합니다.
            }

            private void StandUpFromChair()
            {
                isSitting = false;
                transform.position = chair.transform.position + Vector3.up * 2f; // 플레이어를 의자에서 일어난 위치로 이동시킵니다.

                fishingRod.transform.SetParent(null); // 낚시대의 부모를 제거하여 플레이어의 손에서 분리합니다.
            }
        }
    ```

    이 코드를 추가하면 플레이어가 의자에 앉고 일어서는 것과 낚시대를 손에 붙였다 뗐다 하는 것을 구현할 수 있습니다. 이제 Unity 에디터에서 플레이어와 의자, 낚시대 게임 오브젝트를 적절히 배치하고 테스트해보세요.
```

## 4/28

### ㅇㅏㄵ는 애니메이션 적용 코드

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairInteract : MonoBehaviour
{
    public bool isSitting;
    public bool isInRange;
    public GameObject player;
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.H;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInRange = true;
            player = other.gameObject;
            Debug.Log("Player entered trigger range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInRange = false;
            player = null;
            Debug.Log("Player left trigger range.");
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Interact key pressed.");
            if (!isSitting)
            {
                Sit();
            }
            else
            {
                Stand();
            }
        }
    }

    private void Sit()
    {
        isSitting = true;
        player.GetComponent<Animator>().SetTrigger("IsSitting");
        Debug.Log("Sit triggered.");
        // 다른 필요한 동작을 추가하세요 (예: 플레이어 위치 조정)
    }

    private void Stand()
    {
        isSitting = false;
        player.GetComponent<Animator>().SetTrigger("IsStanding");
        Debug.Log("Stand triggered.");
        // 다른 필요한 동작을 추가하세요 (예: 플레이어 위치 조정)
    }
}
```

## 5/1

```C#
//최종본
using System.Collections;
using UnityEngine;
using StarterAssets;

public class ChairInteract : MonoBehaviour
{
    public bool isSitting;
    public bool isInRange;
    public GameObject player;
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.H;
    public Transform sitPosition;
    public CharacterController playerController;
    public ThirdPersonController thirdPersonControllerScript;
    public KeyCode throwKey = KeyCode.T;
    public Transform playerHands;
    public string fishingPoleName = "Fishing-pole";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInRange = true;
            player = other.gameObject;
            playerController = player.GetComponent<CharacterController>();
            thirdPersonControllerScript = player.GetComponent<ThirdPersonController>();
            // playerHands = player.transform.Find("Mesh/Body/Hands");
            playerHands = player.transform.Find("Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_L/Shoulder_L/Elbow_L/Wrist_L");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInRange = false;
            player = null;
            playerController = null;
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            if (!isSitting)
            {
                Sit();
            }
            else
            {
                Stand();
            }
        }

        if (Input.GetKeyDown(throwKey) && isSitting)
        {
            Animator animator = player.GetComponent<Animator>();
            bool isThrowing = animator.GetBool("isThrowing");

            if (!isThrowing)
            {
                animator.SetBool("isThrowing", true);
            }
            else
            {
                animator.SetBool("isThrowing", false);
            }
        }
    }

    private void Sit()
    {
        isSitting = true;
        player.GetComponent<Animator>().SetTrigger("IsSitting");
        player.transform.position = sitPosition.position;
        player.transform.rotation = sitPosition.rotation;
        playerController.enabled = false;
        thirdPersonControllerScript.isSitting = true;

        Transform fishingPole = transform.Find(fishingPoleName);
        if (fishingPole != null)
        {
            fishingPole.SetParent(playerHands);
            fishingPole.localPosition = new Vector3(0.09f, -0.02f, 0.07f);
            fishingPole.localRotation = Quaternion.Euler(39f, 82f, -185f);
        }
        else
        {
            Debug.LogError("Fishing-pole not found as a child of the chair object.");
        }
    }

    private void Stand()
    {
        if (player == null)
        {
            return;
        }

        isSitting = false;
        player.GetComponent<Animator>().SetTrigger("IsStanding");
        playerController.enabled = true;
        thirdPersonControllerScript.isSitting = false;

        Transform fishingPoleInHand = playerHands.Find(fishingPoleName);
        if (fishingPoleInHand != null)
        {
            fishingPoleInHand.SetParent(transform);
            fishingPoleInHand.position = playerHands.position;
            fishingPoleInHand.rotation = playerHands.rotation;
            fishingPoleInHand.localRotation = Quaternion.Euler(0, 0, 0);

            Vector3 offset = playerHands.transform.forward * 0.25f;
            fishingPoleInHand.position += offset;
        }
    }
}
```

### 5/2

```C
using System.Collections;
using UnityEngine;
using StarterAssets;

public class LayDownGetUp : MonoBehaviour
{
    public bool isLayDown;
    public bool isInRange;
    private GameObject player;
    private string playerTag = "Player";
    private KeyCode interactKey = KeyCode.H;
    public Transform layDownPosition;
    public CharacterController playerController;
    public ThirdPersonController thirdPersonControllerScript;
    // public Vector3 layDownLocalOffset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInRange = true;
            player = other.gameObject;
            playerController = player.GetComponent<CharacterController>();
            thirdPersonControllerScript = player.GetComponent<ThirdPersonController>();
            Debug.Log("lounger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isInRange = false;
            player = null;
            playerController = null;
            Debug.Log("loungerout");
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            if (!isLayDown)
            {
                LayDown();
            }
            else
            {
                GetUp();
            }
        }
    }

    private void LayDown()
    {
        isLayDown = true;
        player.GetComponent<Animator>().SetBool("IsLayDown", true);

        // 로컬 오프셋을 적용한 위치 계산
        // Vector3 layDownPositionWithOffset = layDownPosition.TransformPoint(layDownLocalOffset);

        player.transform.position = layDownPosition.position;
        player.transform.rotation = layDownPosition.rotation;
        playerController.enabled = false;
        thirdPersonControllerScript.isSitting = true; 
    }

    private void GetUp()
    {
        if (player == null)
        {
            return;
        }

        isLayDown = false;
        player.GetComponent<Animator>().SetBool("IsLayDown", false);
        playerController.enabled = true;
        thirdPersonControllerScript.isSitting = false; 
    }
}
```

## 5/3

## 클럽

```csharp
using UnityEngine;
using TMPro;
using System.Collections;

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

```csharp
using UnityEngine;

public class ClubEntrance : MonoBehaviour
{
    public AudioSource mapAudioSource;
    public AudioSource clubAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mapAudioSource.Pause();
            clubAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mapAudioSource.Play();
            clubAudioSource.Pause();
        }
    }
}
```

```csharp
using UnityEngine;
using System.Collections;

public class CDPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    private bool playerInRange = false;
    private AudioSource clubAudioSource;
    private AudioClip originalClip;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered the CD player zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited the CD player zone.");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.H))
        {
            ChangeSong();
            Debug.Log("Player pressed H to change the song.");
        }
    }

    private void ChangeSong()
    {
        if (audioClip != null)
        {
            if (clubAudioSource == null)
            {
                GameObject clubMusicObject = GameObject.FindGameObjectWithTag("ClubMusic");
                if (clubMusicObject != null)
                {
                    clubAudioSource = clubMusicObject.GetComponent<AudioSource>();
                    originalClip = clubAudioSource.clip;
                }
                else
                {
                    Debug.LogError("ClubMusic object not found.");
                }
            }

            if (clubAudioSource != null)
            {
                clubAudioSource.clip = audioClip;
                clubAudioSource.Play();
                StartCoroutine(PlayOriginalClipAfterDelay(audioClip.length));
            }
            else
            {
                Debug.LogError("Club AudioSource not found.");
            }
        }
        else
        {
            Debug.LogError("AudioClip is not assigned.");
        }
    }

    private IEnumerator PlayOriginalClipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clubAudioSource.clip = originalClip;
        clubAudioSource.Play();
    }
}
```

```csharp

```

### 5/4

```csharp
using UnityEngine;

public class LaserBeams : MonoBehaviour
{
    public int numberOfLasers = 6;
    public float laserRange = 50f;
    public float laserStartWidth = 0.3f;
    public float laserEndWidth = 0.1f;
    public float startAngle = 45f; // 부채꼴 시작 각도
    public float endAngle = 135f; // 부채꼴 끝 각도
    public Color[] laserColors = { Color.blue, Color.green, Color.cyan, Color.blue, Color.green, Color.cyan };

    private LineRenderer[] lineRenderers;
    private float[] flickerTimers;

    void Start()
    {
        lineRenderers = new LineRenderer[numberOfLasers];
        flickerTimers = new float[numberOfLasers];

        for (int i = 0; i < numberOfLasers; i++)
        {
            GameObject laser = new GameObject($"Laser_{i}");
            laser.transform.SetParent(transform);
            lineRenderers[i] = laser.AddComponent<LineRenderer>();
            lineRenderers[i].startWidth = laserStartWidth;
            lineRenderers[i].endWidth = laserEndWidth;
            lineRenderers[i].material = new Material(Shader.Find("Sprites/Default"));
            Color semiTransparentColor = laserColors[i % laserColors.Length];
            semiTransparentColor.a = 0.5f;
            lineRenderers[i].material.color = semiTransparentColor;
            lineRenderers[i].sortingOrder = 1;
            flickerTimers[i] = Random.Range(1f, 0.8f);
        }
    }

    void Update()
    {
        float angleRange = endAngle - startAngle;
        for (int i = 0; i < numberOfLasers; i++)
        {
            float angle = startAngle + (angleRange / (numberOfLasers - 1)) * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.up;

            lineRenderers[i].SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, laserRange);
            if (hit.collider != null)
            {
                lineRenderers[i].SetPosition(1, hit.point);
            }
            else
            {
                lineRenderers[i].SetPosition(1, transform.position + direction * laserRange);
            }

            flickerTimers[i] -= Time.deltaTime;
            if (flickerTimers[i] <= 0)
            {
                lineRenderers[i].enabled = !lineRenderers[i].enabled;
                flickerTimers[i] = Random.Range(0.2f, 0.8f);
            }
        }
    }
}
```

### 5/8

```csharp

```

## 5/9

```
풍선 두개 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace HealingSection.Scripts
{
    public class BalloonPickup : MonoBehaviour
    {
        public GameObject balloonPrefab;
        public float fallSpeed = 0.5f;
        private GameObject[] balloonInstances = new GameObject[2];
        private bool canPickup = false;
        private Rigidbody playerRigidbody;
        // private Transform playerHands;
        private Transform playerLeftHand;
        private Transform playerRightHand;
        private bool isFalling = false;
        private ThirdPersonController thirdPersonController;
        private float originalDrag;
        private float originalGravity;

        private void Update()
        {
            if (canPickup && Input.GetKeyDown(KeyCode.F))
            {
                AttachBalloon();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canPickup = true;
                thirdPersonController = other.GetComponent<ThirdPersonController>();
                playerRigidbody = other.GetComponent<Rigidbody>();
                playerLeftHand = other.transform.Find("Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_L/Shoulder_L/Elbow_L/Wrist_L");
                playerRightHand = other.transform.Find("Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_R/Shoulder_R/Elbow_R/Wrist_R");
                originalDrag = playerRigidbody.drag;
                Debug.Log("Player entered trigger area.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                canPickup = false;
            }
        }

        private void AttachBalloon()
        {
            if (balloonInstances[0] == null && balloonInstances[1] == null)
            {
                StartCoroutine(ChangePlayerStats());
            }
        }

        private IEnumerator ChangePlayerStats()
        {

            Debug.Log("Giving player balloons and changing gravity.");

            balloonInstances[0] = Instantiate(balloonPrefab, playerLeftHand.position, Quaternion.identity);
            balloonInstances[0].transform.SetParent(playerLeftHand);
            balloonInstances[0].transform.localPosition = Vector3.zero;

            balloonInstances[1] = Instantiate(balloonPrefab, playerRightHand.position, Quaternion.identity);
            balloonInstances[1].transform.SetParent(playerRightHand);
            balloonInstances[1].transform.localPosition = Vector3.zero;

            float originalGravity = thirdPersonController.Gravity;
            thirdPersonController.Gravity = -0.01f;
            playerRigidbody.drag = 10f;

            Physics.gravity *= 0.1f;

            isFalling = true;

            while (isFalling)
            {
                Vector3 motion = new Vector3(0, fallSpeed , 0) * Time.deltaTime;
                playerRigidbody.AddForce(motion, ForceMode.VelocityChange);

                if (balloonInstances[0] != null)
                {
                    balloonInstances[0].transform.position = playerLeftHand.position;
                }
                if (balloonInstances[1] != null)
                {
                    balloonInstances[1].transform.position = playerRightHand.position;
                }

                if (IsPlayerTouchingGroundOrWater())
                {
                    DetachBalloon();
                    break;
                }

                if (!isFalling)
                {
                    ResetPlayerStats();
                    yield break;
                }

                yield return null;
            }

            thirdPersonController.Gravity = originalGravity;
            if (playerRigidbody != null)
            {
                playerRigidbody.drag = originalDrag;
                playerRigidbody.useGravity = true;
            }
                        Debug.Log("Player gravity and drag returned to original values.");

            if (balloonInstances[0] != null)
            {
                Destroy(balloonInstances[0]);
                balloonInstances[0] = null;
            }
            if (balloonInstances[1] != null)
            {
                Destroy(balloonInstances[1]);
                balloonInstances[1] = null;
            }

        }

        private void DetachBalloon()
        {
            if (balloonInstances[0] != null)
            {
                Destroy(balloonInstances[0]);
                balloonInstances[0] = null;
            }
            if (balloonInstances[1] != null)
            {
                Destroy(balloonInstances[1]);
                balloonInstances[1] = null;
            }
            isFalling = false;
            ResetPlayerStats();
        }

        private void ResetPlayerStats()
        {
            thirdPersonController.Gravity = originalGravity;
            if (playerRigidbody != null)
            {
                playerRigidbody.drag = originalDrag;
                playerRigidbody.useGravity = true;
            }
            Debug.Log("Player gravity and drag returned to original values.");
        }

        private bool IsPlayerTouchingGroundOrWater()
        {
            float distanceToGround = 0.5f; 
            int groundAndWaterLayerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Water"));

            RaycastHit hit;
            if (Physics.Raycast(playerRigidbody.transform.position, Vector3.down, out hit, distanceToGround, groundAndWaterLayerMask))
            {
                return true;
            }
            return false;
        }
    }
}
```

## 5/10

```

```

## 5/11

```
// 완성

using UnityEngine;

public class LaserBeams : MonoBehaviour
{
    public int numberOfLasers = 6;
    public float laserRange = 50f;
    public float laserStartWidth = 0.3f;
    public float laserEndWidth = 0.1f;
    public float startAngle = -50f; // 부채꼴 시작 각도
    public float endAngle = 50f; // 부채꼴 끝 각도
    public Color[] laserColors = { Color.blue, Color.green, Color.cyan, Color.blue, Color.green, Color.cyan };

    private LineRenderer[] lineRenderers;
    private float[] flickerTimers;

    void Start()
    {
        lineRenderers = new LineRenderer[numberOfLasers];
        flickerTimers = new float[numberOfLasers];
        float angleRange = endAngle - startAngle;

        for (int i = 0; i < numberOfLasers; i++)
        {
            GameObject laser = new GameObject($"Laser_{i}");
            laser.transform.SetParent(transform);
            laser.transform.localRotation = Quaternion.Euler(0f, 0f, startAngle + (angleRange / (numberOfLasers - 1)) * i);
            lineRenderers[i] = laser.AddComponent<LineRenderer>();
            lineRenderers[i].startWidth = laserStartWidth;
            lineRenderers[i].endWidth = laserEndWidth;
            lineRenderers[i].material = new Material(Shader.Find("Sprites/Default"));
            Color semiTransparentColor = laserColors[i % laserColors.Length];
            semiTransparentColor.a = 0.5f;
            lineRenderers[i].material.color = semiTransparentColor;
            lineRenderers[i].sortingOrder = 1;
            flickerTimers[i] = Random.Range(1f, 0.8f);
        }
    }


    void Update()
    {
        Vector3[] directions = new Vector3[numberOfLasers];
        directions[0] = Quaternion.Euler(0f, 45f, -50f) * transform.up; // 첫 번째 빔ㅂ
        directions[1] = Quaternion.Euler(0f, 45f, -30f) * transform.up; 
        directions[2] = Quaternion.Euler(0f, 45f, -10f) * transform.up; 
        directions[3] = Quaternion.Euler(0f, 45f, 10f) * transform.up; 
        directions[4] = Quaternion.Euler(0f, 45f, 30f) * transform.up;; 
        directions[5] = Quaternion.Euler(0f, 45f, 50f) * transform.up;;

        for (int i = 0; i < numberOfLasers; i++)
        {
            // 빔 그리기
            lineRenderers[i].SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], laserRange);
            if (hit.collider != null)
            {
                lineRenderers[i].SetPosition(1, hit.point);
            }
            else
            {
                lineRenderers[i].SetPosition(1, transform.position + directions[i] * laserRange);
            }

            // 빔 깜빡임
            flickerTimers[i] -= Time.deltaTime;
            if (flickerTimers[i] <= 0)
            {
                lineRenderers[i].enabled = !lineRenderers[i].enabled;
                flickerTimers[i] = Random.Range(0.2f, 0.8f);
            }
        }
    }
}
```

5/15

```
using UnityEngine;
using UnityEngine.UI;

public class OutlineControl : MonoBehaviour
{
    public float range = 100f;
    private RaycastHit hitInfo, preHitInfo;
    private bool isHighlighted = false;

    public GameObject actionTextObject;
    private Text actionText;

    private void Start()
    {
        actionText = actionTextObject.GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isHighlighted)
            {
                ClearObjectHighlight(preHitInfo.transform);
                isHighlighted = false;
            }
            else if (preHitInfo.transform != null)
            {
                ShowObjectHighlight(preHitInfo.transform, 0);
                isHighlighted = true;
                ShowActiveText(preHitInfo.transform.tag);
            }
        }

        ShootRaycast();
    }

    private void ShowObjectHighlight(Transform parent, int idx)
    {
        cakeslice.Outline outLine = parent.GetComponent<cakeslice.Outline>();

        if (outLine != null)
        {
            outLine.enabled = true;
            outLine.eraseRenderer = false;
        }

        foreach (Transform child in parent)
        {
            ShowObjectHighlight(child, idx + 1);
        }
    }

    private void ClearObjectHighlight(Transform parent)
    {
        cakeslice.Outline outLine = parent.GetComponent<cakeslice.Outline>();

        if (outLine != null)
        {
            outLine.enabled = false;
            outLine.eraseRenderer = true;
        }

        foreach (Transform child in parent)
        {
            ClearObjectHighlight(child);
        }
    }

    private void ShootRaycast()
    {
        if (preHitInfo.transform != null && !isHighlighted)
        {
            ClearObjectHighlight(preHitInfo.transform);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range))
        {
            if (!isHighlighted)
            {
                ShowObjectHighlight(hitInfo.transform, 0);
            }
        }

        preHitInfo = hitInfo;
    }

    private void ShowActiveText(string tag)
    {
        if (string.Equals(tag, "Sunbed", System.StringComparison.OrdinalIgnoreCase))
        {
            actionText.text = "눕기 " + "<color=red>" + "(F)" + "</color>";
            actionTextObject.SetActive(true);
        }
    }
}
```

### 5/16

```
using UnityEngine;
using StarterAssets;
using Photon.Pun;
using System.Collections;

public class HealingInteraction : MonoBehaviourPunCallbacks
{
    [Tooltip("Common")]
    private Animator animator;
    private ThirdPersonController thirdPersonController;
    private CharacterController characterController;

    [Tooltip("Water")]
    public LayerMask waterLayer;
    private bool isSwimming = false;
    private bool wasTreadingWater = false;

    [Tooltip("SunBed")]
    public LayerMask sunbedLayer; // 썬베드 레이어
    private Sunbed currentSunbed;
    private bool isLayDown = false;
    private Vector3 originalPosition; // 썬베드에 누워있기 전의 원래 위치
    private Quaternion originalRotation; // 썬베드에 누워있기 전의 원래 회전
    public int currentSunbedID = -1; // 현재 누운 썬베드의 PhotonView ID

    [Tooltip("Fishing")]
    public LayerMask fishingLayer; // 썬베드 레이어
    private bool isFishing = false;
    private FishingItem currrentFisingItem;
    public int currentFinshingID = -1; // 현재 누운 썬베드의 PhotonView ID
    // PlayerOutlineControl 참조를 위한 필드 추가
    private PlayerOutlineControl playerOutlineControl;

    void Start()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        characterController = GetComponent<CharacterController>();
        // 성훈이가 추가한 부분
        MiniMap miniMap = FindObjectOfType<MiniMap>();
        if (miniMap != null)
        {
            miniMap.SetPlayer(transform);
        }
        playerOutlineControl = GetComponent<PlayerOutlineControl>(); // dkdntfkdls
        // 성훈이가 추가한 부분
    }

    void Update()
    {
        if (!photonView.IsMine) return; // 추가: 로컬 플레이어만 입력을 처리하도록 변경

        bool isTreadingWater = isSwimming && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0;
        if (isTreadingWater != wasTreadingWater)
        {
            animator.SetBool("IsTreadingWater", isTreadingWater);
            wasTreadingWater = isTreadingWater;
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Jab");
        }

        // F 키가 눌렸을 때 처리
        if (Input.GetKeyDown(KeyCode.F))
        {   // 썬 베드 상호작용
            if (currentSunbed != null)
            {
                if (!currentSunbed.IsOccupied)
                {
                    OnLayDownEnter();
                }
                else if(isLayDown)
                {
                    OnLayDownExit();
                }
            }
            // 낚시 상호작용
            if (currrentFisingItem != null)
            {
                if (!currrentFisingItem.IsSitting)
                {
                    OnFishingEnter();
                }
                else if(isFishing)
                {
                    OnFishingExit();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && other is BoxCollider)
        {
            OnSwimmingEnter();
        }

        if (((1 << other.gameObject.layer) & sunbedLayer) != 0)
        {
            currentSunbed = other.GetComponent<Sunbed>();
        }

        if (((1 << other.gameObject.layer) & fishingLayer) != 0)
        {
            currrentFisingItem = other.GetComponent<FishingItem>();
            currentFinshingID = currrentFisingItem.finshingID;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0 && other is BoxCollider)
        {
            OnSwimmingExit();
        }

        if (((1 << other.gameObject.layer) & sunbedLayer) != 0)
        {
            currentSunbed = null;
        }

        if (((1 << other.gameObject.layer) & fishingLayer) != 0)
        {
            currrentFisingItem = null;
            currentFinshingID = -1;
        }
    }

    void OnSwimmingEnter()
    {
        isSwimming = true;
        thirdPersonController.IsSwimming = true;
        animator.SetBool("IsSwimming", true);
    }

    void OnSwimmingExit()
    {
        isSwimming = false;
        thirdPersonController.IsSwimming = false;
        animator.SetBool("IsSwimming", false);
        animator.SetBool("IsTreadingWater", false);
    }

    private void OnLayDownEnter()
    {
        currentSunbed.photonView.RPC("Occupy", RpcTarget.All, photonView.ViewID);
        thirdPersonController.isSitting = true;
        characterController.enabled = false;
        originalRotation = transform.rotation;
        originalPosition = transform.position;
        isLayDown = true;

        animator.SetBool("IsLayDown", true);
        // sunbed의 LoungerPos의 위치로 이동

        // LoungerPos 오브젝트를 찾습니다.
        Transform loungerPos = currentSunbed.transform.Find("LoungerPos");
        if (loungerPos != null)
        {
            transform.position = loungerPos.position;
            transform.rotation = loungerPos.rotation;
        }
        // 아웃라인과 텍스트를 숨깁니다.
        playerOutlineControl.ClearOutline();

    }

    private void OnLayDownExit()
    {
        currentSunbed.photonView.RPC("Vacate", RpcTarget.All);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isLayDown = false;
        animator.SetBool("IsLayDown", false);
        // 원래 위치로 이동

        thirdPersonController.isSitting = false;
        characterController.enabled = true;

        // 아웃라인과 텍스트를 다시 표시합니다.
        playerOutlineControl.ShowActionText(transform);
    }

    private void OnFishingEnter()
    {

        currrentFisingItem.photonView.RPC("PickUp", RpcTarget.All, photonView.ViewID);


        // playerHands = transform.Find(rightHandBone);
        thirdPersonController.isSitting = true;
        characterController.enabled = false;

        isFishing = true;

        animator.SetTrigger("IsSitting");

        Transform fixedPos = currrentFisingItem.transform.Find("FixedPos");
        if (fixedPos != null)
        {
            transform.position = fixedPos.position;
            transform.rotation = fixedPos.rotation;
        }

        animator.SetBool("IsThrowing", true);
        // StartCoroutine(ThrowFishingPole());
    }

    private void OnFishingExit()
    {
        currrentFisingItem.photonView.RPC("Drop", RpcTarget.All);

        isFishing = false;
        animator.SetBool("IsThrowing", false);

        thirdPersonController.isSitting = false;
        characterController.enabled = true;

        animator.SetTrigger("IsStanding");

        currrentFisingItem = null;
    }

    // private IEnumerator ThrowFishingPole()
    // {
    //     yield return new WaitForSeconds(1f); // Adjust the delay if needed
    //     bool isThrowing = animator.GetBool("IsThrowing");

    //     if (!isThrowing)
    //     {
    //         animator.SetBool("IsThrowing", true);
    //         yield return new WaitForSeconds(10000f);
    //         animator.SetBool("IsThrowing", false);
    //     }
    // }

    public override void OnDisable()
    {
        base.OnDisable();

        if(isLayDown)
        {
            PhotonView pv = PhotonView.Find(currentSunbedID);
            pv.RPC("Vacate", RpcTarget.All);
        }

        if(isFishing)
        {
            PhotonView pv = PhotonView.Find(currentFinshingID);
            pv.RPC("Vacate", RpcTarget.All);
            currentFinshingID = -1;
        }
    }

    public override void OnLeftRoom()
    {
        if (isLayDown)
        {
            currentSunbed.photonView.RPC("Vacate", RpcTarget.All);
        }

        if (isFishing)
        {
            currrentFisingItem.photonView.RPC("Vacate", RpcTarget.All);
        }
    }
}
```
