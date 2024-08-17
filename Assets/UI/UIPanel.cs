using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public bool StartOpen;  // if the panel is open and elements are active on start

    [ReadOnly] public bool IsOpen; // { get; private set; }



    void Awake()
    {
        // add to all static list of all ui panels
        AllUIPanels.Add(this);
        
        // start open logic is handled by UIPanelSetup
        
        print($"{Name} setup");
    }



    public void OnOpen(bool doTransition=true)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<CanvasRenderer>(out CanvasRenderer renderer))
            {
                renderer.SetAlpha(1);
            }
        }

        if (doTransition)
        {
            // todo: do transition and stuff here
            print($"opened panel {Name}");
        }

        // also do sound here
        // we can have an enum describing what type of menu this is
        // therefore we can use different sounds for different menus

        IsOpen = true;
    }

    public void OnClose(bool doTransition=true)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<CanvasRenderer>(out CanvasRenderer renderer))
            {
                renderer.SetAlpha(0);
            }
        }

        if (doTransition)
        {
            // todo: do transition and stuff here
            print($"closed panel {Name}");
        }

        IsOpen = false;
    }

    // i dont have to wake up
    // i dont have to be here
    // you dont have to love me
    // i dont need a godsend
    // just love it when its bright out
    // the sunrise is calling
    // i feel at peace
    // i could believe
    public void OnCloseButton()
    {
        GameParent.gameCore.DeselectState();
    }
}
