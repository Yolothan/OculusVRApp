using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointRouteManagerBikes : MonoBehaviour
{
    [SerializeField]
    private WaypointController controller;
    public List<Transform> waypoints = new List<Transform>();
    public float maxPrefabs;
    public float minDelay = 5, maxDelay = 10;
    [HideInInspector]
    public float delay;
    //[HideInInspector]
    public List<GameObject> listBikes = new List<GameObject>();
    [HideInInspector]
    public bool occupied = false;
    [HideInInspector]
    public float timer;
    [HideInInspector]
    public GameObject bike;
    private WaypointsRoute waypointBike;
    [SerializeField]
    private bool spawnRoute = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BikePrefab") || other.CompareTag("CarPrefab"))
        {
            occupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BikePrefab") || other.CompareTag("CarPrefab"))
        {
            occupied = false;
        }
    }

    private void Update()
    {
        if (spawnRoute)
        {          
            if (controller.spawnBikesByDelay)
            {
                timer += Time.deltaTime;
                if (controller.useMaxPrefabs)
                {
                    if (listBikes.Count < maxPrefabs && timer > delay)
                    {
                        if (!occupied)
                        {
                            InstantiateBikes(waypoints[0].transform);
                            waypointBike = bike.GetComponent<WaypointsRoute>();
                            waypointBike.routeManager = this;
                            delay = Random.Range(minDelay, maxDelay);
                            timer = 0.0f;
                        }


                    }

                }
                else
                {
                    if (timer > delay)
                    {
                        if (!occupied)
                        {
                            InstantiateBikes(waypoints[0].transform);
                            waypointBike = bike.GetComponent<WaypointsRoute>();
                            waypointBike.routeManager = this;
                            delay = Random.Range(minDelay, maxDelay);
                            timer = 0.0f;
                        }

                    }

                }
            }
        }
        listBikes.RemoveAll(GameObject => GameObject == null);
    }






    public void InstantiateBikesMaxPrefabs(Transform position)
    {

        if (controller.useMaxPrefabs)
        {
            if (listBikes.Count < maxPrefabs)
            {
                int randomValueCar = Random.Range(0, controller.bikePrefabs.Count);
                bike = Instantiate(controller.bikePrefabs[randomValueCar], position.position, Quaternion.identity);            }
        }
    }
    public void InstantiateBikes(Transform position)
    {
        int randomValueCar = Random.Range(0, controller.bikePrefabs.Count);
        bike = Instantiate(controller.bikePrefabs[randomValueCar], position.position, Quaternion.identity);
    }
}
