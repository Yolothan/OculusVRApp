using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{

    public List<Tabs> tabButtons;
    public Sprite tabIdle, tabHover, tabActive;
    [HideInInspector]
    public Tabs selectedTab;
    public List<GameObject> objectsToSwap;

    
    public void Subscribe(Tabs button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<Tabs> ();
        }

        tabButtons.Add (button);
    }
    
    public void OnTabEnter(Tabs button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.backGround.sprite = tabHover;
            button.backGround.color = new Color32(255, 255, 255, 255);
        }
    }

   public void OnTabExit(Tabs button)
    {
        ResetTabs();
    }

    public void OnTabSelected(Tabs button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();        
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
        button.backGround.sprite = tabActive;
        button.backGround.color = new Color32(255, 255, 255, 255);
    }

    public void ResetTabs()
    {
        
        foreach(Tabs button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab )
            {
                continue;
            }
            button.backGround.sprite = tabIdle;
            button.backGround.color = new Color32(255,255, 255, 0);
        }
    }
}
