using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAeroCraft : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 1f;
    public Vector3 direction = Vector3.up;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + direction * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
