using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class Aircraft_Racing_Controller : MonoBehaviour
{
    private string url = "http://k8b108.p.ssafy.io:6999/api/v1"; // 서버

    // private string url = "http://localhost:6999/api/v1"; // 서버

    //개발자 입력 정보
    public Aircraft_Info info;

    //추력 조절 변수
    float thrustThrottle;

    //미사일 발사 차례
    int FireIndex = 0;

    //미사일 생성 여부
    bool isInstantiate = false;

    //미사일 복제본
    GameObject[] cloneMissile;

    //미사일 타겟
    GameObject[] Target;

    //추력
    Vector3 Thrust;

    //디버그용 벡터 배열
    Vector3[] DebugForces = new Vector3[3];

    //타겟 트랜스폼
    Transform[] _Target;

    // 게임 종료 시 카운트 다운을위한 변수들
    private bool gameOver = false;
    private float countDown = 5.0f;

    // 시작 위치
    private Vector3 initialPosition;

    // 경고문 표시
    private bool isWarningActive = false;
    private float warningStartTime;
    Forces force;
    FireMode fire;

    // 타이머 시작 시간
    private float startTime;
    public GameObject gameOverPanel;
    public GameObject aircraft;
    public GameObject missionCompletePanel;

    // 조작 설명 패널 켜짐 여부
    public GameObject instructionPanel;
    public GameObject minimapCanvas;
    public GameObject keyBoardCanvas;

    void Start()
    {
        initialPosition = new Vector3(1000, 200, 100);
        transform.position = initialPosition;
        Debug.Log("비행기 아이디 : " + PlayerPrefs.GetInt("Idx"));
        startTime = Time.time;
        float elapsedTime = Time.time - startTime;
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime * 100) % 100);
        info.timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    void Update()
    {
        if (!instructionPanel.activeSelf)
        {
            VisualizeSteerWings();
            AfterburnerControl();
            AudioControl();
            CheckAltitudeWarning();
            MissileControl(fire);
            if (!gameOver)
            {
                UpdateTimerText();
            }

            CheckReStartKey();
        }
    }

    // 처음 위치로 가는 키
    void CheckReStartKey()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = initialPosition;
        }
    }

    void FixedUpdate()
    {
        AddForcesToWings();
        CameraEffect();
    }

    private IEnumerator PostRequest()
    {
        string json = JsonUtility.ToJson(
            new RankingInfo
            {
                userIdx = PlayerPrefs.GetInt("Idx"),
                score = -1,
                clearTime = info.timerText.text
            }
        );
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            // Content-Type 헤더를 설정합니다.
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 데이터를 업로드 핸들러에 할당합니다.
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));

            // 다운로드 핸들러를 할당합니다. 이것은 서버로부터의 응답을 처리합니다.
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 요청 보내기
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                // 오류 처리
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // 응답 처리
                aircraft.SetActive(false);
            }
        }
    }

    private void CheckAltitudeWarning()
    {
        if (info.rigid.position.y > 350 && !isWarningActive)
        {
            isWarningActive = true;
            warningStartTime = Time.time;
            info.warningText.gameObject.SetActive(true);
        }
        else if (info.rigid.position.y <= 350 && isWarningActive)
        {
            isWarningActive = false;
            info.warningText.gameObject.SetActive(false);
        }

        if (isWarningActive)
        {
            if (Time.time - warningStartTime >= 5.0f)
            {
                BlindGameInfo();
                ShowMissionEndPanel();
            }
        }
    }

    // 충돌 시
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "FinishLine")
        {
            gameOver = true;
            BlindGameInfo();
            url += "/flight/record-score";
            StartCoroutine(PostRequest());
            info.resultText.text = "미션 성공";
            info.resultText.color = new Color(0, 0, 1, 1);
            info.resultTimerText.text = "걸린 시간 : " + info.timerText.text;
            gameOverPanel.SetActive(true);
        }
        else
        {
            ExplosionHandler explosionHandler = GetComponent<ExplosionHandler>();
            explosionHandler.Explode(col.contacts[0].point);

            gameOver = true;
            BlindGameInfo();
            StartCoroutine(WaitAndDoSomething());
        }
    }

    IEnumerator WaitAndDoSomething()
    {
        yield return new WaitForSeconds(1.5f);
        ShowMissionEndPanel();
    }

    // 타이머 업데이트 함수
    void UpdateTimerText()
    {
        float elapsedTime = Time.time - startTime;
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime * 100) % 100);
        info.timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // 날개 받는 힘 계산 함수
    void AddForcesToWings()
    {
        Quaternion InputSteerAngle = Quaternion.Euler(
            20 * Input.GetAxis("Pitch") / (1 + info.rigid.velocity.magnitude * 0.1f),
            20 * Input.GetAxis("Yaw") / (1 + info.rigid.velocity.magnitude * 0.1f),
            20 * Input.GetAxis("Roll") / (1 + info.rigid.velocity.magnitude * 0.1f)
        );
        force = AerodynamicsForce(
            out DebugForces,
            InputSteerAngle,
            info.rigid,
            transform,
            info.airDensity,
            info.wingArea,
            info.wingLength,
            info.windRange
        );

        foreach (WheelCollider wheel in info.aircraftWheel)
        {
            wheel.motorTorque = Thrust.magnitude * 0.01f;
            wheel.brakeTorque = Input.GetAxis("DragPositive") * 20;
            wheel.steerAngle = 20 * Input.GetAxis("Yaw");
        }

        //추력
        thrustThrottle = Mathf.Clamp(thrustThrottle + Input.GetAxis("Thrust"), 100, 100); //추력 조절
        Thrust = transform.forward * thrustThrottle * info.ThrustScalar; //추력 연산 결과 (기체 앞방향을 바라보는 벡터 * 추력 조절 수치 * 추력 입력 정보)

        info.rigid.AddForce(
            force.Force + (info.rigid.velocity.magnitude >= 500 ? Vector3.zero : Thrust)
        );
        info.rigid.AddTorque(force.Torque);

        info.rigid.angularDrag = 1 + info.rigid.velocity.magnitude * 0.001f;
    }

    // 날개 조작 함수
    void VisualizeSteerWings()
    {
        info.Aileron[0].localRotation = Quaternion.Euler(
            Mathf.Clamp(20 * Input.GetAxis("Pitch") - 20 * Input.GetAxis("Roll"), -20, 20),
            14,
            0
        );
        info.Aileron[1].localRotation = Quaternion.Euler(
            Mathf.Clamp(20 * Input.GetAxis("Pitch") + 20 * Input.GetAxis("Roll"), -20, 20),
            -14,
            0
        );
    }

    // 추진 화력 함수
    void AfterburnerControl()
    {
        info.afterburner.startLifetime = Mathf.Clamp(
            Mathf.Round(info.rigid.velocity.magnitude) * 0.002f,
            0,
            0.2f
        );
    }

    // 카메라 이동 조종
    void CameraEffect()
    {
        GameObject CameraFixed;
        float SmoothTime = 0.125f;

        if (GameObject.FindWithTag("CameraFixed") == null)
        {
            CameraFixed = new GameObject();
            CameraFixed.tag = "CameraFixed";
            CameraFixed.transform.rotation = Quaternion.identity;
            CameraFixed.transform.position = transform.position;

            info.camera.transform.position = info.CameraTargetPos.position;
            info.camera.transform.SetParent(CameraFixed.transform);
        }
        else
        {
            CameraFixed = GameObject.FindGameObjectsWithTag("CameraFixed")[0];
        }

        CameraFixed.transform.position =
            transform.position + info.rigid.velocity * Time.fixedDeltaTime;
        CameraFixed.transform.rotation = Quaternion.Slerp(
            CameraFixed.transform.rotation,
            transform.rotation,
            SmoothTime
        );

        info.camera.fieldOfView = Mathf.Clamp(
            60 + (info.rigid.velocity.magnitude * 0.25f),
            60,
            100
        );
    }

    // // 미사일 발사
    void MissileControl(FireMode fireMode)
    {
        Vector3 targetPoint;
        Vector3 DebugPoint = Vector3.zero;
        if (FireIndex < info.missilePos.Length)
            _Target[FireIndex] = Target[FireIndex].GetComponent<Transform>();

        switch (fireMode)
        {
            case FireMode.forward:

                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = transform.position + transform.forward * 950;
                }

                if (FireIndex < info.missilePos.Length)
                    _Target[FireIndex].position = targetPoint;

                DebugPoint = targetPoint;
                break;
        }

        info.targetingUI.position = Camera.main.WorldToScreenPoint(DebugPoint);
    }

    // 게임 종료 시 결과 창

    void ShowMissionEndPanel()
    {
        info.resultTimerText.text = "걸린 시간 : " + info.timerText.text;
        gameOverPanel.SetActive(true);
        aircraft.SetActive(false);
    }

    void BlindGameInfo()
    {
        info.timerText.gameObject.SetActive(false);
    }

    void AudioControl()
    {
        info.source.clip = info.sound_1;
        info.source.loop = true;

        if (!info.source.isPlaying)
            info.source.Play();

        info.source.pitch = Mathf.Clamp(info.rigid.velocity.magnitude * 0.01f, 0, 1.5f);
    }

    Forces AerodynamicsForce(
        out Vector3[] AerodynamicsForces,
        Quaternion steerAngle,
        Rigidbody rigid,
        Transform transform,
        float airDensity,
        float wingArea,
        float wingLength,
        float windRange
    )
    {
        Vector3 Lift; //양력
        Vector3 Drag; //항력
        Vector3 Moment; //공기역학적 모멘트

        //공기역학적 힘 연산 유체 정보
        Vector3 wind = Vector3.one * UnityEngine.Random.Range(-windRange, windRange); //유체 속도 변동
        Vector3 airFlowVelocity =
            -rigid.velocity
            - Vector3.Cross(rigid.angularVelocity, transform.position - rigid.worldCenterOfMass)
            + wind; //유체 상대 속도 (-기체 속도 - 각속도와 질량 중심부터 회전 중심까지의 위치 벡터의 외적 (중심점을 기준으로한 등속 원운동 속도 (w×r)) + 유체 속도 변동)
        Vector3 localAirFlow = transform.InverseTransformDirection(airFlowVelocity); //로컬 좌표 기준 유체 상대 속도
        Vector3 airFlowDirection = transform.TransformDirection(localAirFlow.normalized); //월드 좌표 기준 유체 진행 방향

        float angleOfAttack = Mathf.Atan2(localAirFlow.y, -localAirFlow.z) * Mathf.Rad2Deg; //받음각
        float airMomentum = airDensity * localAirFlow.sqrMagnitude * wingArea; //공기역학적 힘에 영향을 주는 유체 운동량 (p_air = mv = ρv²∆tS) (∆t는 이후 미분시 약분되므로 생략)

        coefficient aerodynamicsCoe = new coefficient(angleOfAttack); //받음각에 따른 공기역학적 힘의 계수
        coefficient aerodynamicsCoe_Cos = new coefficient(90 - angleOfAttack); //코사인 기준 계수

        //양력
        Vector3 LiftDirection = Vector3.Cross(airFlowDirection, -transform.right); //양력 방향
        Lift = LiftDirection * 0.5f * aerodynamicsCoe.liftCoefficient * airMomentum; //양력 연산 결과

        //항력
        Vector3 DragDirection = airFlowDirection; //항력 방향
        Drag =
            DragDirection * 0.5f * aerodynamicsCoe.dragCoefficient * airMomentum
            + DragDirection
                * 0.5f
                * aerodynamicsCoe_Cos.dragCoefficient
                * airMomentum
                * Input.GetAxis("DragPositive")
                * 0.07f; // 항력 연산 결과

        //공기역학적 모멘트
        Moment =
            (
                -transform.right * new coefficient(steerAngle.x).momentCoefficient
                + -transform.up * new coefficient(steerAngle.y).momentCoefficient
                + transform.forward * new coefficient(steerAngle.z).momentCoefficient
            )
            * 0.5f
            * airMomentum
            * wingLength; //공기역학적 모멘트 연산 결과
        // Moment*=3;
        AerodynamicsForces = new Vector3[3] { Lift, Drag, Moment }; //공기역학적 힘 연산 결과 저장
        return new Forces(Lift + Drag, Moment); //연산 결과 반환
    }
}

