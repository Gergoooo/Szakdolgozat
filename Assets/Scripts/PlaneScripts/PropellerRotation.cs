using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerRotation : MonoBehaviour
{
    public GameObject plane;

    private float speedMultiplier = 15f;
    private Rigidbody planeRigidbody;
    private float planeSpeed;


    void Start()
    {
        planeRigidbody = plane.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (planeRigidbody != null)
        {
            planeSpeed = planeRigidbody.velocity.magnitude;

            if (planeSpeed > 0)
            {
                float rotationSpeed = planeSpeed > 50f ? planeSpeed * speedMultiplier : 50f * speedMultiplier;
                transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
