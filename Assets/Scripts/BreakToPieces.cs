using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakToPieces : MonoBehaviour
{
    [SerializeField]
    GameObject goudappelShattered, vrShattered, productionShattered, appleShattered, colliderObject, virtualReality, production, apple;
    [Space(20)]
    int i = 0;
    [SerializeField]
    AudioClip firstClip, secondClip;
    AudioSource audioGoudappel, audioVirtualReality, audioProduction, audioApple;
    Animation animGoudappel;
    private void Start()
    {
        production.SetActive(false);
        virtualReality.SetActive(false);
        apple.SetActive(false);
        audioGoudappel = gameObject.GetComponent<AudioSource>();
        audioVirtualReality = virtualReality.GetComponent<AudioSource>();
        audioProduction = production.GetComponent<AudioSource>();
        audioApple = apple.GetComponent<AudioSource>();
        animGoudappel = gameObject.GetComponent<Animation>();

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            Instantiate(goudappelShattered, gameObject.transform.position, gameObject.transform.rotation);
            GetComponent<Rigidbody>().AddExplosionForce(800, transform.position, 10);
            Destroy(gameObject);

            Instantiate(vrShattered, virtualReality.transform.position, virtualReality.transform.rotation);
            virtualReality.GetComponent<Rigidbody>().AddExplosionForce(800, virtualReality.transform.position, 10);
            Destroy(virtualReality);

            Instantiate(productionShattered, production.transform.position, production.transform.rotation);
            production.GetComponent<Rigidbody>().AddExplosionForce(800, production.transform.position, 10);
            Destroy(production);

            Instantiate(appleShattered, apple.transform.position, apple.transform.rotation);
            apple.GetComponent<Rigidbody>().AddExplosionForce(800, apple.transform.position, 10);
            Destroy(apple);                  
        }
        if(other.tag == "TriggerScene")
        {
            SceneLoader.Instance.LoadNewScene("Menu");
        }
    }   
    private void FixedUpdate()
    {                    
        i = i + 1;
        
        if(i == 100)
        {
            animGoudappel.Play();
            production.SetActive(true);
            virtualReality.SetActive(true);
            apple.SetActive(true);
            gameObject.layer = 0;
        }
        if (i == 120)
        {
            audioGoudappel.Play();
            virtualReality.layer = 0;
        }
        if (i == 150)
        {
            audioVirtualReality.Play();
            production.layer = 0;
        }
        if (i == 170)
        {
            audioProduction.Play();
            apple.layer = 0;
        }
        if (i == 190)
        {
            audioApple.clip = firstClip;
            audioApple.Play();
        }
        if (i == 210)
        {
            audioApple.clip = secondClip;
            audioApple.Play();
        }
        if (i == 378)
        {
            colliderObject.SetActive(true);            
        }
        if (i == 379)
        {
            Destroy(colliderObject);
        }              
    }
}
