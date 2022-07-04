using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Image))]
public class Tabs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{  
    public TabGroup tabGroup;

    [HideInInspector]
    public Image backGround;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    private void Start()
    {
        backGround = GetComponent<Image>();
        
    }

    public void Select()
    {
        if(onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }   
}

