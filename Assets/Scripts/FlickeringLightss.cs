using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light lightSource;          // Reference to the Light component
    public float minIntensity = 0.5f;  // Minimum intensity during flicker
    public float maxIntensity = 2f;    // Maximum intensity during flicker
    public float flickerSpeed = 0.1f;  
    public bool useRandomness = true;  

    private float targetIntensity; 

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        // Set the initial target intensity
        targetIntensity = lightSource.intensity;
    }

    void Update()
    {
        // Randomize the target intensity
        if (useRandomness)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
        else
        {
            targetIntensity = Mathf.PingPong(Time.time * flickerSpeed * maxIntensity, maxIntensity - minIntensity) + minIntensity;
        }


        lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, flickerSpeed * Time.deltaTime);
    }
}
