using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WordlockController : MonoBehaviour
{
    public List<GameObject> _rullers = new List<GameObject>();

    private bool isShowing;

    [Header("Your Inputs")]
    [SerializeField] private string yourCombination = "ABC";
    private string playerCombi;
    private bool hasUnlocked;

    public bool cursorVisible;
    //[SerializeField] private SimplePlayerController player = null;

    public delegate void OnDismiss();
    public OnDismiss onDismiss;

    private int _numberRuller = 0;
    private bool _isActveEmission = false;
    private int _scroolRuller = 0;
    private int _changeRuller = 0;
    public char[] _charArray = { 'A', 'A', 'A' };
    public GameObject lockInfoMiniDesc;

    [Header("Unlock Events")]
    [SerializeField] private UnityEvent unlock = null;
    private Animator lockAnim;

    private PhotonView photonView;

    private AudioSource spinAudio;
    private AudioSource unlockAudio;

    private void Awake()
    {
        lockAnim = gameObject.GetComponentInChildren<Animator>();

        var audios = gameObject.GetComponents<AudioSource>();
        if (audios.Length > 0)
        {
            unlockAudio = audios[0];
            spinAudio = audios[1];
        }

        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //gameObject.GetComponent<BoxCollider>().enabled = true;
                QuitWordlock();
            }

            MoveRulles();
            RotateRullers();
        }
    }

    public void ShowWordlock()
    {
        isShowing = true;
        _isActveEmission = true;
        gameObject.SetActive(true);
    }

    public void DismissWordlock()
    {
        // 모든 유저와 동기화
        int viewId = gameObject.GetComponent<PhotonView>().ViewID;
        photonView.RPC("RPC_DeleteWordLock", RpcTarget.All, viewId);

        gameObject.SetActive(false);
        QuitWordlock();
    }

    [PunRPC]
    private void RPC_DeleteWordLock(int lockViewId)
    {
        PhotonView lockPhotonView = PhotonView.Find(lockViewId);

        if (lockPhotonView != null)
        {
            lockPhotonView.gameObject.SetActive(false);
            lockPhotonView.gameObject.GetComponent<Lock>().isSolved = true;
        }
    }

    public void QuitWordlock()
    {
        // 자물쇠에 반짝임 효과 없애기
        for (int i = 0; i < _rullers.Count; i++)
        {
            _rullers[i].GetComponent<WordSpinVertical>()._isSelect = false;
            _rullers[i].GetComponent<WordSpinVertical>().BlinkingMaterial();
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

    void UnlockWordlock()
    {
        GetComponent<Lock>().isSolved = true;
        unlock.Invoke();
    }

    public void ToggleCursor()
    {
        //player.enabled = cursorVisible;
        cursorVisible = !cursorVisible;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CheckCombination()
    {
        playerCombi = _charArray[0].ToString() + _charArray[1].ToString() + _charArray[2].ToString();
        
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
        UnlockWordlock();

        DismissWordlock();
    }

    void MoveRulles()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(1))
        {
            _isActveEmission = true;
            _changeRuller++;
            _numberRuller += 1;

            if (_numberRuller > 2)
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
                _numberRuller = 2;
            }
        }
        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;


        for (int i = 0; i < _rullers.Count; i++)
        {
            if (_isActveEmission)
            {
                if (_changeRuller == i)
                {

                    _rullers[i].GetComponent<WordSpinVertical>()._isSelect = true;
                    _rullers[i].GetComponent<WordSpinVertical>().BlinkingMaterial();
                }
                else
                {
                    _rullers[i].GetComponent<WordSpinVertical>()._isSelect = false;
                    _rullers[i].GetComponent<WordSpinVertical>().BlinkingMaterial();
                }
            }
        }

    }

    void RotateRullers()
    {
        float mouseWheelMovement = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.W) || mouseWheelMovement > 0f)
        {
            _isActveEmission = true;
            _scroolRuller = 60;
            _rullers[_changeRuller].transform.Rotate(-_scroolRuller, 0, 0, Space.Self);

            _charArray[_changeRuller] = (char)(_charArray[_changeRuller] + 1);

            if (_charArray[_changeRuller] > 70)
            {
                _charArray[_changeRuller] = (char)65;
            }

            _rullers[_changeRuller].GetComponent<WordSpinVertical>().spinnerNumber = _charArray[_changeRuller];
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
            _scroolRuller = 60;
            _rullers[_changeRuller].transform.Rotate(_scroolRuller, 0, 0, Space.Self);

            _charArray[_changeRuller] = (char)(_charArray[_changeRuller] - 1);

            if (_charArray[_changeRuller] < 65)
            {
                _charArray[_changeRuller] = (char)70;
            }

            _rullers[_changeRuller].GetComponent<WordSpinVertical>().spinnerNumber = _charArray[_changeRuller];
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
