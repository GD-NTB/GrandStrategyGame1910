using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapState : MonoBehaviour
{
    // ---------------------------------------------------- MAP DATA ----------------------------------------------------
    public GameTile[] Tiles = new GameTile[0];
    public State[] States = new State[0];
    // get dictionary of tiles in a specific row from a y value, then a specific tile can be found by looking up its x position
    public Dictionary<int, Dictionary<int, int>> TileIDDictionary = new Dictionary<int, Dictionary<int, int>>(); // (y, x)

    public Dictionary<Color, int> StateMapColourToStateID = new Dictionary<Color, int>();
    public Dictionary<int, GameTile> TileIDToTile = new Dictionary<int, GameTile>();
    public Dictionary<int, State> StateIDToState = new Dictionary<int, State>();
    
    public enum MapView // always static
    {
        Countries = 0,
        States = 1,
        Tiles = 2
    }
    public MapView CurrentMapView { get; private set; }

    public void ChangeCurrentMapView(MapView newMapView)
    {
        CurrentMapView = newMapView;

        // deselect currently selected state if in map editor mode
        if (SceneManager.GetActiveScene().name == "MapEditor")
        {
            MapParent.mapEditorCore.SelectState(null);
            MapParent.mapEditorUI.MapViewDropdown.SetValueWithoutNotify((int)newMapView);
        }

        MapParent.mapUtils.RedrawMap();
    }

    public void ChangeCurrentMapView(int newMapView)
    {
        ChangeCurrentMapView((MapView)newMapView);
    }

    // ---------------------------------------------------- BUILDING DATA ----------------------------------------------------

    public List<BuildingType> Buildings = new List<BuildingType>();

    // ---------------------------------------------------- COUNTRY DATA ----------------------------------------------------

    public List<Country> Countries = new List<Country>();
    public Dictionary<string, Country> CountryTagToCountry = new Dictionary<string, Country>();
    public Dictionary<Color, Country> CountryColourToCountry = new Dictionary<Color, Country>(); // MORE DICTIONARIES NOW!!!
}