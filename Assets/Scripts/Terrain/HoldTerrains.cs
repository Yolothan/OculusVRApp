using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTerrains : MonoBehaviour
{
    public GameObject summer, winter, spring, autumn;
    private TerrainHandler terrainHandler;
    // Start is called before the first frame update
    void Start()
    {
        terrainHandler = FindObjectOfType<TerrainHandler>();
        terrainHandler.summer = summer;
        terrainHandler.winter = winter;
        terrainHandler.spring = spring;
        terrainHandler.autumn = autumn;
        terrainHandler.summer.SetActive(true);
        terrainHandler.winter.SetActive(false);
        terrainHandler.autumn.SetActive(false);
        terrainHandler.spring.SetActive(false);
        terrainHandler.ChangeLeavesSummer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
