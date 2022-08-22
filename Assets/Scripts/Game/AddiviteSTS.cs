using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddiviteSTS : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.Instance.LoadSceneAdditive("X-Huidig");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
