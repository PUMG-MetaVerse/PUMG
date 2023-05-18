using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NumberlockController : MonoBehaviour
{
    public List<GameObject> _rullers = new List<GameObject>(); 

    public bool isShowing;

    [Header("Your Inputs")]
    [SerializeField] private string yourCombination = "1234";
    private string playerCombi;
    private bool hasUnlocked;

    public bool cursorVisible;
    [SerializeField] private SimplePlayerController player = null;

    public delegate void OnDismiss();
    public OnDismiss onDismiss;

    private int _numberRuller = 0;
    private bool _isActveEmission = false;
    private int _scroolRuller = 0;
    private int _changeRuller = 0;
    public int[] _numberArray = { 0, 0, 0, 0 };
    public GameObject lockInfoMiniDesc;

    [Header("Unlock Events")]
    [SerializeField] private UnityEvent unlock = null;
    private Animator lockAnim;

    private int tempStart = 0;

    private PhotonView photonView;

    private AudioSource spinAudio;
    private AudioSource unlockAudio;

    void Awake()
    {
        lockAnim = gameObject.GetComponentInChildren<Animator>();

        var audios = gameObject.GetComponents<AudioSource>();
        if (audios.Length > 0)
        {
            //unlockAudio = audios[0];
            //spinAudio = audios[1];
            foreach (var audio in audios)
            {
                if (audio.clip != null)
                {
                    if (audio.clip.name == "Unlock")
                    {
                        unlockAudio = audio;
                    }
                    else if (audio.clip.name == "Lock_Spin")
                    {
                        spinAudio = audio;
                    }
                }
            }
        }

        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing)
        {
            // 최초 한번은 실행이 안되도록
            // 빌드 환경에서 발생하는 알 수 없는 에러로 인한 임시 코드
            if (tempStart == 0)
            {
                tempStart++;
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                //gameObject.GetComponent<BoxCollider>().enabled = true;
                QuitNumberlock();
            }

            MoveRulles();
            RotateRullers();
        }
    }

    public void ShowNumberlock()
    {
        isShowing = true;
        _isActveEmission = true;
        gameObject.SetActive(true);
    }

    public void DismissNumberlock()
    {
        // 모든 유저와 동기화
        int viewId = gameObject.GetComponent<PhotonView>().ViewID;
        photonView.RPC("RPC_DeleteNumberLock", RpcTarget.All, viewId);

        // 내 코드
        gameObject.SetActive(false);
        QuitNumberlock();
    }

    [PunRPC]
    private void RPC_DeleteNumberLock(int lockViewId)
    {
        PhotonView lockPhotonView = PhotonView.Find(lockViewId);

        if (lockPhotonView != null)
        {
            lockPhotonView.gameObject.SetActive(false);
            lockPhotonView.gameObject.GetComponent<Lock>().isSolved = true;
        }
    }

    public void QuitNumberlock()
    {
        // 자물쇠에 반짝임 효과 없애기
        for (int i = 0; i < _rullers.Count; i++)
        {
            _rullers[i].GetComponent<SpinVertical>()._isSelect = false;
            _rullers[i].GetComponent<SpinVertical>().BlinkingMaterial();
        }

        isShowing = false;
        _isActveEmission = false;

        // 모든 다른 플레이어들의 해당 락의 isOtherSolving 값을 false 로 변경
        int viewId = gameObject.GetComponent<PhotonView>().ViewID;
        photonView.RPC("RPC_SetIsOtherSolvingFalse", RpcTarget.All, viewId);

        ActionController.RestoreMainCamera();
        lockInfoMiniDesc.SetActive(false);
        Lock.lockSolvingActivated = false;
    }

    [PunRPC]
    private void RPC_SetIsOtherSolvingFalse(int itemPhotonViewID)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemPhotonViewID);

        if (itemPhotonView != null)
        {
            GameObject lockObject = itemPhotonView.gameObject;
            lockObject.GetComponent<Lock>().isOtherSolving = false;
        }
    }

    void UnlockNumberlock()
    {
        GetComponent<Lock>().isSolved = true;
        unlock.Invoke();
    }

    public void ToggleCursor()
    {
        player.enabled = cursorVisible;
        cursorVisible = !cursorVisible;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CheckCombination()
    {
        playerCombi = _numberArray[0].ToString("0") + _numberArray[1].ToString("0") + _numberArray[2].ToString("0") + _numberArray[3].ToString("0");
        print($"play: {playerCombi}");
        if (playerCombi == yourCombination)
        {
            if (!hasUnlocked)
            {
                StartCoroutine(CorrectCombination());
                hasUnlocked = true;
            }
        }
    }

    IEnumerator CorrectCombination()
    {
        lockAnim.SetBool("isUnlock", true);
        if (unlockAudio != null)
        {
            unlockAudio.Play();
            photonView.RPC("RPC_UnlockAudio", RpcTarget.All);
        }

        const float waitDuration = 1.2f;
        yield return new WaitForSeconds(waitDuration);

        gameObject.SetActive(false);
        UnlockNumberlock();

        DismissNumberlock();
    }

    void MoveRulles()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(1))
        {
            _isActveEmission = true;
            _changeRuller++;
            _numberRuller += 1;

            if (_numberRuller > 3)
            {
                _numberRuller = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(0))
        {
            _isActveEmission = true;
            _changeRuller--;
            _numberRuller -= 1;

            if (_numberRuller < 0)
            {
                _numberRuller = 3;
            }
        }

        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;

        for (int i = 0; i < _rullers.Count; i++)
        {
            if (_isActveEmission)
            {
                if (_changeRuller == i)
                {
                    _rullers[i].GetComponent<SpinVertical>()._isSelect = true;
                    _rullers[i].GetComponent<SpinVertical>().BlinkingMaterial();
                }
                else
                {
                    _rullers[i].GetComponent<SpinVertical>()._isSelect = false;
                    _rullers[i].GetComponent<SpinVertical>().BlinkingMaterial();
                }
            }
        }

    }

    void RotateRullers()
    {
        float mouseWheelMovement = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.W)|| mouseWheelMovement > 0f)
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(-_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] += 1;

            if (_numberArray[_changeRuller] > 9)
            {
                _numberArray[_changeRuller] = 0;
            }

            _rullers[_changeRuller].GetComponent<SpinVertical>().spinnerNumber = _numberArray[_changeRuller];
            CheckCombination();

            if (spinAudio != null)
            {
                spinAudio.Play();
                photonView.RPC("RPC_SpinAudio", RpcTarget.All);
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || mouseWheelMovement < 0f)
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] -= 1;

            if (_numberArray[_changeRuller] < 0)
            {
                _numberArray[_changeRuller] = 9;
            }

            _rullers[_changeRuller].GetComponent<SpinVertical>().spinnerNumber = _numberArray[_changeRuller];
            CheckCombination();

            if (spinAudio != null)
            {
                spinAudio.Play();
                photonView.RPC("RPC_SpinAudio", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RPC_SpinAudio()
    {
        spinAudio.Play();
    }

    [PunRPC]
    void RPC_UnlockAudio()
    {
        unlockAudio.Play();
    }
}
