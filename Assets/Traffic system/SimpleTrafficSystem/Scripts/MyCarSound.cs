using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyCarSound : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject miniMapImageHazerswoude, miniMapImageLonneker;
    public float minPitch = 0.05f;
    private float pitchFromCar;    
    public AudioClip drivingAudio;    
    TurnTheGameOn.SimpleTrafficSystem.AITrafficCar trafficCar;
    bool audioAlreadyPlayed = false;
    bool audio2AlreadyPlayed = false;
    

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = minPitch;
        trafficCar = gameObject.GetComponent<TurnTheGameOn.SimpleTrafficSystem.AITrafficCar>();
        if (sceneName == "Lonneker")
        {
            miniMapImageLonneker.SetActive(true);
            miniMapImageHazerswoude.SetActive(false);
        }
        else if (sceneName == "Hazerswoude")
        {
            miniMapImageLonneker.SetActive(false);
            miniMapImageHazerswoude.SetActive(true);
        }
        else
        {
            miniMapImageLonneker.SetActive(false);
            miniMapImageHazerswoude.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pitchFromCar = TurnTheGameOn.SimpleTrafficSystem.AITrafficCar.cc.carCurrentSpeed;
        if (!trafficCar.IsBraking())
        {
            audioAlreadyPlayed = false;
            audioSource.clip = drivingAudio;
            audioSource.loop = true;
            if (pitchFromCar < minPitch)
            {
                audioSource.pitch = minPitch;
            }
            else
            {
                audioSource.pitch = pitchFromCar;
            }
            if (!audio2AlreadyPlayed)
            {
                audioSource.Play();
                audio2AlreadyPlayed = true;
            }


        }
        else
        {
            audio2AlreadyPlayed = false;            
            audioSource.loop = false;
            audioSource.pitch = 1;

            if (!audioAlreadyPlayed)
            {
                audioSource.Play();
                audioAlreadyPlayed = true;
            }
        }        
    }
}
