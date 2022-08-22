using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSnowLonneker : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    public float snowAmount;
    public float waterAmount;
    private EnviroSkyMgr enviroSkyMgr;
    int i = 0;
    float p = 0;

    private void Awake()
    {                     
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].SetFloat("_SnowAmount", 0.0f);
        }
    }



    private void Update()    
    {
        if (FindObjectOfType<EnviroSkyMgr>() != null)
        {
            enviroSkyMgr = FindObjectOfType<EnviroSkyMgr>();
            if (enviroSkyMgr.GetCurrentWeatherPreset().name == "Snow")
            {
                i = i + 1;
                p = 0;
                waterAmount = -2.0f;
                for (int i = 0; i < materials.Length; ++i)
                {
                    materials[i].SetInt("Water", 0);
                    materials[i].SetFloat("_Height", -2.0f);
                }
                if (snowAmount < 1)
                {
                    snowAmount = (i / 1000.0f) % 1.2f;

                    for (int i = 0; i < materials.Length; ++i)
                    {
                        materials[i].SetFloat("_SnowAmount", snowAmount);
                    }
                }
            }
            else if (enviroSkyMgr.GetCurrentWeatherPreset().name == "Rain" ||enviroSkyMgr.GetCurrentWeatherPreset().name == "Storm")
            {
                i = 0;
                snowAmount = 0.0f;
                if (waterAmount < 2.0f)
                {
                    p = p + 0.005f;

                    for (int i = 0; i < materials.Length; ++i)
                    {
                        materials[i].SetInt("Water", 1);
                        materials[i].SetFloat("_SnowAmount", 0.0f);
                    }

                    waterAmount = (-2.0f + p);

                    
                        if (waterAmount < -0.43f)
                        {
                            materials[2].SetFloat("_Height", waterAmount);
                            materials[3].SetFloat("_Height", waterAmount);
                            materials[4].SetFloat("_Height", waterAmount);
                        }
                    
                    
                        if (waterAmount < -0.8f)
                        {
                            materials[0].SetFloat("_Height", waterAmount);
                        }
                    
                    
                        if (waterAmount < -0.28f)
                        {
                            materials[1].SetFloat("_Height", waterAmount);                            
                            materials[6].SetFloat("_Height", waterAmount);                            
                        }
                    
                    
                        if (waterAmount < -0.74f)
                        {
                            materials[5].SetFloat("_Height", waterAmount);
                        }
                    
                    
                        if (waterAmount < -0.63f)
                        {
                            materials[7].SetFloat("_Height", waterAmount);
                            materials[8].SetFloat("_Height", waterAmount);
                            materials[21].SetFloat("_Height", waterAmount);
                        }
                    
                   
                        if (waterAmount < -0.3f)
                        {
                            materials[9].SetFloat("_Height", waterAmount);
                            materials[12].SetFloat("_Height", waterAmount);
                        }
                    
                    
                        if (waterAmount < -0.37f)
                        {
                            materials[10].SetFloat("_Height", waterAmount);
                            materials[11].SetFloat("_Height", waterAmount);
                        }
                    
                   
                        if (waterAmount < -1.0f)
                        {
                            materials[13].SetFloat("_Height", waterAmount);                           
                        }
                    
                    
                        if (waterAmount < 0.4f)
                        {
                        materials[14].SetFloat("Height", waterAmount);
                            materials[18].SetFloat("_Height", waterAmount);
                            materials[20].SetFloat("_Height", waterAmount);

                        }
                    if (waterAmount < 0.86f)
                    {
                        materials[15].SetFloat("Height", waterAmount);                        
                    }
                    if (waterAmount < 0.6f)
                    {
                        materials[16].SetFloat("Height", waterAmount);
                    }
                    if (waterAmount < 0.6f)
                    {
                        materials[17].SetFloat("Height", waterAmount);
                    }
                    if (waterAmount < 0.97f)
                    {
                        materials[19].SetFloat("Height", waterAmount);
                    }

                }

            }
            else
            {
                i = 0;
                p = 0;
                for (int i = 0; i < materials.Length; ++i)
                {
                    materials[i].SetInt("Water", 0);
                    materials[i].SetFloat("_SnowAmount", 0.0f);
                    materials[i].SetFloat("_Height", -2.0f);
                }
                this.enabled = false;
            }
        }
        else
        {
            this.enabled = false;
        }
    }
}

