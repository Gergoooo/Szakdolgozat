using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAndLookAtTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null) return;

        transform.position = new Vector3(target.position.x, target.position.y + 20f, target.position.z);

        Vector3 targetRotation = target.eulerAngles;
        transform.rotation = Quaternion.Euler(90, targetRotation.y, 0);
    }
}

