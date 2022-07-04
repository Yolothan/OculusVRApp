using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demoUIHelp : MonoBehaviour
{    
    private UIScript uiScript;
    // Start is called before the first frame update
    void Start()
    {
        uiScript = FindObjectOfType<UIScript>();
        
        uiScript.demoUI.enabled = true;
        
        uiScript.wristUI.SetActive(true);
        uiScript.carIntensity.gameObject.SetActive(true);
        uiScript.carIntensity.lateStart();
        uiScript.carIntensity.gameObject.SetActive(false);
        uiScript.wristUI.SetActive(false);
    }

}
