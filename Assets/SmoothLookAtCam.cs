﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLookAtCam : MonoBehaviour
{
    public GameObject target;
    
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 5.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += target.transform.position.x * mouseSensitivity * Time.deltaTime;


        rotY = Mathf.Clamp(rotY, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(20, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}
