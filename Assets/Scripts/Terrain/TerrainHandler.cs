using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TerrainHandler : MonoBehaviour
{
             
    public Material grass, crops, grassWinter, cropsWinter, ice, water, street, trottoir, fietspad, asfalt, straatstenen3;
    
    public Renderer[] grassRenderer;   
    
    public GameObject summer, winter, autumn, spring, summerImage, winterImage, autumnImage;

    [SerializeField]
    Renderer[] waterModel;
    [SerializeField]
    Material[] materials;

    private void Start()
    {
        ChangeLeavesSummer();
    }

    public void ChangeLeavesSummer()
    {
        winter.SetActive(false);
        spring.SetActive(false);
        autumn.SetActive(false);
        summer.SetActive(true);

        winterImage.SetActive(false);
        summerImage.SetActive(true);
        autumnImage.SetActive(false);
        

        grass.color = new Color32(119, 216, 101, 255);
        crops.color = new Color32(142, 142, 142, 255);
        EnviroSkyMgr.instance.ChangeWeather("Clear Sky");

        Material[] mats1 = grassRenderer[0].materials;
        mats1[0] = grass;
        mats1[4] = trottoir;
        mats1[14] = straatstenen3;
        grassRenderer[0].materials = mats1;

        Material[] mats2 = grassRenderer[1].materials;
        mats2[0] = street;
        mats2[1] = street;
        mats2[4] = straatstenen3;
        mats2[5] = trottoir;
        mats2[6] = asfalt;
        mats2[9] = grass;
        mats2[10] = fietspad;
        grassRenderer[1].materials = mats2;

        Material[] mats3 = grassRenderer[2].materials;
        mats3[0] = crops;
        mats3[1] = grass;
        grassRenderer[2].materials = mats3;

        Material[] mats4 = grassRenderer[3].materials;
        mats4[3] = street;
        mats4[6] = street;
        mats4[10] = grass;
        grassRenderer[3].materials = mats4;        

        Material[] mats5 = grassRenderer[4].materials;
        mats5[3] = trottoir;
        mats5[6] = street;
        mats5[7] = fietspad;
        mats5[8] = asfalt;
        mats5[13] = straatstenen3;
        mats5[23] = grass;
        mats5[28] = street;
        grassRenderer[4].materials = mats5;                 

        for (int i=0; i < materials.Length; ++i )
        {
            materials[i].color = new Color32(255, 255, 255, 255);
        }
        for (int i = 0; i < waterModel.Length; ++i)
        {
            Material[] materials = new Material[waterModel[i].materials.Length];
            for (int j = 0; j < materials.Length; ++j)
            {
                materials[j] = water;
            }
            waterModel[i].materials = materials;
        }
    }

    public void ChangeLeavesWinter()
    {
        winterImage.SetActive(true);
        summerImage.SetActive(false);
        autumnImage.SetActive(false);
        

        grass.color = new Color32(226, 101, 101, 255);
        crops.color = new Color32(255, 146, 0, 255);

        EnviroSkyMgr.instance.ChangeWeather(4);

        Material[] mats1 = grassRenderer[0].materials;
        mats1[0] = grassWinter;
        mats1[4] = trottoir;
        mats1[14] = straatstenen3;
        grassRenderer[0].materials = mats1;

        Material[] mats2 = grassRenderer[1].materials;
        mats2[0] = street;
        mats2[1] = street;
        mats2[4] = straatstenen3;
        mats2[5] = trottoir;
        mats2[6] = asfalt;
        mats2[9] = grassWinter;
        mats2[10] = fietspad;
        grassRenderer[1].materials = mats2;

        Material[] mats3 = grassRenderer[2].materials;
        mats3[0] = cropsWinter;
        mats3[1] = grassWinter;
        grassRenderer[2].materials = mats3;

        Material[] mats4 = grassRenderer[3].materials;
        mats4[3] = street;
        mats4[6] = street;
        mats4[10] = grassWinter;
        grassRenderer[3].materials = mats4;

        Material[] mats5 = grassRenderer[4].materials;
        mats5[3] = trottoir;
        mats5[6] = street;
        mats5[7] = fietspad;
        mats5[8] = asfalt;
        mats5[13] = straatstenen3;
        mats5[23] = grassWinter;
        mats5[28] = street;
        grassRenderer[4].materials = mats5;


        winter.SetActive(true);
        spring.SetActive(false);
        autumn.SetActive(false);
        summer.SetActive(false);

        //Materials Model
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = new Color32(255, 255, 255, 0);
        }
        
        for (int i = 0; i < waterModel.Length; ++i)
        {
            Material[] materials = new Material[waterModel[i].materials.Length];
            for (int j = 0; j < materials.Length; ++j)
            {
                materials[j] = ice;
            }
            waterModel[i].materials = materials;
        }

    }
    public void ChangeLeavesSpring()
    {
               
        EnviroSkyMgr.instance.ChangeWeather(1);
        grass.color = new Color32(119, 216, 101, 255);
        crops.color = new Color32(142, 142, 142, 255);

        winterImage.SetActive(false);
        summerImage.SetActive(true);
        autumnImage.SetActive(false);
        

        Material[] mats1 = grassRenderer[0].materials;
        mats1[0] = grass;
        mats1[4] = trottoir;
        mats1[14] = straatstenen3;
        grassRenderer[0].materials = mats1;

        Material[] mats2 = grassRenderer[1].materials;
        mats2[0] = street;
        mats2[1] = street;
        mats2[4] = straatstenen3;
        mats2[5] = trottoir;
        mats2[6] = asfalt;
        mats2[9] = grass;
        mats2[10] = fietspad;
        grassRenderer[1].materials = mats2;

        Material[] mats3 = grassRenderer[2].materials;
        mats3[0] = crops;
        mats3[1] = grass;
        grassRenderer[2].materials = mats3;

        Material[] mats4 = grassRenderer[3].materials;
        mats4[3] = street;
        mats4[6] = street;
        mats4[10] = grass;
        grassRenderer[3].materials = mats4;

        Material[] mats5 = grassRenderer[4].materials;
        mats5[3] = trottoir;
        mats5[6] = street;
        mats5[7] = fietspad;
        mats5[8] = asfalt;
        mats5[13] = straatstenen3;
        mats5[23] = grass;
        mats5[28] = street;
        grassRenderer[4].materials = mats5;

        winter.SetActive(false);
        spring.SetActive(true);
        autumn.SetActive(false);
        summer.SetActive(false);

        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = new Color32(255, 199, 0, 255);
        }
        for (int i = 0; i < waterModel.Length; ++i)
        {
            Material[] materials = new Material[waterModel[i].materials.Length];
            for (int j = 0; j < materials.Length; ++j)
            {
                materials[j] = water;
            }
            waterModel[i].materials = materials;
        }
    }

    public void ChangeLeavesAutumn()
    {
              
        EnviroSkyMgr.instance.ChangeWeather(3);
        grass.color = new Color32(226, 101, 101, 255);
        crops.color = new Color32(255, 146, 0, 255);

        winterImage.SetActive(false);
        summerImage.SetActive(false);
        autumnImage.SetActive(true);
       

        Material[] mats1 = grassRenderer[0].materials;
        mats1[0] = grassWinter;
        mats1[4] = trottoir;
        mats1[14] = straatstenen3;
        grassRenderer[0].materials = mats1;

        Material[] mats2 = grassRenderer[1].materials;
        mats2[0] = street;
        mats2[1] = street;
        mats2[4] = straatstenen3;
        mats2[5] = trottoir;
        mats2[6] = asfalt;
        mats2[9] = grassWinter;
        mats2[10] = fietspad;
        grassRenderer[1].materials = mats2;

        Material[] mats3 = grassRenderer[2].materials;
        mats3[0] = cropsWinter;
        mats3[1] = grassWinter;
        grassRenderer[2].materials = mats3;

        Material[] mats4 = grassRenderer[3].materials;
        mats4[3] = street;
        mats4[6] = street;
        mats4[10] = grassWinter;
        grassRenderer[3].materials = mats4;

        Material[] mats5 = grassRenderer[4].materials;
        mats5[3] = trottoir;
        mats5[6] = street;
        mats5[7] = fietspad;
        mats5[8] = asfalt;
        mats5[13] = straatstenen3;
        mats5[23] = grassWinter;
        mats5[28] = street;
        grassRenderer[4].materials = mats5;

        winter.SetActive(false);
        spring.SetActive(false);
        autumn.SetActive(true);
        summer.SetActive(false);
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].color = new Color32(224, 118, 50, 255);
        }
        for (int i = 0; i < waterModel.Length; ++i)
        {
            Material[] materials = new Material[waterModel[i].materials.Length];
            for (int j = 0; j < materials.Length; ++j)
            {
                materials[j] = water;
            }
            waterModel[i].materials = materials;
        }
    }  
}
