﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESThirdPersonCarmera : MonoBehaviour
{
    public enum ModeType
    {
        FpsMode,
        ThirdPersonMode
    }
    public ModeType GetModeType = ModeType.ThirdPersonMode;
    public GameObject Target;
    public float FollowSpeed = 50f;
    //
    public float DistanceFromTarget = 4f;
    public float HeightFromTarget = 2f;
    public float RotationDamping = 1.5f;
    public float HeightDamping = 1.5f;
    public bool findbyname;
    public string Gameobjname;
    public enum LerpType
    {
        Linear,
        spherical
    }
    //
    public LerpType _lerptype = LerpType.spherical;
    // Update is called once per frame
    private void FixedUpdate()
    {
        //finds target by name gameobbject.
        if (Target == null && findbyname)
        {
            Target = GameObject.Find(Gameobjname);
        }
        //executes if target found
        if (Target != null)
        {
            /*
            calculation for camera position for 360 rot.
           */
            var targetangle = Target.transform.eulerAngles.y;
            var targetheight = Target.transform.position.y + HeightFromTarget;
            var camangle = transform.eulerAngles.y;
            var camheight = transform.position.y;
            camangle = Mathf.LerpAngle(camangle, targetangle, RotationDamping * Time.smoothDeltaTime);

            camheight = Mathf.Lerp(camheight, targetheight, HeightDamping * Time.smoothDeltaTime);

            var currentrotation = Quaternion.Euler(0f, camangle, 0f);
            Vector3 trans = Target.transform.position;
            trans -= currentrotation * Vector3.forward * DistanceFromTarget;
            if (_lerptype == LerpType.Linear)
            {
                Vector3 lerpposition = GetModeType == ModeType.ThirdPersonMode ?
                 Vector3.Lerp(transform.position, trans, FollowSpeed * Time.smoothDeltaTime) : Vector3.Lerp(transform.position, trans, FollowSpeed);
                transform.position = lerpposition;
            }
            else if (_lerptype == LerpType.spherical)
            {
                Vector3 lerpposition = GetModeType == ModeType.ThirdPersonMode ?
                 Vector3.Slerp(transform.position, trans, FollowSpeed * Time.smoothDeltaTime) : Vector3.Slerp(transform.position, trans, FollowSpeed);
                transform.position = lerpposition;
            }

            Vector3 tempposition = transform.position;
            tempposition.y = camheight;
            transform.position = tempposition;
            //make camera focus on  target
            transform.LookAt(Target.transform);
        }

    }
}