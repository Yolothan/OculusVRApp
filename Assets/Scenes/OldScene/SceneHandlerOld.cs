using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandlerOld : MonoBehaviour
{
    int i = 0;
    [SerializeField]
    GameObject triggerScene;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TriggerScene")        {
            
            SceneLoader.Instance.LoadNewScene("Menu");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        i = i + 1;

        if(i == 650)
        {
            triggerScene.SetActive(true);
        }
    }
}
