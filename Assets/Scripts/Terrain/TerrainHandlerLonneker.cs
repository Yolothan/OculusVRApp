using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TerrainHandlerLonneker : MonoBehaviour
{
    public GameObject Summer, Winter, Autumn, winterImage, summerImage, autumnImage;    
    public Material grass, grassWinter;
    public Renderer[] grassRenderer;
    private TurnRed[] turnRed;
    
    [SerializeField]
    private GameObject canvas;
    private UIScript uiScript;    
    
    [SerializeField]
    Material[] materials;
    private List<Material> materialList = new List<Material>();

    private void Start()
    {
        uiScript = FindObjectOfType<UIScript>();
        if(!uiScript.camera3D.activeSelf)
        {
            canvas.SetActive(false);
        }
        for(int i = 0; i < materials.Length; i++)
        {
            materialList.Add(materials[i]);
        }
        ChangeLeavesSummer();        
    }

    public void ChangeLeavesSummer()
    {
        turnRed = FindObjectsOfType<TurnRed>();
        winterImage.SetActive(false);
        summerImage.SetActive(true);
        autumnImage.SetActive(false);
        

        Summer.SetActive(true);
        Winter.SetActive(false);
        Autumn.SetActive(false);
        

        grass.color = new Color32(119, 216, 101, 255);        
        EnviroSkyMgr.instance.ChangeWeather("Clear Sky");

        
        for (int i = 0; i < grassRenderer.Length; i++)
        {
            grassRenderer[i].sharedMaterial = grass;
        }

        for (int i=0; i < materials.Length; ++i )
        {
            materials[i].color = new Color32(255, 255, 255, 255);
        }
        for (int i = 0; i < turnRed.Length; ++i)
        {            
            if (turnRed[i].materialMainTree1 == null)
            {
                for (int j = 0; j < turnRed[i].materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {                        
                        if (materials[k].name == turnRed[i].materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materials[j].color = new Color32(255, 255, 255, 255);
                        }
                    }

                }
            }
            else
            {
                for (int j = 0; j < turnRed[i].materialMainTree1.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {                        
                        if (materials[k].name == turnRed[i].materialMainTree1.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree1.materials[j].color = new Color32(255, 255, 255, 255);
                        }
                    }
                }
                for (int j = 0; j < turnRed[i].materialMainTree2.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree2.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree2.materials[j].color = new Color32(255, 255, 255, 255);
                        }
                    }
                }
            }
            turnRed[i].AddOldColors();
        }

    }

    public void ChangeLeavesWinter()
    {
        turnRed = FindObjectsOfType<TurnRed>();
        winterImage.SetActive(true);
        summerImage.SetActive(false);
        autumnImage.SetActive(false);
        

        Summer.SetActive(false);
        Winter.SetActive(true);
        Autumn.SetActive(false);

        grass.color = new Color32(226, 101, 101, 255);        

        EnviroSkyMgr.instance.ChangeWeather(4);

       
        for (int i = 0; i < grassRenderer.Length; i++)
        {
            grassRenderer[i].material = grassWinter;
        }
                
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = new Color32(255, 255, 255, 0);
        }
        for (int i = 0; i < turnRed.Length; ++i)
        {            
            if (turnRed[i].materialMainTree1 == null)
            {
                for (int j = 0; j < turnRed[i].materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materials[j].color = new Color32(255, 255, 255, 0);
                        }
                    }

                }
            }
            else
            {
                for (int j = 0; j < turnRed[i].materialMainTree1.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree1.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree1.materials[j].color = new Color32(255, 255, 255, 0);
                        }
                    }
                }
                for (int j = 0; j < turnRed[i].materialMainTree2.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree2.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree2.materials[j].color = new Color32(255, 255, 255, 0);
                        }
                    }
                }
            }
            turnRed[i].AddOldColors();
        }
    }
    public void ChangeLeavesSpring()
    {
        turnRed = FindObjectsOfType<TurnRed>();
        Summer.SetActive(true);
        Winter.SetActive(false);
        Autumn.SetActive(false);

        EnviroSkyMgr.instance.ChangeWeather(1);
        grass.color = new Color32(119, 216, 101, 255);
        
        winterImage.SetActive(false);
        summerImage.SetActive(true);
        autumnImage.SetActive(false);

        

        for (int i = 0; i < grassRenderer.Length; i++)
        {
            grassRenderer[i].sharedMaterial = grass;
        }

        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = new Color32(255, 199, 0, 255);
        }
        for (int i = 0; i < turnRed.Length; ++i)
        {            
            if (turnRed[i].materialMainTree1 == null)
            {
                for (int j = 0; j < turnRed[i].materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materials[j].color = new Color32(255, 199, 0, 255);
                        }
                    }

                }
            }
            else
            {
                for (int j = 0; j < turnRed[i].materialMainTree1.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree1.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree1.materials[j].color = new Color32(255, 199, 0, 255);
                        }
                    }
                }
                for (int j = 0; j < turnRed[i].materialMainTree2.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree2.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree2.materials[j].color = new Color32(255, 199, 0, 255);
                        }
                    }
                }
            }
            turnRed[i].AddOldColors();
        }
    }

    public void ChangeLeavesAutumn()
    {
        turnRed = FindObjectsOfType<TurnRed>();
        Summer.SetActive(false);
        Winter.SetActive(false);
        Autumn.SetActive(true);

        EnviroSkyMgr.instance.ChangeWeather(3);
        grass.color = new Color32(226, 101, 101, 255);
        
        winterImage.SetActive(false);
        summerImage.SetActive(false);
        autumnImage.SetActive(true);

       
        for (int i = 0; i < grassRenderer.Length; i++)
        {
            grassRenderer[i].sharedMaterial = grassWinter;
        }

        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = new Color32(224, 118, 50, 255);
        }

        for (int i = 0; i < turnRed.Length; ++i)
        {            
            if (turnRed[i].materialMainTree1 == null)
            {
                for (int j = 0; j < turnRed[i].materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materials[j].color = new Color32(224, 118, 50, 255);
                        }
                    }

                }
            }
            else
            {
                for (int j = 0; j < turnRed[i].materialMainTree1.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree1.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree1.materials[j].color = new Color32(224, 118, 50, 255);
                        }
                    }
                }
                for (int j = 0; j < turnRed[i].materialMainTree2.materials.Length; ++j)
                {
                    for (int k = 0; k < materials.Length; ++k)
                    {
                        if (materials[k].name == turnRed[i].materialMainTree2.materials[j].name.Replace(" (Instance)", ""))
                        {
                            turnRed[i].materialMainTree2.materials[j].color = new Color32(224, 118, 50, 255);
                        }
                    }
                }
            }
            turnRed[i].AddOldColors();
        }
    }  
}