[System.Serializable]
public struct Aircraft_Info
{
    [Header("UserControl")]
    public float ThrustScalar; //추력 스칼라값 입력
    public float windRange; //유체 속도 변동 범위

    [Header("Aerodynamics Parameter")]
    public float airDensity; //유체 밀도
    public float wingArea; //날개 넓이
    public float wingLength; //날개 길이

    [Header("SteerWIngs")]
    public Transform[] Aileron;
    public Transform[] Rudder;

    [Header("Component")]
    public Rigidbody rigid;
    public Camera camera;
    public ParticleSystem afterburner;
    public WheelCollider[] aircraftWheel;

    [Header("TransformInfo")]
    public Transform CameraTargetPos;

    [Header("MissileControl")]
    public GameObject missile;
    public Transform[] missilePos;

    [Header("UIControl")]
    public TMP_Text timerText;
    public TMP_Text resultTimerText;
    public TMP_Text resultText;
    public TMP_Text warningText;
    public RectTransform targetingUI;

    [Header("AudioControl")]
    public AudioSource source;
    public AudioClip sound_1;
}

public struct Forces
{
    public Vector3 Force;
    public Vector3 Torque;

    public Forces(Vector3 InputForce, Vector3 InputTorque)
    {
        Force = InputForce;
        Torque = InputTorque;
    }
}

public struct coefficient
{
    public float liftCoefficient;
    public float dragCoefficient;
    public float momentCoefficient;

    public coefficient(float angleOfAttack)
    {
        float AOA_Rad = angleOfAttack * Mathf.Deg2Rad;

        liftCoefficient = 0.8f * Mathf.Sin(2 * AOA_Rad);
        dragCoefficient = 0.8f * Mathf.Sin(2 * AOA_Rad - Mathf.PI * 0.5f) + 0.8f;
        momentCoefficient = -0.6f * Mathf.Sin(AOA_Rad * 0.5f);
    }
}

public enum ForceType
{
    Lift,
    Drag,
    Moment
}

public enum FireMode
{
    forward,
    follow
}

[System.Serializable]
public class RankingInfo
{
    public int userIdx;
    public int score;
    public string clearTime;
}
