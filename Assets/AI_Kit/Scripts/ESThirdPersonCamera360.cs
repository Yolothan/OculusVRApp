﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESThirdPersonCamera360 : MonoBehaviour
{

    /*
    this scripts controls the third person camera movement;
    */
    public GameObject Target;
    public float DistanceFromTarget = 7f;
    public float FollowSpeed = 50f;
    public float Sensitivity = 5f;
    public float RotationDamping = 2.5f;
    public float HeightDamping = 2.5f;
    public bool findbyname;
    public string Gameobjname;
    public float ClampValue = 2f;
    private float HeightFromTarget, rottarget;
    private float lowerbound = 0;
    [SerializeField] private float mouseX;
    private float mouseY;

    // Update is called once per frame
    void FixedUpdate()
    {
        //finds target by name gameobbject.
        if (Target == null && findbyname)
        {
            Target = GameObject.Find(Gameobjname);
        }
        //get input from mouse X
        mouseX += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        //get input from mouse Y
        mouseY += Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        //clamps MOuse Y value.
        mouseY = Mathf.Clamp(mouseY, -ClampValue, lowerbound);
        //store mouse in a local variable rottarget
        rottarget = mouseX;
        //executes if target found
        if (Target != null)
        {
            /*
             calculation for camera position for 360 rot.
            */
            var targetangle = rottarget;
            HeightFromTarget = -mouseY;
            var targetheight = Target.transform.position.y + HeightFromTarget;

            var camangle = transform.eulerAngles.y;
            var camheight = transform.position.y;
            camangle = Mathf.LerpAngle(camangle, targetangle, RotationDamping * Time.deltaTime);
            camheight = Mathf.Lerp(camheight, targetheight, HeightDamping * Time.deltaTime);

            var currentrotation = Quaternion.Euler(0f, camangle, 0f);
            Vector3 trans = Target.transform.position;
            trans -= currentrotation * Vector3.forward * DistanceFromTarget;
            Vector3 lerpposition = Vector3.Lerp(transform.position, trans, FollowSpeed * Time.deltaTime);
            transform.position = lerpposition;
            Vector3 tempposition = transform.position;
            tempposition.y = camheight;
            transform.position = tempposition;
            //make camera focus on  target
            transform.LookAt(Target.transform);
        }

    }
}
