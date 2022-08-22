using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShiftWaypoints : MonoBehaviour
{
    private WaypointCar waypointCar;    
    public WaypointRouteManager[] manager, firstManager;
    private int newRouteInt;
    [SerializeField]
    private int changeOdds = 1;
    [HideInInspector]
    public bool routeChange = false;
    public int newRouteChangeSituation;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarPrefab"))
        {
            waypointCar = other.GetComponent<WaypointCar>();
            if (!routeChange)
            {
                for (int i = 0; i < firstManager.Length; i++)
                {
                    if (waypointCar.firstManager == firstManager[i])
                    {
                        newRouteInt = Random.Range(0, manager.Length + changeOdds);
                        if (newRouteInt < manager.Length)
                        {
                            waypointCar.routeManager = manager[newRouteInt];
                            waypointCar.RedoStart();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < firstManager.Length; i++)
                {
                    if (waypointCar.firstManager == firstManager[i])
                    {                                       
                         waypointCar.routeManager = manager[newRouteChangeSituation];
                         waypointCar.RedoStart();                        
                    }
                }
            }
            
        }
    }
}
