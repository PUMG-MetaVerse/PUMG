using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMesh playerNameText;

    // public Image imageComponent;
    public SpriteRenderer spriteRenderer;

    // public TextMesh playerTitleText;
    void Start()
    {
        Debug.Log($"캐릭터 이름 : {photonView.Owner.NickName}");
        playerNameText.text = photonView.Owner.NickName;
        Photon.Realtime.Player player = photonView.Owner;

        // 커스텀 프로퍼티에서 타이틀을 가져옵니다.
        // 타이틀 문자열을 로드할 이미지의 경로로 사용합니다.
        string title = (string)player.CustomProperties["Title"];

        // Resources 폴더 내에 있는 이미지를 로드합니다.
        Sprite titleImage = Resources.Load<Sprite>(title);

        Debug.Log("title image : " + titleImage);
        // 이제 titleImage를 원하는 방식으로 사용할 수 있습니다.
        // 예를 들어, UnityEngine.UI.Image 컴포넌트의 sprite 프로퍼티를 설정할 수 있습니다:
        spriteRenderer.sprite = titleImage;
    }

    public void SetPlayerTitleImage(string title)
    {
        // Resources 폴더 내에 있는 이미지를 로드합니다.
        Sprite titleImage = Resources.Load<Sprite>(title);

        // 이미지를 갱신합니다.
        spriteRenderer.sprite = titleImage;
    }
    [PunRPC]
public void SetTitle(string newTitle)
{
    // 칭호를 변경하는 로직을 여기에 작성합니다.
    string title = newTitle;
    Sprite titleImage = Resources.Load<Sprite>(title);
    this.spriteRenderer.sprite = titleImage;
}
}
