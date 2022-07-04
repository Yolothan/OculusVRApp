using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ESManagerCtrl : MonoBehaviour
{
    [HideInInspector]public ESDialogueManager[] managers;
    [HideInInspector]public ESDialogueManager PlayingManager;
    [HideInInspector]public ESDialogueManager Dormantmanager;
    // Update is called once per frame
    private void Update()
    {
        managers = GameObject.FindObjectsOfType<ESDialogueManager>();
        if (managers.Length > 0)
        {
            for (int i = 0; i < managers.Length; ++i)
            {
                if (managers[i].DialogueGraph != null)
                {
                    if (PlayingManager == null)
                    {
                        if (managers[i].IsPlaying)
                        {
                            PlayingManager = managers[i];
                        }
                    
                    }
                    if (PlayingManager != null)
                    {
                        if (managers[i].IsPlaying)
                        {
                            if (PlayingManager != managers[i])
                            {
                                Dormantmanager = managers[i];
                            }
                        }
                        if (Dormantmanager != null)
                        {
                           PlayingManager.Stop(0.0f);
                           PlayingManager = Dormantmanager;
                           Dormantmanager = null;
                        }
                    } 
                }
            }
        }
    }
}
