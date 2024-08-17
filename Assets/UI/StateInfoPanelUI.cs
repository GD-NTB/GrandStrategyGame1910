using TMPro;
using UnityEngine;

public class StateInfoPanelUI : MonoBehaviour
{
    public UIPanel StateInfoPanel { get; private set; }
    
    public TMP_Text StateNameText;
    public TMP_Text StateOccupierNameText;
    public TMP_Text OccupierText;
    public TMP_Text TilesText;

    void Start()
    {
        StateInfoPanel = UIPanel.FindByName("StateInfoPanel");

        StateNameText = StateInfoPanel.FindElementComponentByName<TMP_Text>("StateNameText");
        StateOccupierNameText = StateInfoPanel.FindElementComponentByName<TMP_Text>("StateOccupierNameText");
        OccupierText = StateInfoPanel.FindElementComponentByName<TMP_Text>("OccupierText");
        TilesText = StateInfoPanel.FindElementComponentByName<TMP_Text>("TilesText");
    }

    // todo: make ui update on its own independent rate instead of using update, we can have some global caller for this from another script
    void Update()
    {
        if (StateInfoPanel.IsOpen) { OnUIUpdate(); } // dont do anything if the panel is not open
    }

    public void OnUIUpdate()
    {
        // ------------------------------ DIPLOMACY ------------------------------
        // StateNameText
        StateNameText.text = $"State of {GameParent.gameState.SelectedState.Name}";

        // StateOccupierNameText
        StateOccupierNameText.text = $"Occupied by {GameParent.gameState.SelectedCountry.Name}";

        // OccupierText
        OccupierText.text = $"State of {GameParent.gameState.SelectedCountry.Name}";

        // TilesText
        TilesText.text = $"Tiles: {GameParent.gameState.SelectedState.TilesID.Length}";
    }
}
