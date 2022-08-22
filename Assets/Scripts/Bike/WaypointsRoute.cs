using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Don't forget to change the name of the class to the new Script name
public class WaypointsRoute : MonoBehaviour
{
    [SerializeField]
    private GameObject imageHazerswoude, imageLonneker;
    public List<Transform> waypointList = new List<Transform>();    
    private WaypointController controllerComponent;
    private Transform targetWaypoint;
    private int targetWaypointIndex;
    private float minDistance = 0.1f;
    private int lastWaypointIndex;
    private Animator animator;
    private float timerStop;
    [HideInInspector]
    public int chosenInt;
    private WaypointsRoute otherWaypointBike;

    [HideInInspector]
    public WaypointRouteManagerBikes routeManager, firstManager;


    private WaypointSpeed waypointSpeed;

    private float initialSpeedAnim;

    public float movementSpeed = 3.0f;
    private float maxSpeed;
    private float rotationSpeed = 2.0f;

    public bool stopCycling = false;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if(sceneName == "Lonneker")
        {
            imageHazerswoude.SetActive(false);
            imageLonneker.SetActive(true);
        }
        if (sceneName == "Hazerswoude")
        {
            imageHazerswoude.SetActive(true);
            imageLonneker.SetActive(false);
        }


        controllerComponent = FindObjectOfType<WaypointController>();        

        waypointList = routeManager.waypoints;

        firstManager = routeManager;

        lastWaypointIndex = waypointList.Count;
        targetWaypoint = waypointList[targetWaypointIndex];

        animator = GetComponent<Animator>();
        initialSpeedAnim = animator.speed;
        routeManager.listBikes.Add(gameObject);
    }

    private void Update()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);       

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);

        CheckDistanceToWaypoint(distance);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

        transform.parent = targetWaypoint;

        waypointSpeed = targetWaypoint.gameObject.GetComponent<WaypointSpeed>();

        maxSpeed = waypointSpeed.waypointSpeed;

        if (stopCycling && movementSpeed > 0.0f)
        {
            movementSpeed += -10.0f * Time.deltaTime;
            if (animator.speed > 0.0f)
            {
                animator.speed = -1.0f * Time.deltaTime;
            }
        }
        if (movementSpeed < 0.1f && stopCycling)
        {
            
            movementSpeed = 0.0f;
            animator.SetBool("OnBike", false);
            animator.speed = 1.0f;
            timerStop += Time.deltaTime;
            
        }
        if (!stopCycling && movementSpeed < maxSpeed)
        {
            movementSpeed += 3.0f * Time.deltaTime;
            animator.speed = 1.0f;
            animator.SetBool("OnBike", true);
        }

        if (!stopCycling && animator.speed < initialSpeedAnim)
        {
            animator.speed += 0.5f * Time.deltaTime;
        }
        if (movementSpeed > maxSpeed)
        {
            movementSpeed = maxSpeed;
        }
        if (animator.speed > initialSpeedAnim)
        {
            animator.speed = initialSpeedAnim;
        }
        if (timerStop > 20.0f)
        {
            stopCycling = false;
            timerStop = 0.0f;
        }
    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {

        if (targetWaypointIndex == lastWaypointIndex)
        {
            if (controllerComponent.useMaxPrefabs && !routeManager.occupied && !controllerComponent.spawnBikesByDelay)
            {
                routeManager.InstantiateBikesMaxPrefabs(controllerComponent.spawnRoutes[chosenInt].waypoints[0]);
                otherWaypointBike = routeManager.bike.GetComponent<WaypointsRoute>();
                otherWaypointBike.routeManager = controllerComponent.spawnRoutes[chosenInt];
            }
            routeManager.listBikes.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            targetWaypoint = waypointList[targetWaypointIndex];
        }
    }

    public void RedoStart()
    {
        targetWaypointIndex = 0;
        waypointList = routeManager.waypoints;
        lastWaypointIndex = waypointList.Count;
        targetWaypoint = waypointList[targetWaypointIndex];
    }

    public void ConnectRoute(int targetIndex)
    {
        targetWaypointIndex = targetIndex;
        waypointList = routeManager.waypoints;
        lastWaypointIndex = waypointList.Count;
        targetWaypoint = waypointList[targetWaypointIndex];
    }
}

