using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] toggleObjects;
    [SerializeField]
    private Sprite oldSprite, newSprite;
    [SerializeField]
    private Image image;
    

    public void ToggleObjects()
    {
        for (int i = 0; i < toggleObjects.Length; i++)
        {
            toggleObjects[i].SetActive(!toggleObjects[i].activeSelf);
            if (oldSprite != null && newSprite != null && image != null)
            {
                if (image.sprite == oldSprite)
                {
                    image.sprite = newSprite;
                }
                else
                {
                    image.sprite = oldSprite;
                }
            }
        }
    }
}
