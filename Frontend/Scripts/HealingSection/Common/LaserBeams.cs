// 완성

using UnityEngine;

public class LaserBeams : MonoBehaviour
{
    public int numberOfLasers = 6;
    public float laserRange = 50f;
    public float laserStartWidth = 0.3f;
    public float laserEndWidth = 0.1f;
    public float startAngle = -50f; // 부채꼴 시작 각도
    public float endAngle = 50f; // 부채꼴 끝 각도
    public Color[] laserColors = { Color.blue, Color.green, Color.cyan, Color.blue, Color.green, Color.cyan };

    private LineRenderer[] lineRenderers;
    private float[] flickerTimers;

    void Start()
    {
        lineRenderers = new LineRenderer[numberOfLasers];
        flickerTimers = new float[numberOfLasers];
        float angleRange = endAngle - startAngle;

        for (int i = 0; i < numberOfLasers; i++)
        {
            GameObject laser = new GameObject($"Laser_{i}");
            laser.transform.SetParent(transform);
            laser.transform.localRotation = Quaternion.Euler(0f, 0f, startAngle + (angleRange / (numberOfLasers - 1)) * i);
            lineRenderers[i] = laser.AddComponent<LineRenderer>();
            lineRenderers[i].startWidth = laserStartWidth;
            lineRenderers[i].endWidth = laserEndWidth;
            lineRenderers[i].material = new Material(Shader.Find("Sprites/Default"));
            Color semiTransparentColor = laserColors[i % laserColors.Length];
            semiTransparentColor.a = 0.5f;
            lineRenderers[i].material.color = semiTransparentColor;
            lineRenderers[i].sortingOrder = 1;
            flickerTimers[i] = Random.Range(1f, 0.8f);
        }
    }


    void Update()
    {
        Vector3[] directions = new Vector3[numberOfLasers];
        directions[0] = Quaternion.Euler(0f, 45f, -50f) * transform.up; // 첫 번째 빔
        directions[1] = Quaternion.Euler(0f, 45f, -30f) * transform.up; 
        directions[2] = Quaternion.Euler(0f, 45f, -10f) * transform.up; 
        directions[3] = Quaternion.Euler(0f, 45f, 10f) * transform.up; 
        directions[4] = Quaternion.Euler(0f, 45f, 30f) * transform.up;; 
		directions[5] = Quaternion.Euler(0f, 45f, 50f) * transform.up;;

        for (int i = 0; i < numberOfLasers; i++)
        {
            // 빔 그리기
            lineRenderers[i].SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], laserRange);
            if (hit.collider != null)
            {
                lineRenderers[i].SetPosition(1, hit.point);
            }
            else
            {
                lineRenderers[i].SetPosition(1, transform.position + directions[i] * laserRange);
            }

            // 빔 깜빡임
            flickerTimers[i] -= Time.deltaTime;
            if (flickerTimers[i] <= 0)
            {
                lineRenderers[i].enabled = !lineRenderers[i].enabled;
                flickerTimers[i] = Random.Range(0.2f, 0.8f);
            }
        }
    }
}