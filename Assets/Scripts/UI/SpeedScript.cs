using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedScript : MonoBehaviour
{
    public Rigidbody planeRigidbody;

    [SerializeField] private TMP_Text SpeedText;

    void Update()
    {
       SpeedText.text  = (planeRigidbody.velocity.magnitude * 3.6f).ToString("0") + (" km/h");
    }
}

