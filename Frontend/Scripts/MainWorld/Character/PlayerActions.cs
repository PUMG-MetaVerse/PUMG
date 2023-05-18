using UnityEngine;
using Cinemachine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class PlayerActions : MonoBehaviour
{
    public ParticleSystem[] magicParticlePrefab;
    public float magicDuration = 1f; // 추가한 지속 시간 변수
    public GameObject[] weapons;
    RawImage[] weaponSlots;  // 각 무기 슬롯의 RawImage 컴포넌트에 대한 참조를 저장합니다.
    public AudioClip weapon2_swingSound;
    public AudioClip weapon3_swingSound;
    private AudioSource audioSource;
    

    
    
    GameObject nearObject;
    GameObject equipWeapon;
    public float speed;
    private PhotonView photonView;
    bool sDownDefault;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isSwap;
    bool isSkill;
    int equipWeaponIndex = -1;
    int weaponIndex = -1;

    private Animator animator;
    private Transform playerTransform;
    private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;

    void Start()
    {
        // magicParticlePrefab = GetComponents<ParticleSystem>( );
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        weaponSlots = new RawImage[3];
        audioSource =  GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = weapon2_swingSound;
        for(int i=0; i<4; i++)
        {
            GameObject slotObject = GameObject.Find("Weapon" + i);
            if(slotObject != null)
            {
                weaponSlots[i] = slotObject.GetComponent<RawImage>();
            }

        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        // if (Input.GetMouseButton(1))
        // {
        //     ShootMagic();
        // }
        GetInput();
        if(!isSwap&&!isSkill)
        {
            Swap();
            // if(weaponIndex==-1)
            //     {
            //         if(Input.GetButtonDown("Fire1"))
            //         {
            //             FirePortal(0, transform.position, transform.forward, 250.0f);
            //         }
            //         else if (Input.GetButtonDown("Fire2"))
            //         {
            //             FirePortal(1, transform.position, transform.forward, 250.0f);
                        
            //         }
            //     }
            // if (Input.GetMouseButtonDown(1))
            // {
            //     // ShootMagic();
                
            //     if(weaponIndex==0)
            //     {
            //         ShootGun();    
            //     }
            //     else if(weaponIndex==1 || weaponIndex==2)
            //     {
            //         animator.SetTrigger("IsAttack"); // 먼저 공격 애니메이션을 시작합니다.
            //         StartCoroutine(DelayedMagic());  // 그 후에 딜레이를 두고 마법을 쏘게 됩니다.
            //     }
            // }
            if (Input.GetMouseButtonDown(1))
            {
                if(weaponIndex==0)
                {
                    photonView.RPC("ShootGun", RpcTarget.AllBuffered);   
                }
                else if(weaponIndex==1 || weaponIndex==2)
                {
                    photonView.RPC("TriggerAttackAnimation", RpcTarget.AllBuffered);
                    StartCoroutine(DelayedMagic());
                }
            }
        }
        
    }

    void GetInput()
    {
        sDownDefault = Input.GetKeyDown(KeyCode.Alpha1);
        sDown1 = Input.GetKeyDown(KeyCode.Alpha2);
        sDown2 = Input.GetKeyDown(KeyCode.Alpha3);
        // sDown3 = Input.GetKeyDown(KeyCode.Alpha4);    
    }

    void Swap()
    {
        // int weaponIndex = -1;
        if(sDownDefault && equipWeaponIndex == -1 )return;
        if(sDown1 && equipWeaponIndex == 1) return;
        if(sDown2 && equipWeaponIndex == 2) return;
        // if(sDown3 && equipWeaponIndex == 2) return;
        if(sDownDefault)
        {
            SelectWeapon(0);
            equipWeapon.SetActive(false);
            weaponIndex = -1;
            equipWeaponIndex = -1;
            animator.SetBool("IsGun",false);
        }
        if(sDown1) {
            // SelectWeapon(1);
            // weaponIndex = 0;
            // animator.SetBool("IsGun",true);
            SelectWeapon(1);
            weaponIndex = 1;
            animator.SetBool("IsGun",false);
        }
        if(sDown2) {
            // SelectWeapon(2);
            // weaponIndex = 1;
        // animator.SetBool("IsGun",false);
            SelectWeapon(2);
            weaponIndex = 2;
            animator.SetBool("IsGun",false);
        }
        // if(sDown3) {
        //     SelectWeapon(3);
        //     weaponIndex = 2;
        //     animator.SetBool("IsGun",false);
        // }
        if((sDown1 || sDown2))
        {
            // if(equipWeapon != null)
            //     equipWeapon.SetActive(false);
            // equipWeapon = weapons[weaponIndex];
            // equipWeaponIndex = weaponIndex;
            // equipWeapon.SetActive(true);
            // animator.SetTrigger("doSwap");
            // isSwap = true;
            // Invoke("SwapOut",0.4f);
            photonView.RPC("ChangeWeapon", RpcTarget.AllBuffered, weaponIndex);
            photonView.RPC("TriggerSwapAnimation", RpcTarget.AllBuffered);
            // animator.SetTrigger("doSwap");
            isSwap = true;
            Invoke("SwapOut",0.4f);
        }
    }
    void SwapOut()
    {
        isSwap = false;
    }
    void SkillOut()
    {
        isSkill = false;
    }
    [PunRPC]
    IEnumerator DelayedMagic()
    {
        
        yield return new WaitForSeconds(0.6f);
        // photonView.RPC("ShootMagic", RpcTarget.AllBuffered);
        ShootMagic();

        audioSource.Play();
    }
    [PunRPC]
    void ChangeWeapon(int weaponIndex)
    {
        
        if(equipWeapon != null)
            equipWeapon.SetActive(false);
        equipWeapon = weapons[weaponIndex];
        equipWeaponIndex = weaponIndex;
        PhotonView pv = equipWeapon.GetComponent<PhotonView>();
        if (pv != null)
        {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        equipWeapon.SetActive(true);
    }
    [PunRPC]
    void TriggerSwapAnimation()
    {
        animator.SetTrigger("doSwap");
    }

     [PunRPC]
    void TriggerAttackAnimation()
    {
        animator.SetTrigger("IsAttack");
        isSkill = true;
    }

    // [PunRPC]
    void ShootGun()
    {
        Vector3 particlePosition = playerTransform.position + mainCamera.transform.forward * 1f + Vector3.up * 1f;
        Quaternion particleRotation = Quaternion.LookRotation(mainCamera.transform.forward);
        isSkill = true;
        // animator.SetTrigger("Magic");
        GameObject magicInstance = PhotonNetwork.Instantiate(magicParticlePrefab[weaponIndex].name, particlePosition, particleRotation);
        StartCoroutine(DestroyParticleAfterSeconds(magicInstance, 10f));
        Invoke("SkillOut",0.4f);
// PhotonNetwork.Destroy(magicInstance.gameObject, 10f);

    }

    [PunRPC]
    void ShootMagic()
    {
        
        Vector3 particlePosition = playerTransform.position + mainCamera.transform.forward * 1f + Vector3.up * 1f;
        Quaternion particleRotation = Quaternion.LookRotation(mainCamera.transform.forward);
        
        // animator.SetTrigger("Magic");
        // particleRotation *= Quaternion.Euler(0, 0, 0); 
        GameObject magicInstance = PhotonNetwork.Instantiate(magicParticlePrefab[weaponIndex].name, particlePosition, particleRotation);
        // PhotonNetwork.Destroy(magicInstance, magicDuration);
        StartCoroutine(DestroyParticleAfterSeconds(magicInstance, magicDuration));
        Invoke("SkillOut",0.4f);
    }
    IEnumerator DestroyParticleAfterSeconds(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(instance);
    }
    [PunRPC]
    public void ApplyKnockback(Vector3 knockback)
    {
        knockback /= 3.0f;
        StartCoroutine(KnockbackRoutine(knockback, 0.5f));
    }
    
    // private IEnumerator KnockbackRoutine(Vector3 knockback ,float duration)
    // {
    //     var controller = GetComponent<CharacterController>();
    //     Debug.Log("피격됨");
    //     float elapsed = 0;
    //     Vector3 startPosition = transform.position;
        
    //     // 충격 방향으로 대각선 방향으로 이동
    //     Vector3 jumpDirection = knockback.normalized * knockback.magnitude * 0.25f + transform.up * 1f;

    //     while (elapsed < duration)
    //     {          
    //         // 이동 방향을 충격 방향으로 설정
    //         Vector3 moveDirection = knockback.normalized;

    //         // 점프하는 방향과 이동하는 방향의 합을 사용하여 이동
    //         controller.Move((moveDirection + jumpDirection) * knockback.magnitude * Time.deltaTime / duration);

    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }
    // }

    private IEnumerator KnockbackRoutine(Vector3 knockback, float duration)
    {
        var controller = GetComponent<CharacterController>();
        float elapsed = 0;

        Vector3 jumpDirection = knockback.normalized * knockback.magnitude * 0.25f + transform.up * 1f;
        Vector3 originalPosition = transform.position;

        while (elapsed < duration)
        {
            Vector3 moveDirection = knockback.normalized;
            controller.Move((moveDirection + jumpDirection) * knockback.magnitude * Time.deltaTime / duration);
            if (photonView.IsMine)
            {
                photonView.RPC("SynchronizePosition", RpcTarget.Others, transform.position);
            }
            elapsed += Time.deltaTime;
            yield return 0.1f;
        }

        // Synchronize position
        if (photonView.IsMine)
        {
            photonView.RPC("SynchronizePosition", RpcTarget.Others, transform.position);
        }
    }

    [PunRPC]
    void SynchronizePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    private void SelectWeapon(int index)
    {
        // 모든 슬롯의 투명도를 낮춥니다.
        foreach (RawImage weaponSlot in weaponSlots)
        {
            Color color = weaponSlot.color;
            color.a = 0.88f;  // 225 / 255
            weaponSlot.color = color;
        }

        // 선택된 슬롯의 투명도를 높입니다.
        Color selectedColor = weaponSlots[index].color;
        selectedColor.a = 1f;
        weaponSlots[index].color = selectedColor;
    }
}