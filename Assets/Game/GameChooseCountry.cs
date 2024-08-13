using TMPro;
using UnityEngine;

public class GameChooseCountry : MonoBehaviour
{
    // ui
    private UIPanel choosingCountryPanel;
    private TMP_Text testText;

    void Start()
    {
        // get references to ui Elements
        choosingCountryPanel = UIPanel.FindByName("ChoosingCountryPanel");
        testText = choosingCountryPanel.FindElementComponentByName<TMP_Text>("TestText");

        choosingCountryPanel.OnOpen(false);
    }

    void Update()
    {
        if (!GameParent.gameState.IsChoosingCountry) { return; }
        UpdateChoosingCountry();

        // testPanel.text = GameParent.gameState.IsChoosingCountry.ToString();
        if (choosingCountryPanel.IsOpen)
        {
            testText.text = Time.frameCount.ToString();
        }
    }
    
    public void StartChoosingCountry()
    {
        GameParent.gameState.IsChoosingCountry = true;

        // ui
        
        print("choosing country...");
    }

    private void UpdateChoosingCountry()
    {
        // on country click
        if (GameParent.gameCore.IsMouseOnLand && Input.GetMouseButtonDown(0))
        {
            Country country = MapParent.mapUtils.GetCountryAtCoords(MapParent.mapUtils.GetCoordsAtMouse(), true);
            ChooseCountry(country);
        }
    }

    private void ChooseCountry(Country country)
    {
        GameParent.gameState.IsChoosingCountry = false;
        GameParent.gameState.PlayerCountry = country;

        choosingCountryPanel.OnClose(false);
        UIPanel.FindByName("BasePanel").OnOpen(false);

        print($"selected country {country.Name}");
    }
}
