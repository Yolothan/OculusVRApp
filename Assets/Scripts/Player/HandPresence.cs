using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    [SerializeField]
    private InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.None;
        
    public GameObject handModelPrefab = null;

    private InputDevice targetDevice;
    [HideInInspector]
    public GameObject spawnedHandModel = null, firstHandModel;
    private Animator handAnimator = null;
    private bool animationPresent = false;
    public UIScript uiScript;

    private void Start()
    {
        Tryinitialize();
        firstHandModel = spawnedHandModel;
        if (uiScript.testingTutorial)
        {
            firstHandModel.SetActive(false);
        }
    }
    
    // Initialize the hand controller/ model
    public void Tryinitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        // Set the hand model for the controller 
        if (devices.Count > 0)
        {
            targetDevice = devices[0];            
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            if (spawnedHandModel.GetComponent<Animator>() != null)
            {
                animationPresent = true;
                handAnimator = spawnedHandModel.GetComponent<Animator>();
            }
            else
            {
                animationPresent = false;
            }
        }
    }

    private void UpdateHandAnimation()
    {
        if (animationPresent)
        {
            // If the trigger is pushed, the animator will play.
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                handAnimator.SetFloat("Trigger", triggerValue);
            }
            else
            {
                handAnimator.SetFloat("Trigger", 0);
            }

            // If the gripbutton is pushed, the animator will play.
            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                handAnimator.SetFloat("Grip", gripValue);
            }
            else
            {
                handAnimator.SetFloat("Grip", 0);
            }
        }
    }

    private void Update()
    {
        // Initilize if the hand is not active else play the animations
        if (!targetDevice.isValid)
        {
            Tryinitialize();
        }
        else
        {
            UpdateHandAnimation();
        }
    }     
}
