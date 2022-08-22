using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine.XR.Interaction.Toolkit.UI;


public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI textCountDown;
    private GameObject wristUI;    
    public GameObject target, enviroSky, button1, button2;
    private float timer;
    private UIScript uiScript;
    private UI_Assistant assistant;
    private bool invokeOnce = true;
    
    public GameObject routeX, routeY;

    
    
    

    
    // Start is called before the first frame update
    void Start()
    {
        
        target.SetActive(false);
        enviroSky.SetActive(false);
        uiScript = FindObjectOfType<UIScript>();
        assistant = FindObjectOfType<UI_Assistant>();
        assistant.WriteText(assistant.message);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (invokeOnce)
        {
            timer += Time.deltaTime;
            float i = math.round(20 - timer);
            textCountDown.text = "(" + i + ")";

            if (i == 0)
            {
                if (target != null)
                {

                    target.SetActive(true);
                }

                ActivateFirstTask();
                assistant.WriteText("Druk op de triggers achterop de controller om te teleporteren. Probeer het groene licht te bereiken.");
                
                
                invokeOnce = false;


            }

        }
        
    }

    public void LoadNewLevel(string SceneName)
    {
        uiScript.allowInstantiate = false;
        uiScript.ReturnHome(SceneName);       
    }       
    public void ActivateFirstTask()
    {
       
        uiScript.ActivateFirstTask();
        button1.SetActive(false);
        button2.SetActive(false);
        gameObject.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = false;
        uiScript.allowedDeactivateTrigger = true;
        this.enabled = false;
    }
}
