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
