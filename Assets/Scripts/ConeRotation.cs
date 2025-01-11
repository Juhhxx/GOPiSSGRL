using UnityEngine;

public class ConeRotation : MonoBehaviour
{
    public float rotationSpeed = 1f; 
    public float smoothness = 0.1f;  
    
    private float targetYRotation = 0f;
    private float currentYRotation = 0f; 
    void Start()
    {
        transform.rotation = Quaternion.Euler(20f, 0, -10f);
    }
    void Update()
    {
        SmoothRotateBackAndForth();
    }

    private void SmoothRotateBackAndForth()
    {
        targetYRotation = Mathf.PingPong(Time.time * rotationSpeed * 180f, 180f);

        currentYRotation = Mathf.Lerp(currentYRotation, targetYRotation, smoothness);

        transform.rotation = Quaternion.Euler(20f, -currentYRotation, -10f);
    }
}
