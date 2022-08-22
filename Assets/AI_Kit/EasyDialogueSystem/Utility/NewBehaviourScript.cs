using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public ESDialogueManager manager;
    public float mytime = 0;
    private bool Not_showed_ctrl;

    // Start is called before the first frame update
    void Awake()
    {
        manager = this.GetComponent<ESDialogueManager>();
        manager.TriggerLayer(1003281, 8.0f);
    }
}
