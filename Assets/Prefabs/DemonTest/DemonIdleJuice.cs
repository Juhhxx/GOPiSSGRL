using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public bool onlyHorizontal;
    public float floatHeight = 0.5f; // Height of the float movement
    public float floatSpeed = 1f; // Speed of the float movement
     private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }
    void Update()
    {
        LookAtPlayer();
        Floating();
    }
    private void LookAtPlayer()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = player.position - transform.position;
            if(onlyHorizontal)
            {
                directionToPlayer.y = 0; 
            }

            // Calculate the target rotation (only around the Y-axis)
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            Vector3 targetEulerAngles = targetRotation.eulerAngles;
            //targetEulerAngles.x = -90f;

            targetRotation = Quaternion.Euler(targetEulerAngles);

            // Smoothly rotate the object around its local axis
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void Floating()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Apply the floating effect to the object's position
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
