using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OldlockController : MonoBehaviour
{
    private Animator lockAnim;
    private bool isOpen = false;

    public bool cursorVisible;

    private PhotonView photonView;

    private AudioSource openCap, keyInsert;

    public Text escapeMessage; 

    private void Awake()
    {
        lockAnim = gameObject.GetComponentInChildren<Animator>();
        photonView = GetComponent<PhotonView>();

        var audios = gameObject.GetComponents<AudioSource>();

        if (audios.Length > 0)
        {
            openCap = audios[0];
            keyInsert = audios[1];
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            if (lockAnim.GetCurrentAnimatorStateInfo(0).IsName("OldLock_Cap_Anim")
            && lockAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isOpen = true;
                lockAnim.SetTrigger("opened");

                if (keyInsert != null)
                {
                    keyInsert.Play();
                    photonView.RPC("RPC_KeyInsertAudio", RpcTarget.All);
                }
            }
        }
        else
        {
            if (lockAnim.GetCurrentAnimatorStateInfo(1).IsName("OldLock_Key_Anim")
            && lockAnim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1.0f)
            {
                lockAnim.SetBool("isLock", true);
            }
        }
    }

    public void OnUnLock()
    {
        StartCoroutine(CorrectKey());
        GetComponent<Lock>().isSolved = true;
    }

    public void ShowOldlock()
    {
        gameObject.SetActive(true);
    }

    public void DismissOldlock()
    {
        escapeMessage.gameObject.SetActive(true);
        escapeMessage.color = new Color(escapeMessage.color.r, escapeMessage.color.g, escapeMessage.color.b, 1f);

        if (gameObject.name.Equals("OldLock_2F_1"))
        {
            escapeMessage.text = "2층 탈출 성공";
        }
        else if (gameObject.name.Equals("OldLock_1F_1"))
        {
            escapeMessage.text = "1층 탈출 성공";
        }
        else
        {
            escapeMessage.text = "마지막 잠금 해제";
        }

        StartCoroutine(FadeOutAfterDelay(1f));
    }

    [PunRPC]
    private void RPC_DeleteOldLock(int lockViewId)
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
        ActionController.RestoreMainCamera();
        Lock.lockSolvingActivated = false;
    }

    IEnumerator FadeOutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float fadeTime = 1f;
        float startAlpha = escapeMessage.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(escapeMessage.color.r, escapeMessage.color.g, escapeMessage.color.b, Mathf.Lerp(startAlpha, 0, t));
            escapeMessage.color = newColor;
            yield return null;
        }

        escapeMessage.color = new Color(escapeMessage.color.r, escapeMessage.color.g, escapeMessage.color.b, 0);

        // 모든 유저와 동기화
        int viewId = gameObject.GetComponent<PhotonView>().ViewID;
        photonView.RPC("RPC_DeleteOldLock", RpcTarget.All, viewId);

        QuitNumberlock();

        escapeMessage.gameObject.SetActive(false);
    }

    public void ToggleCursor()
    {
        cursorVisible = !cursorVisible;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    IEnumerator CorrectKey()
    {
        lockAnim.SetBool("isUnlock", true);

        if (openCap != null)
        {
            openCap.Play();
            photonView.RPC("RPC_OpenCapAudio", RpcTarget.All);
        }

        const float waitDuration = 7f;
        yield return new WaitForSeconds(waitDuration);

        DismissOldlock();
    }

    [PunRPC]
    void RPC_OpenCapAudio()
    {
        openCap.Play();
    }

    [PunRPC]
    void RPC_KeyInsertAudio()
    {
        keyInsert.Play();
    }
}

