using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESIsMobile : MonoBehaviour
{
    /*
    attach this to the pedestrain it will boost perfomance by disabling the ped contoller class based
    on camera veiw , if you dont like what it does please just remove component thats all :{} 
    */
    // Start is called before the first frame update
    [Header("Requires a Triggered Sphere Collider")]
    public Renderer bodymeshrenderer;
    [Tooltip("Helps Boost Performance by delecting overlapped peds")]
    public bool SmartDelect = false;
    [HideInInspector] public Material backbodymeshrenderer;
    public Renderer[] extras;
    public Material Tranperacy;
    [HideInInspector] public bool IsSet = true;
    void Start()
    {
        backbodymeshrenderer = bodymeshrenderer.sharedMaterial;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsSet == false)
        {
            if (SmartDelect)
                Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (bodymeshrenderer.isVisible)
        {
            if (!IsSet)
            {
                this.GetComponent<ESPedestrains>().enabled = true;
                bodymeshrenderer.sharedMaterial = backbodymeshrenderer;
                if (extras.Length > 0)
                    for (int i = 0; i < extras.Length; i++)
                    {
                        extras[i].enabled = true;
                    }
                IsSet = true;
            }
        }
        else
        {
            if (IsSet)
            {
                this.GetComponent<ESPedestrains>().enabled = false;
                bodymeshrenderer.sharedMaterial = Tranperacy;
                if (extras.Length > 0)
                {
                    for (int i = 0; i < extras.Length; i++)
                    {
                        extras[i].enabled = false;
                    }
                }
                IsSet = false;
            }
        }
    }
}
