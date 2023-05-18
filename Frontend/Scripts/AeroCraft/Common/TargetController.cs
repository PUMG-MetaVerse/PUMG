using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TargetController : MonoBehaviour
{
    public Vector3 moveRange = new Vector3(1700, 1000, 1700);
    public int scoreValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "missile")
        {
            TargetScoreManager.Instance.AddScore(scoreValue);
            Destroy(other.gameObject);
            MoveToRandomPosition();
        }
    }

    private void MoveToRandomPosition()
    {
        Vector3 newPosition = new Vector3(
            Random.Range(300, moveRange.x),
            Random.Range(300, moveRange.y),
            Random.Range(300, moveRange.z)
        );
        Debug.Log(newPosition);
        transform.position = newPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 newPosition = new Vector3(
            Random.Range(300, moveRange.x),
            Random.Range(300, moveRange.y),
            Random.Range(300, moveRange.z)
        );
        Debug.Log(newPosition);
        transform.position = newPosition;
    }

}
