using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI3D : MonoBehaviour
{
    [SerializeField]
    private Slider sliderDrag, sliderLook, sliderScroll;
    private FlyController flyController;
    private GameDataHandler gameDataHandler;
    [SerializeField]
    private Text dragText, lookText, scrollText;
    private bool showDeleted = true;
    private bool showObjects = true;
    private UIScript uiScript;
    

    private void Start()
    {
        if (FindObjectOfType<FlyController>() != null)
        {
            flyController = FindObjectOfType<FlyController>();
            gameDataHandler = FindObjectOfType<GameDataHandler>();
            OnDragChange();
            OnZoomChange();
            OnLookChange();
        }
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        uiScript = FindObjectOfType<UIScript>();
        uiScript.LoadAssets();
        uiScript.goInThreeD = true;
        SceneLoader.Instance.LoadNewScene("MenuComputer");
        uiScript.go3Dbool = false;
    }

    public void OnDragChange()
    {
        flyController.translationSensitivity = sliderDrag.value * (2.0f / 5.0f);              
        dragText.text = sliderDrag.value.ToString();               
    }

    public void OnZoomChange()
    {        
        flyController.zoomSensitiviy = sliderScroll.value * 2.0f;
        scrollText.text = sliderScroll.value.ToString();
    }

    public void OnLookChange()
    {        
        flyController.rotationSensitiviry = sliderLook.value * (2.0f / 5.0f);
        lookText.text = sliderLook.value.ToString();
    }

    public void ShowDeleted()
    {
        if (gameDataHandler.changedObjectsDestroyed != null)
        {
            if (showDeleted)
            {
                for (int i = 0; i < gameDataHandler.changedObjectsDestroyed.Count; i++)
                {
                    gameDataHandler.changedObjectsDestroyed[i].SetActive(true);
                    if (!gameDataHandler.changedObjectsDestroyed[i].name.Contains("tree_main"))
                    {                        
                        if (gameDataHandler.changedObjectsDestroyed[i].GetComponent<Renderer>() == null)
                        {
                            Material[] materials = gameDataHandler.changedObjectsDestroyed[i].GetComponentInChildren<Renderer>().materials;                           
                            for (int j = 0; j < materials.Length; j++)
                            {                                
                                materials[j].color = Color.red;
                            }
                        }
                        else
                        {
                            Material[] materials = gameDataHandler.changedObjectsDestroyed[i].GetComponent<Renderer>().materials;
                            for (int j = 0; j < materials.Length; j++)
                            {                                
                                materials[j].color = Color.red;
                            }
                        }
                    }
                    else
                    {
                        if (gameDataHandler.changedObjectsDestroyed[i].GetComponent<TurnRed>() != null)
                        {                            
                            Material[] materials1 = gameDataHandler.changedObjectsDestroyed[i].GetComponent<TurnRed>().materialMainTree1.materials;
                            Material[] materials2 = gameDataHandler.changedObjectsDestroyed[i].GetComponent<TurnRed>().materialMainTree2.materials;
                            for (int j = 0; j < materials1.Length; j++)
                            {
                                materials1[j].color = Color.red;
                            }
                            for (int j = 0; j < materials2.Length; j++)
                            {
                                materials2[j].color = Color.red;
                            }
                        }
                    }
                }
                showDeleted = false;
            }
            else
            {
                for (int i = 0; i < gameDataHandler.changedObjectsDestroyed.Count; i++)
                {
                    gameDataHandler.changedObjectsDestroyed[i].SetActive(false);
                }
                showDeleted = true;
            }
        }        
    }

    public void ChangedObjects()
    {
        if (gameDataHandler.changedObjects != null)
        {
            if (showObjects)
            {
                for (int i = 0; i < gameDataHandler.changedObjects.Count; i++)
                {                    
                    if (!gameDataHandler.changedObjects[i].name.Contains("tree_main"))
                    {
                        if (gameDataHandler.changedObjects[i].GetComponent<Renderer>() == null)
                        {
                            Material[] materials = gameDataHandler.changedObjects[i].GetComponentInChildren<Renderer>().materials;
                            for (int j = 0; j < materials.Length; j++)
                            {                                
                                materials[j].color = Color.green;
                            }
                        }
                        else
                        {
                            Material[] materials = gameDataHandler.changedObjects[i].GetComponent<Renderer>().materials;
                            for (int j = 0; j < materials.Length; j++)
                            {                                
                                materials[j].color = Color.green;
                            }
                        }
                    }
                    else
                    {
                        if (gameDataHandler.changedObjects[i].GetComponent<TurnRed>() != null)
                        {
                            Material[] materials1 = gameDataHandler.changedObjects[i].GetComponent<TurnRed>().materialMainTree1.materials;
                            Material[] materials2 = gameDataHandler.changedObjects[i].GetComponent<TurnRed>().materialMainTree2.materials;
                            for (int j = 0; j < materials1.Length; j++)
                            {                                
                                materials1[j].color = Color.green;
                            }
                            for (int j = 0; j < materials2.Length; j++)
                            {
                                materials2[j].color = Color.green;
                            }
                        }
                    }
                }
                showObjects = false;
            }
            else
            {
                for (int i = 0; i < gameDataHandler.changedObjects.Count; i++)
                {
                    if(gameDataHandler.changedObjects[i].GetComponent<TurnRed>() != null)
                    {
                        TurnRed turnRed = gameDataHandler.changedObjects[i].GetComponent<TurnRed>();                        
                        turnRed.ChangeColorBack();
                    }
                    else
                    {
                        TurnRed turnRed = gameDataHandler.changedObjects[i].GetComponentInChildren<TurnRed>();                        
                        turnRed.ChangeColorBack();
                    }
                }
                showObjects = true;
            }
        }
    }
}
