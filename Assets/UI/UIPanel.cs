using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour // REMEMBER, ONLY UPDATE UI ELEMENTS IF IsOpen, ELSE ITS A WASTE!!!! 
{
    public static List<UIPanel> AllUIPanels = new List<UIPanel>(); // static reference of all ui panels
    
    public static UIPanel FindByName(string name)
    {
        // linear search every ui panel
        foreach (UIPanel uiPanel in AllUIPanels)
        {
            if (uiPanel.Name == name)
            {
                return uiPanel;
            }
        }
        return null;
    }

    public GameObject FindElementByName(string name)
    {
        // linear search every ui panel
        foreach (Transform element in transform)
        {
            if (element.name == name)
            {
                return element.gameObject;
            }
        }
        Debug.LogWarning($"unable to find ui element '{name}'");
        return null;
    }

    public T FindElementComponentByName<T>(string name)
    {
        GameObject element = FindElementByName(name);
        if (element == null) { return default; } // awrning log is done in previous function
        return element.GetComponent<T>();
    }



    public string Name; // name of this ui panel
    public float SortOrder;  // the sort order of this ui panel
    public bool StartOpen;  // if the panel is open and elements are active on start

    [ReadOnly] public bool IsOpen; // { get; private set; }

    // public UIPanel(string name, float sortOrder, bool startOpen, List<GameObject> elements)
    // {
    //     Name = name;
    //     SortOrder = sortOrder;
    //     StartOpen = startOpen;
    // }



    void Awake()
    {
        // add to all static list of all ui panels
        AllUIPanels.Add(this);
        
        // start open logic
        if (StartOpen) { OnOpen(false); }
        else { OnClose(false); }
    }

    void Update()
    {
        // set sorting order
        gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, SortOrder);
    }



    public void OnOpen(bool doTransition=true)
    {
        foreach (Transform element in transform)
        {
            element.gameObject.SetActive(true);
        }

        if (doTransition)
        {
            // todo: do transition and stuff here
        }

        // also do sound here
        // we can have an enum describing what type of menu this is
        // therefore we can use different sounds for different menus

        IsOpen = true;
    }

    public void OnClose(bool doTransition=true)
    {
        foreach (Transform element in transform)
        {
            element.gameObject.SetActive(false);
        }

        if (doTransition)
        {
            // todo: do transition and stuff here
        }

        IsOpen = false;
    }
}
