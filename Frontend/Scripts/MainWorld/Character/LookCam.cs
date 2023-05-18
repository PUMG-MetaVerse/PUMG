using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    public GameObject Cam;

    void Start()
    {
        Cam = Camera.main.gameObject;
    }

    void Update()
    {
        transform.rotation = Cam.transform.rotation;
    }
}