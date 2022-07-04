using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSituationLonneker : MonoBehaviour
{      
    
    public List<GameObject> newObjectsVisible, oldObjectsVisible, highPoly, lowPoly;

    [SerializeField]
    private WaypointRouteManager spawnrouteZ, spawnrouteY1;

    [SerializeField]
    private BoxCollider shiftWaypointsY1, shiftWaypointsY;

    [SerializeField]
    private ShiftWaypoints shiftWaypointZ1;

    [SerializeField]
    private Material materialPLines;

    private UIScript uiScript;



    private void Start()
    {
        uiScript = FindObjectOfType<UIScript>();
        uiScript.wristUI.SetActive(true);
        uiScript.carIntensity.gameObject.SetActive(true);
        uiScript.carIntensity.lateStart();
        uiScript.carIntensity.gameObject.SetActive(false);
        uiScript.wristUI.SetActive(false);
    }


    public void ChangeRoutes()
    {
        spawnrouteZ.minDelay = 16.0f;
        spawnrouteZ.maxDelay = 22.0f;
        spawnrouteY1.minDelay = 17.0f;
        spawnrouteY1.maxDelay = 22.0f;

        shiftWaypointsY.enabled = false;
        shiftWaypointsY1.enabled = false;
        shiftWaypointZ1.routeChange = true;

        for(int i = 0; i < newObjectsVisible.Count; i++)
        {
            if (newObjectsVisible[i] != null)
            {
                newObjectsVisible[i].SetActive(true);
            }
        }
        for(int i = 0; i < oldObjectsVisible.Count; i++)
        {
            if (oldObjectsVisible[i] != null)
            {
                oldObjectsVisible[i].SetActive(false);
            }
        }

        materialPLines.color = new Color32(106, 105, 255, 255);
       
    }

    public void ChangeRoutesBack()
    {
        spawnrouteZ.minDelay = 16.0f;
        spawnrouteZ.maxDelay = 22.0f;
        spawnrouteY1.minDelay = 17.0f;
        spawnrouteY1.maxDelay = 22.0f;

        shiftWaypointsY.enabled = true;
        shiftWaypointsY1.enabled = true;
        shiftWaypointZ1.routeChange = false;

        for (int i = 0; i < newObjectsVisible.Count; i++)
        {
            newObjectsVisible[i].SetActive(false);
        }
        for (int i = 0; i < oldObjectsVisible.Count; i++)
        {
            oldObjectsVisible[i].SetActive(true);
        }
        materialPLines.color = new Color32(204, 204, 204, 255);
    }

    public void LowPoly()
    {
        for (int i = 0; i < lowPoly.Count; i++)
        {
            if (lowPoly[i] != null)
            {
                lowPoly[i].SetActive(true);
            }
        }
        for (int i = 0; i < highPoly.Count; i++)
        {
            if (highPoly[i] != null)
            {
                highPoly[i].SetActive(false);
            }
        }
    }
    public void HighPoly()
    {
        for (int i = 0; i < lowPoly.Count; i++)
        {
            if (lowPoly[i] != null)
            {
                lowPoly[i].SetActive(false);
            }
        }
        for (int i = 0; i < highPoly.Count; i++)
        {
            if (highPoly[i] != null)
            {
                highPoly[i].SetActive(true);
            }
        }
    }
}
