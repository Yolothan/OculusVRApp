using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoScript : MonoBehaviour
{    
    public Renderer wheel1, wheel2, wheel3, wheel4;
    private EnviroSkyMgr enviroSkyMgr;
    public AudioSource source;
    private bool invokeOnce = false;
    private UIScript uiScript;
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "Tutorial")
        {
            this.enabled = false;
        }
        if (sceneName == "Tutorial")
        {
            GetComponent<MeshRenderer>().enabled = false;
            wheel1.enabled = false;
            wheel2.enabled = false;
            wheel3.enabled = false;
            wheel4.enabled = false;
            source.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (FindObjectOfType<EnviroSkyMgr>() != null && !invokeOnce)
        {
            enviroSkyMgr = FindObjectOfType<EnviroSkyMgr>();
            if (sceneName == "Tutorial" && enviroSkyMgr.GetCurrentWeatherPreset().name == "Rain")
            {
                GetComponent<MeshRenderer>().enabled = true;
                uiScript = FindObjectOfType<UIScript>();
                uiScript.ActivateThirdTask();
                wheel4.enabled = true;
                wheel3.enabled = true;
                wheel2.enabled = true;
                wheel1.enabled = true;
                source.enabled = true;
                invokeOnce = true;
                this.enabled = false;
            }
        }
    }
}
