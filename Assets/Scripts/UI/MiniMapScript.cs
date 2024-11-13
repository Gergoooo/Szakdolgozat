using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform plane;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - plane.position;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = plane.position;
        newPosition.y = plane.position.y + 21f;
        transform.position = newPosition;
    }
}



