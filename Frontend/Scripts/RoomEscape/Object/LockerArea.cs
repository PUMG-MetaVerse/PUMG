using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerArea : MonoBehaviour
{
    public GameObject locker;
    private Animator lockerAnim;
    private AudioSource lockerAudio;

    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        lockerAnim = locker.GetComponentInChildren<Animator>();
        lockerAudio = locker.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Gary_mesh") && !isPlaying)
        {
            isPlaying = true;
            lockerAnim.SetTrigger("active_locker");
            lockerAudio.Play();
        }
    }
}
