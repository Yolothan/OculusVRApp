using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{    
    public List<GameObject> carPrefabs = new List<GameObject>();        
       
    public bool spawnCarsByDelay = false;    
    public WaypointRouteManager[] spawnRoutes;
    [HideInInspector]
    public int chosenInt;
    [HideInInspector]
    public GameObject car;
    private WaypointCar waypointCar;
    public bool useMaxPrefabs = false;

    private SpawnWaypointCar[] spawnWaypoints;
    private BoxCollider[] colliders;

    [HideInInspector]
    public GameObject[] allCars;




    private void Start()
    {
        
        if (spawnCarsByDelay)
        {
            spawnWaypoints = FindObjectsOfType<SpawnWaypointCar>();
            for (int i = 0; i < spawnWaypoints.Length; i++)
            {
                colliders = spawnWaypoints[i].gameObject.GetComponents<BoxCollider>();
                for (int j = 0; j < colliders.Length; j++)
                {
                    colliders[j].enabled = false;
                }
                spawnWaypoints[i].enabled = false;
            }
            
        }

        if (!spawnCarsByDelay)
        {
            if (!useMaxPrefabs)
            {
                for (int i = 0; i < spawnRoutes.Length; i++)
                {
                    InstantiateCars(spawnRoutes[i].waypoints[0].transform);
                    chosenInt = i;
                    waypointCar = car.GetComponent<WaypointCar>();
                    waypointCar.routeManager = spawnRoutes[chosenInt];
                    waypointCar.chosenInt = chosenInt;
                    spawnRoutes[i].delay = Random.Range(spawnRoutes[i].minDelay, spawnRoutes[i].maxDelay);
                }

            }
            else
            {
                for (int i = 0; i < spawnRoutes.Length; i++)
                {
                    spawnRoutes[i].InstantiateCarsMaxPrefabs(spawnRoutes[i].waypoints[0].transform);
                    chosenInt = i;
                    waypointCar = car.GetComponent<WaypointCar>();
                    waypointCar.routeManager = spawnRoutes[chosenInt];
                    waypointCar.chosenInt = chosenInt;
                }
            }
        }
        
        
    }   

    public void InstantiateCars(Transform position)
    {       
            int randomValueCar = Random.Range(0, carPrefabs.Count);
            car = Instantiate(carPrefabs[randomValueCar], position.position, Quaternion.identity);                   
    }      
  
}
