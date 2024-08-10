using TMPro;
using UnityEngine;

public class MapEditorUI : MonoBehaviour
{
    public TMP_Dropdown MapViewDropdown { get; private set; }

    private TMP_Text HoveringOverTileText;

    private TMP_Text CurrentStateText;
    private TMP_Text NumberOfStatesText;

    private TMP_Text HoveringOverCountryText;
    private TMP_Text NumberOfCountriesText;

    void Start()
    {
        MapViewDropdown = GameObject.Find("MapViewDropdown").GetComponent<TMP_Dropdown>();

        HoveringOverTileText = GameObject.Find("HoveringOverTileText").GetComponent<TMP_Text>();

        CurrentStateText = GameObject.Find("CurrentStateText").GetComponent<TMP_Text>();
        NumberOfStatesText = GameObject.Find("NumberOfStatesText").GetComponent<TMP_Text>();

        HoveringOverCountryText = GameObject.Find("HoveringOverCountryText").GetComponent<TMP_Text>();
        NumberOfCountriesText = GameObject.Find("NumberOfCountriesText").GetComponent<TMP_Text>();
    }

    void Update()
    {
        HoveringOverTileText.text = $"Hovering over tile:\n{MapParent.mapUtils.GetTileAtMouse()}";

        CurrentStateText.text = $"Current state:\n{MapParent.mapEditorCore.SelectedState}";
        NumberOfStatesText.text = $"Number of states: {MapParent.mapState.States.Length}";

        HoveringOverCountryText.text = $"Current country:\n{MapParent.mapUtils.GetCountryAtCoords(MapParent.mapUtils.GetCoordsAtMouse())}";
        NumberOfCountriesText.text = $"Number of countries: {MapParent.mapState.Countries.Count}";
    }

    public void OnSaveDataButtonClick()
    {
        // MapParent.mapEditorCore.SaveDataToFile();
    }

    public void OnLoadDataButtonClick()
    {
        // MapParent.mapEditorCore.LoadDataFromFile();
    }
}
