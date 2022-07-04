using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControl : MonoBehaviour
{
    public ESDialogueManager manager;
    public ESDialogueManager othermanger;
    public float mytime = 0;
    private bool Not_showed_ctrl;
  
    void Update()
    {
        //time delta
        //mytime += 0.019f;
        if (othermanger.IsPlaying == false && Not_showed_ctrl == false)
        {
            mytime += 0.019f;
            if (mytime > 2f)
            {
                //print("Pussy");  
                manager.TriggerLayer(1006187, 0.0f);
                Not_showed_ctrl = true;
            }
        }
    }
}
