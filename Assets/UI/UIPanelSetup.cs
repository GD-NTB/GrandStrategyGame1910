using UnityEngine;

public class UIPanelSetup : MonoBehaviour
{
    void Awake()
    {
        // activate every disabled panel
        UIPanel[] panels = GetComponentsInChildren<UIPanel>(true);
        foreach (UIPanel panel in panels)
        {
            panel.gameObject.SetActive(true);

            if (panel.StartOpen) { panel.OnOpen(false); }
            else { panel.OnClose(false); }
        }
    }
}
