using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnRed : MonoBehaviour
{
    [HideInInspector]
    public Material[] materials;    
    public Renderer materialMainTree1; 
    public Renderer materialMainTree2;
    [HideInInspector]
    public List<Color> oldColors = new List<Color>();
    private List<Color> oldColor1 = new List<Color>();
    private List<Color> oldColor2 = new List<Color>();
    private XRSimpleInteractable interactable;
    private GameDataHandler gameDataHandler;
    public bool isMainTree = false;
    


    private void Start()
    {
        gameDataHandler = FindObjectOfType<GameDataHandler>();
        gameDataHandler.targetList.Add(this.gameObject);
        interactable = gameObject.AddComponent<XRSimpleInteractable>() as XRSimpleInteractable;

        interactable.hoverEntered.AddListener(delegate { ChangeColor(); });
        interactable.hoverExited.AddListener(delegate { ChangeColorBack(); });
        interactable.selectExited.AddListener(delegate { Delete(); });

        interactable.enabled = false;

        if (!isMainTree)
        {

            if (GetComponent<Renderer>() == null)
            {
                materials = GetComponentInChildren<Renderer>().materials;
            }
            else
            {
                materials = GetComponent<Renderer>().materials;
            }
        }       

        AddOldColors();    
    }

    public void ChangeColor()
    {     
        
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].color.a == 0)
            {
                materials[i].color = new Color(255, 0, 0, 0);
            }
            else
            {
                materials[i].color = Color.red;
            }
        }
        if (isMainTree)
        {
            for (int i = 0; i < materialMainTree1.materials.Length; i++)
            {
                if (materialMainTree1.materials[i].color.a == 0)
                {
                    materialMainTree1.materials[i].color = new Color(255, 0, 0, 0);
                }
                else
                {
                    materialMainTree1.materials[i].color = Color.red;
                }
            }
            for (int i = 0; i < materialMainTree2.materials.Length; i++)
            {
                if (materialMainTree2.materials[i].color.a == 0)
                {
                    materialMainTree2.materials[i].color = new Color(255, 0, 0, 0);
                }
                else
                {
                    materialMainTree2.materials[i].color = Color.red;
                }
            }
        }
    }

    public void ChangeColorBack()
    {        
        if (isMainTree)
        {
            for (int i = 0; i < materialMainTree1.materials.Length; i++)
            {
                materialMainTree1.materials[i].color = oldColor1[i];
            }
            for (int i = 0; i < materialMainTree2.materials.Length; i++)
            {
                materialMainTree2.materials[i].color = oldColor2[i];
            }
        }
        else
        {            
            if (GetComponentInChildren<Renderer>() != null)
             {                
                 materials = GetComponentInChildren<Renderer>().materials;
             }
             else
             {                
                materials = GetComponent<Renderer>().materials;
             }
            
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = oldColors[i];
            }
        }
        

    }

    public void Delete()
    {        
        gameDataHandler.destroyedObjects.Add(gameObject);
        if(gameDataHandler.instantiatedObjects.Contains(gameObject))
        {
            gameDataHandler.instantiatedObjects.Remove(gameObject);
        }        
        gameObject.SetActive(false);
    }

    public void AddOldColors()
    {
        oldColors.Clear();
        oldColor1.Clear();
        oldColor2.Clear();
        if (!isMainTree)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                oldColors.Add(color);
            }
        }
        else
        {
            for (int i = 0; i < materialMainTree1.materials.Length; i++)
            {
                Color color = materialMainTree1.materials[i].color;
                oldColor1.Add(color);
                
            }
            for (int i = 0; i < materialMainTree2.materials.Length; i++)
            {
                Color color2 = materialMainTree2.materials[i].color;
                oldColor2.Add(color2);
            }
        }
        
        
    }

}
