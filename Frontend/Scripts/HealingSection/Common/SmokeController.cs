using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    private ParticleSystem smoke;
    private float timer;

    private void Start()
    {
        smoke = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 10) // 10초마다
        {
            smoke.Play(); // 연기 쏘기 시작
            Invoke("StopSmoke", 3); // 3초 후에 연기 멈추기
            timer = 0; // 타이머 재설정
        }
    }

    private void StopSmoke()
    {
        smoke.Stop(true, ParticleSystemStopBehavior.StopEmitting); // 연기 천천히 멈추기
    }
}
