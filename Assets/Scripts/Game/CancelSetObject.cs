using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class CancelSetObject : MonoBehaviour
{
    private GameObject controllerObject;
    private XRController controller;
    private ScriptSpawnObject[] spawnAreas;    
    private HoldRaycasts holdRaycasts;
    private Painting painting;

    private void Start()
    {
        spawnAreas = FindObjectsOfType<ScriptSpawnObject>();        
        if (GameObject.Find("RightHandController") != null)
        {
            controllerObject = GameObject.Find("RightHandController");
            controller = controllerObject.GetComponent<XRController>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
            {
                if (isPressed)
                {
                    for (int i = 0; i < spawnAreas.Length; i++)
                    {
                        spawnAreas[i].gameObject.layer = spawnAreas[i].oldLayer;
                        XRSimpleInteractable interaction = spawnAreas[i].GetComponent<XRSimpleInteractable>();
                        Destroy(interaction);
                        if (spawnAreas[i].oldLayer == 11)
                        {
                            TeleportationArea ta = spawnAreas[i].gameObject.AddComponent<TeleportationArea>() as TeleportationArea;
                        }
                    }
                    holdRaycasts = FindObjectOfType<HoldRaycasts>();

                    for (int i = 0; i < holdRaycasts.raycasts.Length; i++)
                    {
                        holdRaycasts.raycasts[i].enabled = true;
                    }
                    painting = FindObjectOfType<SpawnObjects>().painting;
                    painting.PaintSpace();
                    Destroy(gameObject);
                }
            }
        }
    }
}
