using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;



public class SpawnObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObjectPrefab;

    [SerializeField]
    private XRController controller;
    

    [SerializeField]
    private HoldRaycasts raycastHolder;
    [SerializeField]
    private XRInteractorLineVisual interactorLineVisual;    

    private TeleportationArea teleportationArea;    
    
    [SerializeField]
    private RemoveAllObjects erase;
    
    public Painting painting;
    private ScriptSpawnObject[] floors;
    [HideInInspector]
    public bool isSpawningMaterial = false;

    private ScriptSpawnObject[] spawnAreas;
    
    private HoldRaycasts holdRaycasts;

    
    

    // Start is called before the first frame update
    public void SpawnObject()
    {
        erase.DisallowErase();        
       
       

        floors = FindObjectsOfType<ScriptSpawnObject>();
        for (int i = 0; i < floors.Length; i++)
        {
            XRSimpleInteractable interactable;
            teleportationArea = floors[i].GetComponent<TeleportationArea>();
            Destroy(teleportationArea);
            ScriptSpawnObject spawnObject = floors[i].GetComponent<ScriptSpawnObject>();
            if (floors[i].GetComponent<XRSimpleInteractable>() == null)
            {
                interactable = floors[i].gameObject.AddComponent<XRSimpleInteractable>() as XRSimpleInteractable;

                interactable.selectExited.AddListener(delegate { spawnObject.SetObject(); });
            }
            else
            {
                interactable = floors[i].gameObject.GetComponent<XRSimpleInteractable>();
                interactable.selectExited.AddListener(delegate { spawnObject.SetObject(); });
            }

            floors[i].gameObject.layer = 6;
                
        }

        GameObject go = Instantiate(spawnObjectPrefab);
        GameObject parentGo = interactorLineVisual.reticle;
        go.transform.parent = parentGo.transform;
        go.transform.localPosition = new Vector3(0, 0, 0);
        Vector3 rotation = new Vector3(0, 0, 0);
        go.transform.localRotation = Quaternion.Euler(rotation);        

        for(int i = 0; i < raycastHolder.raycasts.Length; i++)
        {
            raycastHolder.raycasts[i].enabled = false;
        }
    }

    public void SpawnMaterial(Material material)
    {
        isSpawningMaterial = true;
        erase.DisallowErase();

        floors = FindObjectsOfType<ScriptSpawnObject>();
        for (int i = 0; i < floors.Length; i++)
        {
            XRSimpleInteractable interactable;
            teleportationArea = floors[i].GetComponent<TeleportationArea>();
            Destroy(teleportationArea);
            floors[i].preventExit = false;
            ScriptSpawnObject spawnObject = floors[i].GetComponent<ScriptSpawnObject>();
            if (floors[i].GetComponent<XRSimpleInteractable>() == null)
            {
                interactable = floors[i].gameObject.AddComponent<XRSimpleInteractable>() as XRSimpleInteractable;
            }
            else
            {
                interactable = floors[i].gameObject.GetComponent<XRSimpleInteractable>();
            }

            interactable.hoverEntered.AddListener(delegate { spawnObject.HoverMaterial(); });

            interactable.hoverExited.AddListener(delegate { spawnObject.ExitMaterial(); });            

            interactable.selectExited.AddListener(delegate { spawnObject.SetMaterial(); });           

            interactorLineVisual.reticle.GetComponent<Renderer>().sharedMaterial = material;

            floors[i].gameObject.layer = 6;
        }
        for (int i = 0; i < raycastHolder.raycasts.Length; i++)
        {
            raycastHolder.raycasts[i].enabled = false;
        }
    }

    private void Update()
    {
        if (isSpawningMaterial)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
            {
                if (isPressed)
                {
                    spawnAreas = FindObjectsOfType<ScriptSpawnObject>();
                    for (int i = 0; i < spawnAreas.Length; i++)
                    {
                        if (spawnAreas[i].gameObject.GetComponent<TeleportationArea>() == null)
                        {
                            spawnAreas[i].gameObject.layer = spawnAreas[i].oldLayer;
                            XRSimpleInteractable interaction = spawnAreas[i].GetComponent<XRSimpleInteractable>();
                            Destroy(interaction);
                            if (spawnAreas[i].oldLayer == 11)
                            {
                                TeleportationArea ta = spawnAreas[i].gameObject.AddComponent<TeleportationArea>() as TeleportationArea;
                            }
                        }
                    }
                    holdRaycasts = FindObjectOfType<HoldRaycasts>();

                    for (int i = 0; i < holdRaycasts.raycasts.Length; i++)
                    {
                        if (!holdRaycasts.raycasts[i].enabled)
                        {
                            holdRaycasts.raycasts[i].enabled = true;
                        }
                    }
                    painting = FindObjectOfType<SpawnObjects>().painting;
                    painting.PaintSpace();
                    
                    isSpawningMaterial = false;
                }
            }
        }
    }
}
