
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;

public class AddTeleportArea : MonoBehaviour
{   
    void Start()
    {        
        TeleportationArea ta = gameObject.AddComponent<TeleportationArea>() as TeleportationArea;
    }    
}
