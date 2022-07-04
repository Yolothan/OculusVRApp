using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameEvent : MonoBehaviour
{
    public List<SphereCollider> sphereColliders;
    public ESDialogueManager manager;
    public ESDialogueManager quit_manager;
    // Start is called before the first frame update
    void Start()
    {
        if (sphereColliders.Count > 0)
        {
            for (int i = 0; i < sphereColliders.Count; ++i)
            {
                sphereColliders[i].enabled = false;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (manager != null)
        {
            if (manager.HasPlayed == true)
            {
                for (int i = 0; i < sphereColliders.Count; ++i)
                {
                    sphereColliders[i].enabled = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quit_manager.IsPlaying == false)
            {
                quit_manager.TriggerLayer(1004845, 0.0f);
            }
        }
        //
        string id = quit_manager.GetSelectedDecisionID();
        if (id == "1004542GV")
        {
            //print("Kill");
         #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
         #else
            Application.Quit();
         #endif 
          
        }
    }
}
