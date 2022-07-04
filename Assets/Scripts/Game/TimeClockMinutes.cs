using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class TimeClockMinutes : MonoBehaviour
{        
    
    private void Update()
    {
        float minutesDegree = ((EnviroSkyMgr.instance.GetCurrentMinute() / 60.0f) * 360f) + 140.0f;        

        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(minutesDegree, gameObject.transform.rotation.y, gameObject.transform.rotation.z));
    }
}
