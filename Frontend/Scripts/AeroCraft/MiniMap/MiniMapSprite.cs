using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiniMapSprite : MonoBehaviour
{
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90, 0, -target.eulerAngles.y);
    }
}