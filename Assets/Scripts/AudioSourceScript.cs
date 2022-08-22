using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceScript : MonoBehaviour
{
    [SerializeField]
    AudioClip firstClip, secondClip;
    int i = 0;
    
    void FixedUpdate()
    {        
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        i = i + 1;

        if(i == 100)
        {
            audio.clip = firstClip;
            audio.Play();
        }
        if(i == 378)
        {
            audio.clip = secondClip;
            audio.Play();
        }        
    }
}
