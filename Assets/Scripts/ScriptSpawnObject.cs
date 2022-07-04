using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;



public class ScriptSpawnObject : MonoBehaviour
{    
    private GameObject lineVisualObject, parent;
    private Transform reticle, prefab;    
    private SpawnObjects spawnObjects;

    private XRInteractorLineVisual lineVisual;   

    private RotateGameObject rotateScript;

    private ScaleGameObject scaleScript;

    private ScriptSpawnObject[] spawnAreas;
    private CancelSetObject cancelSetObject;
    private HoldRaycasts holdRaycasts;

    private Painting painting;
    
    private Material oldMaterial;
    private Renderer renderer2;
    [HideInInspector]
    public bool preventExit = false;
    [HideInInspector]
    public int oldLayer;

    private GameDataHandler gameDataHandler;


    private void Start()
    {
        renderer2 = GetComponent<Renderer>();
        oldMaterial = renderer2.sharedMaterial;
        spawnAreas = FindObjectsOfType<ScriptSpawnObject>();
        if (GameObject.Find("RightHandRayInteractor") != null)
        {
            lineVisualObject = GameObject.Find("RightHandRayInteractor");
            lineVisual = lineVisualObject.GetComponent<XRInteractorLineVisual>();
        }
        oldLayer = gameObject.layer;

    }


    public void SetObject()
    {         
        gameDataHandler = FindObjectOfType<GameDataHandler>();
        reticle = lineVisual.reticle.transform;

        

        prefab = reticle.GetChild(1);

        if (GameObject.Find("ParentSpawnObjects") == null)
        {
            parent = new GameObject("ParentSpawnObjects");
        }
        else
        {
            parent = GameObject.Find("ParentSpawnObjects");
        }
       
        prefab.parent = parent.transform;                          
            
        rotateScript = prefab.gameObject.GetComponent<RotateGameObject>();
        rotateScript.enabled = false;

        cancelSetObject = prefab.gameObject.GetComponent<CancelSetObject>();
        cancelSetObject.enabled = false;

        gameDataHandler.instantiatedObjects.Add(prefab.gameObject);

        if(prefab.gameObject.GetComponent<ScaleGameObject>() != null)
        {
            scaleScript = prefab.gameObject.GetComponent <ScaleGameObject>();
            scaleScript.enabled = false;
        }

        for(int i = 0; i < spawnAreas.Length; i++)
        {
            spawnAreas[i].gameObject.layer = oldLayer;
            XRSimpleInteractable interaction = spawnAreas[i].GetComponent<XRSimpleInteractable>();
            Destroy(interaction);
            if (oldLayer == 11)
            {
                TeleportationArea ta = spawnAreas[i].gameObject.AddComponent<TeleportationArea>() as TeleportationArea;
            }
        }        
        holdRaycasts = FindObjectOfType<HoldRaycasts>();

        for(int i = 0; i < holdRaycasts.raycasts.Length; i++)
        {
            holdRaycasts.raycasts[i].enabled = true;
        }
        painting = FindObjectOfType<SpawnObjects>().painting;
        painting.PaintSpace();

    }

    public void SetMaterial()
    {
        gameDataHandler = FindObjectOfType<GameDataHandler>();
        preventExit = true;


        renderer2.sharedMaterial = lineVisual.reticle.GetComponent<Renderer>().sharedMaterial;        
        oldMaterial = renderer2.sharedMaterial;

        if (!gameDataHandler.materials.Contains(gameObject))
        {
            gameDataHandler.materials.Add(gameObject);
        }

        for (int i = 0; i < spawnAreas.Length; i++)
        {
            
            XRSimpleInteractable interaction = spawnAreas[i].GetComponent<XRSimpleInteractable>();
            Destroy(interaction);
            if (oldLayer == 11)
            {
                TeleportationArea ta = spawnAreas[i].gameObject.AddComponent<TeleportationArea>() as TeleportationArea;                
            }
            spawnAreas[i].gameObject.layer = oldLayer;
        }
        holdRaycasts = FindObjectOfType<HoldRaycasts>();

        for (int i = 0; i < holdRaycasts.raycasts.Length; i++)
        {
            holdRaycasts.raycasts[i].enabled = true;
        }
        spawnObjects = FindObjectOfType<SpawnObjects>();
        spawnObjects.isSpawningMaterial = false;
        painting = FindObjectOfType<SpawnObjects>().painting;
        painting.PaintSpace();

    }

    public void HoverMaterial()
    {        
        renderer2.sharedMaterial = lineVisual.reticle.GetComponent<Renderer>().sharedMaterial;
    }
    public void ExitMaterial()
    {
        if (!preventExit)
        {
            renderer2.sharedMaterial = oldMaterial;
        }
        
    }

    
    
}