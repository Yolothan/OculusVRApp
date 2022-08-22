using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tutorial : MonoBehaviour
{    
    public GameObject starrySky, enviroSkyMgr, ui_Assistent;  
    private UI_Assistant assistant;
    private UIScript uiScript;

  



    private void OnTriggerEnter(Collider other)    
    {
        if(other.CompareTag("Player"))
        {                   
            enviroSkyMgr.SetActive(true);                           
            
            uiScript = FindObjectOfType<UIScript>();
            uiScript.ActivateSecondTask(); 
            ui_Assistent.SetActive(true);
            assistant = ui_Assistent.GetComponent<UI_Assistant>();
            assistant.WriteText("Druk op de linker X-knop om het menu te openen");
            uiScript.allowedDeactivateButton = true;

            Destroy(starrySky);
            Destroy(gameObject);
        }
    }
}
