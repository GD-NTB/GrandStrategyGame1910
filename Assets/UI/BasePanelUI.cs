using TMPro;
using UnityEngine;

public class BasePanelUI : MonoBehaviour
{
    public UIPanel BasePanel { get; private set; }

    public UIPanel DiplomacyRegionPanel { get; private set; }
    public TMP_Text CountryNameText;
    public TMP_Text StabilityPercentText;

    public UIPanel MilitaryRegionPanel { get; private set; }
    public UIPanel EconomyRegionPanel { get; private set; }
    public UIPanel MiscRegionPanel { get; private set; }

    void Start()
    {
        BasePanel = UIPanel.FindByName("BasePanel");

        // ------------------------------ DIPLOMACY ------------------------------
        DiplomacyRegionPanel = UIPanel.FindByName("DiplomacyRegionPanel");
        CountryNameText = DiplomacyRegionPanel.FindElementComponentByName<TMP_Text>("CountryNameText");
        StabilityPercentText = DiplomacyRegionPanel.FindElementComponentByName<TMP_Text>("StabilityPercentText");

        // ------------------------------ MILITARY ------------------------------
        MilitaryRegionPanel = UIPanel.FindByName("MilitaryRegionPanel");

        // ------------------------------ ECONOMY ------------------------------
        EconomyRegionPanel = UIPanel.FindByName("EconomyRegionPanel");

        // ------------------------------ MISC ------------------------------
        MiscRegionPanel = UIPanel.FindByName("MiscRegionPanel");
    }

    // todo: make ui update on its own independent rate instead of using fixed update
    void FixedUpdate()
    {
        OnUIUpdate();
    }

    public void OnUIUpdate()
    {
        if (!BasePanel.IsOpen) { return; } // dont do anything if the panel is not open

        // ------------------------------ DIPLOMACY ------------------------------
        CountryNameText.text = GameParent.gameState.PlayerCountry.Name;
        StabilityPercentText.text = GameParent.gameState.PlayerCountry.StabilityPercent();
    }
}
