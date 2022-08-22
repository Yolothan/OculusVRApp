using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class RotateGameObject : MonoBehaviour
{
    private GameObject controllerObject;
    private XRController controller;
    
    
    [SerializeField]
    private float rotateSpeed = 2.0f;
    
    private void Start()
    {
        if (GameObject.Find("LeftHandController") != null)
        {
            controllerObject = GameObject.Find("LeftHandController");
            controller = controllerObject.GetComponent<XRController>();
        }
                  
        
    }
    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            {
                if ((position.x > position.y && position.x > 0.0f) || (position.x < position.y && position.x < 0.0f))
                {
                    if (position.x > 0.0f || position.x < 0.0f)
                    {
                        Vector3 rotation = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y + (position.x * rotateSpeed), gameObject.transform.localEulerAngles.z);
                        gameObject.transform.localRotation = Quaternion.Euler(rotation);
                    }
                }
            }
        }
        
    }
}
