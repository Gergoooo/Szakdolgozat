using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldWheel : MonoBehaviour
{
    public GameObject leftWheel;  // Assign the left wheel object
    public GameObject rightWheel; // Assign the right wheel object

    public float leftWheelTargetAngle = 11f;  // Target angle for left wheel (X-axis)
    public float rightWheelTargetAngle = 172f; // Target angle for right wheel (X-axis)
    public float rotationDuration = 2f;  // Duration of the rotation
    public KeyCode rotateKey = KeyCode.G;  // Key to trigger the rotation

    private Quaternion leftWheelOriginalRotation;
    private Quaternion rightWheelOriginalRotation;
    private Quaternion leftWheelTargetRotation;
    private Quaternion rightWheelTargetRotation;
    private bool isRotating = false;
    private bool isAtTarget = false;
    private float startTime;

    void Start()
    {
        if (leftWheel != null)
        {
            // Store the original rotation of the left wheel
            leftWheelOriginalRotation = leftWheel.transform.localRotation;
            // Set the target rotation for the left wheel (90 to 11 on X-axis)
            leftWheelTargetRotation = Quaternion.Euler(leftWheelTargetAngle, leftWheel.transform.localEulerAngles.y, leftWheel.transform.localEulerAngles.z);
        }

        if (rightWheel != null)
        {
            // Store the original rotation of the right wheel
            rightWheelOriginalRotation = rightWheel.transform.localRotation;
            // Set the target rotation for the right wheel (90 to 172 on X-axis)
            rightWheelTargetRotation = Quaternion.Euler(rightWheelTargetAngle, rightWheel.transform.localEulerAngles.y, rightWheel.transform.localEulerAngles.z);
        }
    }

    void Update()
    {
        // Check if the rotate key (G) is pressed and we are not already rotating
        if (Input.GetKeyDown(rotateKey) && !isRotating)
        {
            isAtTarget = !isAtTarget;  // Toggle between target and original positions
            startTime = Time.time;  // Record the start time of the rotation
            isRotating = true;  // Start the rotation
        }

        if (isRotating)
        {
            float elapsed = Time.time - startTime;  // Calculate the time elapsed since the rotation started
            float progress = Mathf.Clamp01(elapsed / rotationDuration);  // Determine how far through the rotation we are (0 to 1)

            // Rotate the left wheel
            if (leftWheel != null)
            {
                // Lerp between the original and target rotations based on the progress
                Quaternion startRotation = isAtTarget ? leftWheelOriginalRotation : leftWheelTargetRotation;
                Quaternion endRotation = isAtTarget ? leftWheelTargetRotation : leftWheelOriginalRotation;
                leftWheel.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, progress);
            }

            // Rotate the right wheel
            if (rightWheel != null)
            {
                Quaternion startRotation = isAtTarget ? rightWheelOriginalRotation : rightWheelTargetRotation;
                Quaternion endRotation = isAtTarget ? rightWheelTargetRotation : rightWheelOriginalRotation;
                rightWheel.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, progress);
            }

            // Stop rotating when the transition is complete
            if (progress >= 1f)
            {
                isRotating = false;
            }
        }
    }
}
