using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeClockHours : MonoBehaviour
{
    private void Update()
    {
        float hoursDegree = ((EnviroSkyMgr.instance.GetCurrentHour() / 12.0f) * 360f) - 20f + (((EnviroSkyMgr.instance.GetCurrentMinute() / 60f) * 360f) / 12f);

        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(hoursDegree, gameObject.transform.rotation.y, gameObject.transform.rotation.z));
    }
}
