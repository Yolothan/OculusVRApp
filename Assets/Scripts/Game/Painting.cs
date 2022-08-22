using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    private GameObject controllerObject, paintPointer;    
    private GameObject[] drawObjects, paint;
    
    public Toggle paintOn, paintSpace, paintSurface;
    [SerializeField]
    private Text textSlider;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private UIScript uiScript;
    [SerializeField]
    private GameObject reticle, reticleChild;
    [SerializeField]
    private XRRayInteractor interactor;
    [SerializeField]
    private RemoveAllObjects erase;
    [SerializeField]
    private Dropdown dropDownMenu, dropDownShaders;
    private XRController controller;
    private MeshLineRenderer currLine;
    private LineRenderer currLine2;
    private bool firstTrigger = false;
    private Material materialPicked;
    
    [SerializeField]
    private Material startMaterial;
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private Shader[] shaders;
    private List<string> materialList = new List<string>();
    private List<string> shaderList = new List<string>();    
    private ScriptSpawnObject[] floors;
    private TeleportationArea teleportationArea;        
    private int numClicks = 0;
    private float sliderValue = 1;
    private int indexShader, indexColor;
    [SerializeField]
    private GameDataHandler gameDataHandler;
    
    private void Start()
    {       
        controllerObject = GameObject.Find("RightHandController");
        

        controller = controllerObject.GetComponent<XRController>();
        

        textSlider.text = slider.value + " px";
        materialPicked = startMaterial;
        
        for (int i = 0; i < materials.Length; i++)
        {
            materialList.Add(materials[i].name);
        }        
        dropDownMenu.AddOptions(materialList);
        ColorPicker();

        for(int i = 0; i < shaders.Length; i++)
        {
            shaderList.Add(shaders[i].name);
        }
        
        dropDownShaders.AddOptions(shaderList);
        
        ShaderPicker();        
        floors = FindObjectsOfType<ScriptSpawnObject>();
        drawObjects = GameObject.FindGameObjectsWithTag("Draw");
        this.gameObject.SetActive(false);

    }

    public void OnRelease()
    {
        firstTrigger = false;
    }

    public void ChangeTextSlider()
    {
        textSlider.text = slider.value + " px";
        sliderValue = slider.value;
    }

    public void ColorPicker()
    {
        indexColor = dropDownMenu.value;
        materialPicked = materials[indexColor];
        materialPicked.shader = shaders[0];
        Image image = dropDownMenu.gameObject.GetComponent<Image>();
        image.color = materialPicked.color;
        materialPicked.shader = shaders[indexShader];
        
    }
   
    public void ShaderPicker()
    {
        indexShader = dropDownShaders.value;
        Material newMaterial = new Material(shaders[indexShader]);
        newMaterial.name = materialPicked.name;
        newMaterial.color = materials[indexColor].color;
        materialPicked = newMaterial;
    }

    public void PaintSpace()
    {        
        paintSpace.isOn = true;
        paintSurface.isOn = false;

        if (paintOn.isOn)
        {
            FloorsHandleOff();
        }
           
        
    }

    public void PaintSurface()
    {        
        paintSpace.isOn = false;
        paintSurface.isOn = true;
        if (paintOn.isOn)
        {
            FloorsHandle();
        }
        
    }

    public void Paint()
    {        
        if (!paintOn.isOn)
        {             
            paintOn.isOn = true;
            erase.DisallowErase();

            FloorsHandle();
            
        }
        else
        {
            DisallowPaint();


        }
    }

    private void FloorsHandle()
    {
        floors = FindObjectsOfType<ScriptSpawnObject>();
        drawObjects = GameObject.FindGameObjectsWithTag("Draw");
        if (paintSurface.isOn)
        {
            uiScript.DeactivateTeleport();
        }
       

        for (int i = 0; i < floors.Length; i++)
            {                     
            if (paintSurface.isOn)
            {
                if (floors[i].GetComponent<TeleportationArea>() != null)
                {
                    teleportationArea = floors[i].GetComponent<TeleportationArea>();
                    Destroy(teleportationArea);

                }
                floors[i].gameObject.layer = 6;
            }
        }
            for (int i = 0; i < drawObjects.Length; i++)
            {
            
                
            
            if (paintSurface.isOn)
            {
                if (drawObjects[i].GetComponent<XRSimpleInteractable>() == null)
                {
                    XRSimpleInteractable interactable = drawObjects[i].AddComponent<XRSimpleInteractable>() as XRSimpleInteractable;

                }
                drawObjects[i].gameObject.layer = 6;
            }
        }
        
    }

    private void FloorsHandleOff()
    {
        floors = FindObjectsOfType<ScriptSpawnObject>();
        drawObjects = GameObject.FindGameObjectsWithTag("Draw");
        if (paintSpace.isOn)
        {
            uiScript.ActivateTeleportRay();
        }
        for (int i = 0; i < drawObjects.Length; i++)
            {
                if (drawObjects[i].GetComponent<XRSimpleInteractable>() != null)
                {
                    XRSimpleInteractable interactable = drawObjects[i].GetComponent<XRSimpleInteractable>();
                    Destroy(interactable);
                }
            }
          

            for (int i = 0; i < floors.Length; i++)
            {
                if (floors[i].GetComponent<TeleportationArea>() == null)
                {
                    if (floors[i].oldLayer == 11)
                    {
                        TeleportationArea ta = floors[i].gameObject.AddComponent<TeleportationArea>() as TeleportationArea;
                    }
                    floors[i].gameObject.layer = floors[i].oldLayer;
                }
            }

        
        
    }

    public void DisallowPaint()
    {        
        paintOn.isOn = false;
        uiScript.ActivateTeleportRay();

        FloorsHandleOff();
        
    }

    public void ErasePaint()
    {
        paint = GameObject.FindGameObjectsWithTag("Painting");
        for (int i = 0; i < paint.Length; i++)
        {
            Destroy(paint[i]);
        }
    }
    
 


    public void Update()
    {
        /*
            if (uiScript.paint)
            {

                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
                {                    
                    if (isPressed && !reticle.activeSelf && uiScript.space)
                    {
                        if (!firstTrigger)
                        {

                            GameObject go = new GameObject("Lines");                            
                            go.AddComponent<MeshFilter>();
                            go.AddComponent<MeshRenderer>();
                            currLine = go.AddComponent<MeshLineRenderer>();
                            currLine.setWidth(sliderValue * 0.01f);
                            currLine.lmat = materialPicked;
                            firstTrigger = true;
                            Debug.Log(sliderValue);
                        }
                        else
                        {
                            Vector3 newPosition = new Vector3(paintPointer.transform.position.x, paintPointer.transform.position.y, paintPointer.transform.position.z);

                            currLine.AddPoint(newPosition);
                        }
                    }
                    else if (isPressed && reticle.activeSelf && uiScript.surface)
                    {
                        if (!firstTrigger)
                        {
                            GameObject go = new GameObject("Lines");                           
                            go.AddComponent<MeshFilter>();
                            go.AddComponent<MeshRenderer>();
                            currLine = go.AddComponent<MeshLineRenderer>();
                            currLine.setWidth(sliderValue * 0.01f);
                            currLine.lmat = materialPicked;

                            firstTrigger = true;
                        }
                        else
                        {
                            Vector3 newPosition = new Vector3(reticle.transform.position.x, reticle.transform.position.y, reticle.transform.position.z);

                            currLine.AddPoint(newPosition);
                           
                        }
                    }
                    else
                    {
                        firstTrigger = false;
                    }


                }
            }*/
        
        
            if (paintOn.isOn)
            {                
                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
                {                
                    if (isPressed && paintSpace.isOn)
                    {                        
                        interactor.enabled = false;
                        if (!firstTrigger)
                        {                            
                            GameObject go = new GameObject("Lines");
                            go.tag = "Painting";
                           
                            paintPointer = GameObject.Find("PaintPointer");
                            currLine2 = go.AddComponent<LineRenderer>();
                            currLine2.startWidth = sliderValue * 0.01f;
                            currLine2.endWidth = sliderValue * 0.01f;
                            currLine2.sharedMaterial = materialPicked;
                            gameDataHandler.lines.Add(go);
                            
                            numClicks = 0;                        
                            firstTrigger = true;

                        }
                        else
                        {                            
                            Vector3 newPosition = new Vector3(paintPointer.transform.position.x, paintPointer.transform.position.y, paintPointer.transform.position.z);
                            currLine2.positionCount = numClicks + 1;
                            currLine2.SetPosition(numClicks, newPosition);                            
                            numClicks++;
                        }
                    }
                    else if (isPressed && paintSurface.isOn)
                    {                        
                        if (!firstTrigger)
                        {
                            GameObject go = new GameObject("Lines");
                            go.tag = "Painting";                                                   
                            currLine2 = go.AddComponent<LineRenderer>();
                            currLine2.startWidth = sliderValue * 0.01f;
                            currLine2.endWidth = sliderValue * 0.01f;
                            currLine2.sharedMaterial = materialPicked;
                            gameDataHandler.lines.Add(go);
                            firstTrigger = true;
                            numClicks = 0;
                        }
                        else
                        {
                            Vector3 newPosition = new Vector3(reticleChild.transform.position.x, reticleChild.transform.position.y, reticleChild.transform.position.z);
                            currLine2.positionCount = numClicks + 1;
                            currLine2.SetPosition(numClicks, newPosition);                            
                            numClicks++;
                        }
                    }
                    
                    if (!isPressed && paintOn)
                    {
                        interactor.enabled = true;
                    }    
                }
            }        
    }
}
