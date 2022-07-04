using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLignt : MonoBehaviour
{
    EnviroSkyMgr enviroSky;
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        enviroSky = FindObjectOfType<EnviroSkyMgr>();        
    }

    private void Update()
    {
        if(EnviroSkyMgr.instance.Time.Hours > 17 || EnviroSkyMgr.instance.Time.Hours < 7)
        {            
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
    }

}
