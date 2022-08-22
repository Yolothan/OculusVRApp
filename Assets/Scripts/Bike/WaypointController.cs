using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    public List<GameObject> bikePrefabs = new List<GameObject>();

    public bool spawnBikesByDelay = false;
    public WaypointRouteManagerBikes[] spawnRoutes;
    [HideInInspector]
    public int chosenInt;
    [HideInInspector]
    public GameObject bike;
    private WaypointsRoute waypointBike;
    public bool useMaxPrefabs = false;

    private SpawnBikers[] spawnWaypoints;
    private BoxCollider[] colliders;

    [HideInInspector]
    public GameObject[] allBikes;

    private void Start()
    {

        if (spawnBikesByDelay)
        {
            spawnWaypoints = FindObjectsOfType<SpawnBikers>();
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

        if (!spawnBikesByDelay)
        {
            if (!useMaxPrefabs)
            {
                for (int i = 0; i < spawnRoutes.Length; i++)
                {
                    InstantiateBikes(spawnRoutes[i].waypoints[0].transform);
                    chosenInt = i;
                    waypointBike = bike.GetComponent<WaypointsRoute>();
                    waypointBike.routeManager = spawnRoutes[chosenInt];
                    waypointBike.chosenInt = chosenInt;
                    spawnRoutes[i].delay = Random.Range(spawnRoutes[i].minDelay, spawnRoutes[i].maxDelay);
                }

            }
            else
            {
                for (int i = 0; i < spawnRoutes.Length; i++)
                {
                    spawnRoutes[i].InstantiateBikesMaxPrefabs(spawnRoutes[i].waypoints[0].transform);
                    chosenInt = i;
                    waypointBike = bike.GetComponent<WaypointsRoute>();
                    waypointBike.routeManager = spawnRoutes[chosenInt];
                    waypointBike.chosenInt = chosenInt;
                }
            }
        }


    }
    

    public void InstantiateBikes(Transform position)
    {
        int randomValueCar = Random.Range(0, bikePrefabs.Count);
        bike = Instantiate(bikePrefabs[randomValueCar], position.position, Quaternion.identity);
    }

   
}
