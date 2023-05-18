using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartControllerRace : MonoBehaviour
{
        public GameObject instructionPanel; // 조작키 설명 패널을 연결합니다.
        public GameObject targetImage; // 조작키 설명 패널을 연결합니다.
        public GameObject KeyBoardCanvas; // 조작키 설명 패널을 연결합니다.
        public GameObject minimapCanvas; // 조작키 설명 패널을 연결합니다.

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
         // 엔터 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Return))
        {
            targetImage.SetActive(true);
            KeyBoardCanvas.SetActive(true);
            minimapCanvas.SetActive(true);
            // 패널을 비활성화합니다.
            Time.timeScale = 1;
            instructionPanel.SetActive(false);
        }
    }
}
