using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    private TabButton selectedTab;
    public List<GameObject> optionPages;

    private void Start()
    {
        //this line deactivates every option page at start; this is for convenience.
        foreach(GameObject page in optionPages)
        {
            page.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    /// <summary>
    /// when mouse is hovering over
    /// </summary>
    /// <param name="button"></param>
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || selectedTab != button)
        {
            button.background.color = tabHover;
        }

    }

    /// <summary>
    /// when mouse isn't hovering over
    /// </summary>
    /// <param name="button"></param>
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    /// <summary>
    /// when the tab selected
    /// </summary>
    /// <param name="button"></param>
    public void OnTabSelected(TabButton button)
    {
        //check if there is already selected tab, if so call deselect method in it
        if(selectedTab != null)
        {
            selectedTab.DeSelect();
        }
        //define new tab as selected tab
        selectedTab = button;
        //call the select method in the selected tab
        selectedTab.Select();
        ResetTabs();
        button.background.color = tabActive;

        //activate the related page
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<optionPages.Count; i++)
        {
            //gets the canvas group of the related page
            CanvasGroup cg = optionPages[i].GetComponent<CanvasGroup>();

            if(i == index)
            {
                cg.alpha = 1;
                cg.blocksRaycasts = true;
            }
            else
            {
                cg.alpha = 0;
            }
        }
    }
    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if(selectedTab != null && selectedTab == button) { continue; }
            button.background.color = tabIdle;
        }
    }
}
