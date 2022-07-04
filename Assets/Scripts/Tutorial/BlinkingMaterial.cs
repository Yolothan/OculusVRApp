using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingMaterial : MonoBehaviour
{       
    public Renderer rendererMat;
    public Material material;       
    public float fadeOutTime;
    public float fadeInTime;
    

    
    // Start is called before the first frame update
    public void StartFading()
    {
        rendererMat.material = material;
        StartCoroutine(FadeOut(material));
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator FadeOut(Material material)
    {
        float elapsedTime = 0.0f;
        Color c = material.color;
        while(elapsedTime < fadeOutTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeOutTime);
            material.color = c;
            
        }
        
        StartCoroutine(FadeIn(material));
    }

    IEnumerator FadeIn(Material material)
    {
        float elapsedTime = 0.0f;
        Color c = material.color;
        while (elapsedTime < fadeInTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeInTime);
            material.color = c;
        }
        StartCoroutine(FadeOut(material));
    }
}
