using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESHumanTrafficManager : MonoBehaviour
{
    [HideInInspector] public GameObject Player;
    [HideInInspector] public float dist;
    [HideInInspector] public bool DeleteBasednplayer;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (DeleteBasednplayer == false) return;
        if (Player == null) return;
        //kills all pedestrain that goes too far from player's position.
        if (Vector3.Distance(Player.transform.position, this.transform.position) > dist)
        {
            Destroy(this.gameObject);
        }
    }
    //

}
