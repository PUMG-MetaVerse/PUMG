using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform target;
    public float offsetRatio;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 targetForwardVector = target.forward;
        targetForwardVector.y = 500;
        targetForwardVector.Normalize();

        Vector3 position = new Vector3(target.transform.position.x, 500, target.transform.position.z+300)
                           + targetForwardVector * offsetRatio * cam.orthographicSize;
        transform.position = position;
        transform.eulerAngles = new Vector3(90, 0, -target.eulerAngles.y);
    }
}
