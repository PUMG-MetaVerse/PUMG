using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody rigid;
    CapsuleCollider col;
    ParticleSystem afterburner;
    ParticleSystem explosion;
    TrailRenderer trail;
    AudioSource audio;

    Transform targetPos;
    GameObject obj;
    bool start;
    bool isHit = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        trail = GetComponent<TrailRenderer>();
        audio = transform.GetChild(2).GetComponent<AudioSource>();

        afterburner = transform.GetChild(1).GetComponent<ParticleSystem>();
        afterburner.Stop();

        explosion = transform.GetChild(2).GetComponent<ParticleSystem>();
        explosion.Stop();
    }

    void FixedUpdate()
    {
        AddVelocityAtMissile(start, 5f);
        RotationToTarget(start, targetPos);
    }

    public IEnumerator MissileStart(Vector3 startVelocity, Transform target, GameObject DeleteObj)
    {
        col.enabled = true;
        rigid.isKinematic = false;
        rigid.velocity += startVelocity;

        yield return new WaitForSeconds(0.3f);

        if (!isHit)
        {
            if (this != null)
            {
            transform.parent = null;
            rigid.useGravity = false;
            trail.emitting = true;
            afterburner.Play();
            targetPos = target;
            obj = DeleteObj;
            start = true;
            }
        }

        yield return new WaitForSeconds(5.0f);

        if (!isHit)
        {
            if (this != null)
            {
                Destroy(gameObject);
            Destroy(DeleteObj);

            }
        }
    }

    void AddVelocityAtMissile(bool isStart, float MissileVelocityM)
    {
        transform.position += isStart ? transform.up * MissileVelocityM : Vector3.zero;
    }

    void RotationToTarget(bool isStart, Transform target)
    {
        transform.up = isStart ? Vector3.Lerp((target.position - transform.position).normalized - transform.up, (target.position - transform.position).normalized, 0.8f) : transform.up;
    }

    void Explosion(Vector3 explosionPos, float explosionForce, float explosionRadius, float upForce, string targetTag)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(targetTag))
        {
            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPos, explosionRadius, upForce);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        isHit = true;
        explosion.transform.parent = null;
        explosion.transform.position = col.contacts[0].point;
        explosion.Play();
        audio.Play();

        Explosion(col.contacts[0].point, 1200f, 300f, 20f, "Target");

        EffectRemover script = explosion.gameObject.GetComponent<EffectRemover>();
        script.duration_Input = explosion.duration;
        script.isStart_Input = true;

        Destroy(obj);
        Destroy(gameObject);
    }
}
