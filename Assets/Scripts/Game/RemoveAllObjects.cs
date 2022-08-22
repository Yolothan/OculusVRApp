using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.XR;

public class RemoveAllObjects : MonoBehaviour
{   
    private GameObject[] allObjects;   
    [SerializeField]
    private Toggle eraseOn;    
    [SerializeField]
    private Painting painting;
    private GameDataHandler gameData;
    
    

    public void RemoveAll()
    {        
        allObjects = GameObject.FindGameObjectsWithTag("Objects");
        gameData = FindObjectOfType<GameDataHandler>();

        for (int i = 0; i < allObjects.Length; i++)
        {        
            if(!gameData.destroyedObjects.Contains(allObjects[i]))
            {
                gameData.destroyedObjects.Add(allObjects[i]);
                if(gameData.instantiatedObjects.Contains(allObjects[i]))
                {
                    gameData.instantiatedObjects.Remove(allObjects[i]);
                }
            }
            allObjects[i].SetActive(false);
        }
    }

    public void Eraser()
    {
        if (!eraseOn.isOn)
        {
            allObjects = GameObject.FindGameObjectsWithTag("Objects");

            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i].GetComponent<XRSimpleInteractable>() != null)
                {
                    allObjects[i].GetComponent<XRSimpleInteractable>().enabled = true;
                    allObjects[i].GetComponent<BoxCollider>().enabled = true;
                }
                    
                               
            }
            painting.DisallowPaint();
            eraseOn.isOn = true;
        }
        else
        {
            DisallowErase();
        }
    }

    public void DisallowErase()
    {
        

        eraseOn.isOn = false;
        
        allObjects = GameObject.FindGameObjectsWithTag("Objects");

        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].GetComponent<XRSimpleInteractable>() != null)
            {
                allObjects[i].GetComponent<XRSimpleInteractable>().enabled = false;
                allObjects[i].GetComponent<BoxCollider>().enabled = false;

            }
            
        }
        
    }

   

}
