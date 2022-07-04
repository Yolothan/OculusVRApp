using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSituation : MonoBehaviour
{
    private ChangeSituationLonneker changeLonneker;
    [SerializeField]
    private Button huidigeSituatie, toekomstigeSituatie, lowPoly, highPoly;
    private ColorBlock oldBlock, newBlock, lowBlock, highBlock;    

    private void Start()
    {        
        oldBlock = huidigeSituatie.colors;

        highBlock = highPoly.colors;

        oldBlock.normalColor = new Color32(255, 117, 117, 255);

        highBlock.normalColor = new Color32(255, 117, 117, 255);

        huidigeSituatie.colors = oldBlock;

        highPoly.colors = highBlock;

        newBlock = toekomstigeSituatie.colors;

        lowBlock = lowPoly.colors;

        newBlock.normalColor = new Color32(255, 255, 255, 255);

        lowBlock.normalColor = new Color32(255, 255, 255, 255);

        toekomstigeSituatie.colors = newBlock;

        lowPoly.colors = lowBlock;        
    }
    public void New()
    {
        if(FindObjectOfType<CarController>() != null)
        {
            changeLonneker = FindObjectOfType<ChangeSituationLonneker>();
            changeLonneker.ChangeRoutes();            

            oldBlock.normalColor = new Color32(255, 255, 255, 255);

            huidigeSituatie.colors = oldBlock;            

            newBlock.normalColor = new Color32(255, 117, 117, 255);

            toekomstigeSituatie.colors = newBlock;
        }
    }

    public void Old()
    {
        if (FindObjectOfType<CarController>() != null)
        {
            changeLonneker = FindObjectOfType<ChangeSituationLonneker>();
            changeLonneker.ChangeRoutesBack();

            oldBlock.normalColor = new Color32(255, 117, 117, 255);

            huidigeSituatie.colors = oldBlock;            

            newBlock.normalColor = new Color32(255, 255, 255, 255);

            toekomstigeSituatie.colors = newBlock;
        }
    }

    public void HighPoly()
    {
        changeLonneker = FindObjectOfType<ChangeSituationLonneker>();
        changeLonneker.HighPoly();

        highBlock.normalColor = new Color32(255, 117, 117, 255);

        highPoly.colors = highBlock;

        lowBlock.normalColor = new Color32(255, 255, 255, 255);

        lowPoly.colors = lowBlock;
    }
    public void LowPoly()
    {
       
            changeLonneker = FindObjectOfType<ChangeSituationLonneker>();
            changeLonneker.LowPoly();

            highBlock.normalColor = new Color32(255, 255, 255, 255);

            highPoly.colors = highBlock;

            lowBlock.normalColor = new Color32(255, 117, 117, 255);

            lowPoly.colors = lowBlock;
       
    }
}
