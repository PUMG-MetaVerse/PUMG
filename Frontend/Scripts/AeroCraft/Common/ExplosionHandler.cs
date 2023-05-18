using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public ParticleSystem explosion;
    public AudioSource audio;
    public string targetTag = "Target";
    public float explosionForce = 1200f;
    public float explosionRadius = 300f;
    public float upForce = 20f;

    public void Explode(Vector3 explosionPos)
    {
        explosion.transform.parent = null;
        explosion.transform.position = explosionPos;
        explosion.Play();
        audio.Play();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(targetTag))
        {
            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPos, explosionRadius, upForce);
        }

        // EffectRemover script = explosion.gameObject.GetComponent<EffectRemover>();
        // script.duration_Input = explosion.duration;
        // script.isStart_Input = true;
    }
}
