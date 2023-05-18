using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material skyboxDay;
    public Material skyboxNight;
    public float dayDuration = 60.0f; // 낮의 길이를 초 단위로 설정합니다.
    
    private float timeElapsed;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = Mathf.Sin(2 * Mathf.PI * timeElapsed / dayDuration);
        RenderSettings.skybox.Lerp(skyboxDay, skyboxNight, (t + 1) / 2);
    }
}
