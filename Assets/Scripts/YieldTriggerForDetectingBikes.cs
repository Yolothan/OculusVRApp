using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnTheGameOn.SimpleTrafficSystem;

public class YieldTriggerForDetectingBikes : MonoBehaviour
{
    [HideInInspector]
    public bool bikerInBox = false, carInBox = false;   


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TriggerBike"))
        {
            bikerInBox = true;            
        }          
        else if(other.CompareTag("CarPrefab"))
        {
            carInBox = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerBike"))
        {
            bikerInBox = false;
        }        
        else if(other.CompareTag("CarPrefab"))
        {
            carInBox = false;
        }
    }


}
