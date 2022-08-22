using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    int i = 0;
    [SerializeField]
    UnityEngine.Video.VideoPlayer videoPlayer;
    [SerializeField]
    Canvas canvas;
    private Camera cameraObject;
    private GameObject mainCamera;
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        i = i + 1;
        mainCamera = GameObject.FindWithTag("MainCamera");
        cameraObject = mainCamera.GetComponent<Camera>();
        canvas.worldCamera = cameraObject;
        if(i == 100)
        {
            videoPlayer.Play();
        }

        if (i == 350)
        {            
            SceneLoader.Instance.LoadNewScene("Menu");
        }
    }
}
