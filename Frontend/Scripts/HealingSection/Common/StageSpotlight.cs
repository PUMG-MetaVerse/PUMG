using UnityEngine;

public class StageSpotlight : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float spotAngle = 30f;
    public float spotlightRange = 50f;

    private Light spotlight;

    void Start()
    {
        spotlight = gameObject.AddComponent<Light>();
        spotlight.type = LightType.Spot;
        spotlight.spotAngle = spotAngle;
        spotlight.range = spotlightRange;
        spotlight.color = Color.white;
    }

    void Update()
    {
        if (spotlight == null) return;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
