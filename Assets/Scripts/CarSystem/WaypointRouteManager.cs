using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointRouteManager : MonoBehaviour
{
    [SerializeField]
    private CarController controller;
    public List<Transform> waypoints = new List<Transform>();
    public float maxPrefabs;
    public float minDelay = 5, maxDelay = 10;
    [HideInInspector]
    public float delay, oldDelayMin, oldDelayMax;
    //[HideInInspector]
    public List<GameObject> listCars = new List<GameObject>();
    [HideInInspector]
    public bool occupied = false;
    [HideInInspector]
    public float timer;
    [HideInInspector]
    public GameObject car;
    private WaypointCar waypointCar;
    [SerializeField]
    private bool spawnRoute = false;

    private void Start()
    {
        oldDelayMin = minDelay;
        oldDelayMax = maxDelay;
    }

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
            if (controller.spawnCarsByDelay)
            {
                timer += Time.deltaTime;
                if (controller.useMaxPrefabs)
                {
                    if (listCars.Count < maxPrefabs && timer > delay)
                    {
                        if (!occupied)
                        {
                            InstantiateCars(waypoints[0].transform);
                            waypointCar = car.GetComponent<WaypointCar>();
                            waypointCar.routeManager = this;
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
                            InstantiateCars(waypoints[0].transform);
                            waypointCar = car.GetComponent<WaypointCar>();
                            waypointCar.routeManager = this;
                            delay = Random.Range(minDelay, maxDelay);
                            timer = 0.0f;
                        }

                    }

                }
            }
            listCars.RemoveAll(GameObject => GameObject == null);
        }       
    }



    


    public void InstantiateCarsMaxPrefabs(Transform position)
    {
        
        if (controller.useMaxPrefabs)
        {
            if (listCars.Count < maxPrefabs)
            {
                int randomValueCar = Random.Range(0, controller.carPrefabs.Count);
                car = Instantiate(controller.carPrefabs[randomValueCar], position.position, Quaternion.identity);
            }
        }
    }
    public void InstantiateCars(Transform position)
    {
        int randomValueCar = Random.Range(0, controller.carPrefabs.Count);
        car = Instantiate(controller.carPrefabs[randomValueCar], position.position, Quaternion.identity);
    }
}
