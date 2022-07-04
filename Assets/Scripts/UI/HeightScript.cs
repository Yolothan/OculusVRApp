using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class HeightScript : MonoBehaviour
{
    
    [SerializeField]
    private GameObject cameraOffset, vrPlayer;
    [SerializeField]
    private Text heightText;
    [SerializeField]
    private Slider heightSlider;      
    
    private float oldHeightCameraOffset;

    // Start is called before the first frame update
    void Start()
    {        
        heightText.text = System.Math.Round(1.72f + heightSlider.value, 2) + " m";
    }

    // Update is called once per frame
    public void SetHeight()
    {
        oldHeightCameraOffset = vrPlayer.transform.position.y;
        cameraOffset.transform.position = new Vector3(cameraOffset.transform.position.x, heightSlider.value + oldHeightCameraOffset, cameraOffset.transform.position.z);              
        heightText.text = System.Math.Round(1.72f + heightSlider.value, 2) + " m";
    }

    public void SetHeightChild()
    {
        heightSlider.value = -0.5f;
        SetHeight();
    }
    public void SetHeightTeenager()
    {
        heightSlider.value = -0.02f;
        SetHeight();
    }
    public void SetHeightAdult()
    {
        heightSlider.value = 0.2f;
        SetHeight();
    }
}
