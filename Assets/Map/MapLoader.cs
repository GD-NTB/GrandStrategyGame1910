using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public void CreateEverything()
    {
        // ------------------------------------------------------------
        float startTime = Time.realtimeSinceStartup;
        // ------------------------------------------------------------

        MapParent.mapCreator.CreateTiles();
        LoadMapData();
        
        LoadBuildingData();
        MapParent.mapCreator.CreateBuildings();

        LoadCountryData();
        LoadOccupationData();
        // MapParent.mapUtils.RedrawMap();

        // ------------------------------------------------------------
        float endTime = Time.realtimeSinceStartup; // for benchmarking
        print($"took {endTime - startTime} seconds to create everything");
        // ------------------------------------------------------------
    }

    // ---------------------------------------------------- LOAD MAP DATA ----------------------------------------------------
    public void LoadMapData()
    {
        // load textures
        StatesMapTexture = Resources.Load("Map/StatesMap") as Texture2D;
        OccupationMapTexture = Resources.Load("Countries/OccupationMap") as Texture2D;

        // LoadTilesData(); // loading tile data is not a thing - all tile data is assigned through loading everything else!
        LoadStatesData(); // tiles in state, etc
        CreateColourStatesIDDict(); // create colour to state id dict
        CreateTileIDDict(); // create tile id to tile dict
        CreateStateIDDict(); // create state id to state dict
        AssignStateTileIDs(); // set State.TilesID for every state
    }

    private TextAsset statesFile;
    public void LoadStatesData()
    {
        // load states from file
        statesFile = Resources.Load("Map/States") as TextAsset;
        MapParent.mapState.States = JsonConvert.DeserializeObject<State[]>(statesFile.text);

        // assign ids to states
        int id = 0;
        foreach (State state in MapParent.mapState.States)
        {
            state.ID = id;
            id++;
        }
        
        print($"loaded {MapParent.mapState.States.Count()} states");
    }

    public void CreateColourStatesIDDict()
    {
        MapParent.mapState.StateMapColourToStateID.Clear();
        foreach (State state in MapParent.mapState.States)
        {
            MapParent.mapState.StateMapColourToStateID.Add(state.StatesMapColour, state.ID);
        }

        print("created StateMapColourToStateID dict");
    }    

    public void CreateTileIDDict()
    {
        MapParent.mapState.TileIDToTile.Clear();
        foreach (GameTile tile in MapParent.mapState.Tiles)
        {
            MapParent.mapState.TileIDToTile.Add(tile.ID, tile);
        }
        print("created TileIDToTile dict");
    }

    public void CreateStateIDDict()
    {
        MapParent.mapState.StateIDToState.Clear();
        foreach (State state in MapParent.mapState.States)
        {
            MapParent.mapState.StateIDToState.Add(state.ID, state);
        }
        print("created StateIDToState dict");
    }

    public Texture2D StatesMapTexture { get; private set; }
    public void AssignStateTileIDs()
    {
        // make temp TilesID lists for adding tiles to (tilesID is an array)
        Dictionary<int, List<int>> tempTilesID = new Dictionary<int, List<int>>();
        foreach (State state in MapParent.mapState.States)
        {
            tempTilesID.Add(state.ID, new List<int>());
        }

        for (int y = 0; y < StatesMapTexture.height; y++)
        {
            for (int x = 0; x < StatesMapTexture.width; x++)
            {
                Color pixelColour = StatesMapTexture.GetPixel(x, y);
                if (pixelColour.a == 0) { continue; } // if transparent pixel, skip

                State state = MapParent.mapUtils.StateMapColourToState(pixelColour);
                if (state == null)
                {
                    // Debug.LogError($"unknown colour on states map {pixelColour} at ({x}, {y})");
                    continue;
                }

                // tileState.TilesID.Add(tileState.ID, )
                int tileID = MapParent.mapState.TileIDDictionary[y][x];
                tempTilesID[state.ID].Add(tileID);
            }
        }

        // convert temp TilesID lists back to state info
        foreach (State state in MapParent.mapState.States)
        {
            state.TilesID = tempTilesID[state.ID].ToArray();
        }
    }

    // ---------------------------------------------------- LOAD BUILDING DATA ----------------------------------------------------

    private TextAsset buildingsFile;
    public void LoadBuildingData()
    {
        // load buildings from file
        buildingsFile = Resources.Load("Map/Buildings/Buildings") as TextAsset;
        MapParent.mapState.Buildings = JsonConvert.DeserializeObject<List<BuildingType>>(buildingsFile.text);

        print($"loaded {MapParent.mapState.Buildings.Count} buildings");
    }

    // ---------------------------------------------------- LOAD COUNTRY DATA ----------------------------------------------------

    private TextAsset countriesFile;
    public void LoadCountryData()
    {
        // load buildings from file
        countriesFile = Resources.Load("Countries/Countries") as TextAsset;
        MapParent.mapState.Countries = JsonConvert.DeserializeObject<List<Country>>(countriesFile.text);

        print($"loaded {MapParent.mapState.Countries.Count} countries");
        
        CreateCountryTagToCountryDict();
        CountryColourToCountry();
    }
    
    public void CreateCountryTagToCountryDict()
    {
        MapParent.mapState.CountryTagToCountry.Clear();
        foreach (Country country in MapParent.mapState.Countries)
        {
            MapParent.mapState.CountryTagToCountry.Add(country.Tag, country);
        }

        print("created CountryTagToCountry dict");
    }

    public void CountryColourToCountry()
    {
        MapParent.mapState.CountryColourToCountry.Clear();
        foreach (Country country in MapParent.mapState.Countries)
        {
            MapParent.mapState.CountryColourToCountry.Add(country.Colour, country);
        }

        print("created CountryTagToCountry dict");
    }

    public Texture2D OccupationMapTexture { get; private set; }
    public void LoadOccupationData()
    {
        for (int y = 0; y < OccupationMapTexture.height; y++)
        {
            for (int x = 0; x < OccupationMapTexture.width; x++)
            {
                Color pixelColour = OccupationMapTexture.GetPixel(x, y);
                if (pixelColour.a == 0) { continue; } // if transparent pixel, skip

                // get tile here and set tile occupier to country represented by the pixel colour
                GameTile tile = MapParent.mapUtils.GetTileAtCoords(new Vector2Int(x, y), false);
                Country occupier = MapParent.mapState.CountryColourToCountry[pixelColour];
                MapParent.mapUtils.SetTileOccupier(tile, occupier, false);
            }
        }
    }
}
