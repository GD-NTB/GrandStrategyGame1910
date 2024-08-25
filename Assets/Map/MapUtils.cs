using UnityEngine;

public class MapUtils : MonoBehaviour
{
    public static Vector2 CoordToWorld(Vector2 coord)
    {
        return new Vector2((coord.x - 2500.0f)*0.01f+0.005f,
                           (coord.y - 1250.0f)*0.01f+0.005f);
    }

    public static Vector2Int WorldToCoord(Vector2 world)
    {
        Vector2 pos = new Vector2(world.x + 25.0f,
                                  world.y + 12.5f) * 100.0f;
        return new Vector2Int(Mathf.FloorToInt(pos.x),
                              Mathf.FloorToInt(pos.y));
        }

    // --------------------------------------------------------------------------------------------------------------------------

    public Vector2Int GetCoordsAtMouse()
    {
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return WorldToCoord(worldMousePos);
    }

    public bool AreCoordsOnLand(Vector2Int position)
    {
        return MapParent.mapCreator.BaseTexture.GetPixel(position.x, position.y).a != 0.0f; // transparent = water
    }

    public GameTile GetTileAtCoords(Vector2Int coords, bool skipLandCheck=false)
    {
        if (coords.x < 0 || coords.x > 4999 || coords.y < 0 || coords.y > 2499) { return null; }
        if (!skipLandCheck && !AreCoordsOnLand(coords)) { return null; }
        int tileID = MapParent.mapState.TileIDDictionary[coords.y][coords.x];
        return MapParent.mapState.TileIDToTile[tileID];
    }

    public GameTile GetTileAtMouse()
    {
        return GetTileAtCoords(GetCoordsAtMouse());
    }
    
    public State TileToState(GameTile tile)
    {
        return GetStateAtCoords(new Vector2Int(tile.x, tile.y));
    }

    public State GetStateAtCoords(Vector2Int coords)
    {
        Color stateColour = MapParent.mapLoader.StatesMapTexture.GetPixel(coords.x, coords.y);
        if (stateColour.a == 0)
        {
            return null;
        }

        return StateMapColourToState(stateColour);
    }


    public State StateMapColourToState(Color stateMapColour)
    {
        if (!MapParent.mapState.StateMapColourToStateID.TryGetValue(stateMapColour, out int stateID))
        {
            return null;
        }
        return MapParent.mapState.StateIDToState[stateID];
    }

    // --------------------------------------------------------------------------------------------------------------------------
    
    public void SetTileColour(GameTile tile, Color colour)
    {
        if (!AreCoordsOnLand(tile.Position())) { return; }
        MapParent.mapCreator.WorldTilemap.SetColor((Vector3Int)tile.Position(), colour);
    }

    public void SetTileColourAtCoords(Vector2Int position, Color colour)
    {
        if (position.x < 0 || position.x > 4999 || position.y < 0 || position.y > 2499) { return; }
        if (!AreCoordsOnLand(position)) { return; }
        MapParent.mapCreator.WorldTilemap.SetColor((Vector3Int)position, colour);
    }

    public void SetStateColour(State state, Color colour)
    {
        foreach (int tileID in state.TilesID)
        {
            SetTileColour(MapParent.mapState.TileIDToTile[tileID], colour);
        }
    }

    public void OutlineState(State state, Color colour, float outlineStrength=1.0f, float fillStrength=0.5f)
    {
        // look through each tile in state on states texture
        // if there are any neighbouring tiles not in the state, colour the tile we're on
        foreach (int tileID in state.TilesID)
        {
            GameTile tile = MapParent.mapState.TileIDToTile[tileID];

            bool isNeighbouringNonTiles = false;
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Color neighbourColour = MapParent.mapLoader.StatesMapTexture.GetPixel(tile.x + x, tile.y + y);
                    // if we neighbour a tile outside of the state, then we are an outline tile, so go to next tile
                    if (neighbourColour != state.StatesMapColour)
                    {
                        isNeighbouringNonTiles = true;
                        goto outOfLoop; // first ever recorded use of goto in unity
                    }
                }
            }
            outOfLoop:

            // if not neighbouring any tiles outside of state, the tile is inside the state, so pick correct fill strength
            float strength = isNeighbouringNonTiles ? outlineStrength : fillStrength;
            SetTileColour(tile, GetTileColourTinted(tile, colour, strength));
        }
    }

    // get the colour of a tile based on what view mode we're in
    // todo: should prob move this somewhere else
    public Color GetTileColour(GameTile tile)
    {
        Color newColour;

        switch (MapParent.mapState.CurrentMapView)
        {
            // ---------------------------------------- MapView.Countries ----------------------------------------
            case MapState.MapView.Countries:
                Country tileCountry = MapParent.mapUtils.GetCountryByTag(tile.OccupiedByCountryTag);
                if (tileCountry != null)
                {
                    newColour = tileCountry.Colour;
                }
                else
                {
                    Debug.LogWarning($"tried getting a countryless tile's colour ({tile.x}, {tile.y})");
                    newColour = Color.black;
                }
            break;
            // ---------------------------------------- MapView.States ----------------------------------------
            case MapState.MapView.States:
                State tileState = TileToState(tile);
                if (tileState != null)
                {
                    newColour = tileState.StatesMapColour;
                }
                else
                {
                    // Debug.LogWarning($"tried getting a stateless tile's colour ({tile.x}, {tile.y})");
                    newColour = Color.black;
                }
            break;
            
            // ---------------------------------------- MapView.Tiles ----------------------------------------
            case MapState.MapView.Tiles:
                newColour = ((tile.x + tile.y) % 2 == 0) ? Color.magenta : Color.yellow; // checkerboard

                // these lag for some reason, maybe because we're creating so many colour objects?
                // newColour = new Color(tile.x / 500.0f, tile.y / 2500.0f, 0.0f);
                // newColour = new Color(Random.value, Random.value, Random.value);
            break;

            // ---------------------------------------- Default ----------------------------------------
            default:
                newColour = Color.black;
            break;
        }

        return newColour;
    }

    public Color GetTileColourTinted(GameTile tile, Color tintColour, float strength)
    {
        return TintColour(GetTileColour(tile), tintColour, strength);
    }

    public Color TintColour(Color originalColour, Color tintColour, float strength)
    {
        if (strength == 1.0f) { return tintColour; }
        if (strength == 0.0f) { return originalColour; }
        Color newColour = originalColour + (tintColour-originalColour)*strength;
        newColour.a = originalColour.a;
        return newColour;
    }

    public void RedrawTile(GameTile tile, bool log=true)
    {
        // using SetTileColour is slower as it checks if the tile is on land
        MapParent.mapCreator.WorldTilemap.SetColor((Vector3Int)tile.Position(), GetTileColour(tile));
        if (log) { print($"redrew tile ({tile.x}, {tile.y})"); }
    }

    public void RedrawState(State state, bool log=true)
    {
        // todo: see if there is a way to set multiple tiles at once
        foreach (int tileID in state.TilesID)
        {
            RedrawTile(MapParent.mapState.TileIDToTile[tileID], false);
        }

        if (log) { print($"redrew state ({state.ID}, {state.Name})"); }
    }

    public void RedrawMap(bool log=true)
    {
        // ------------------------------------------------------------
        float startTime = 0.0f;
        if (log) {  startTime = Time.realtimeSinceStartup; }
        // ------------------------------------------------------------

        foreach (GameTile tile in MapParent.mapState.Tiles)
        {
            RedrawTile(tile, false);
        }

        // ------------------------------------------------------------
        if (log)
        {
            float endTime = Time.realtimeSinceStartup; // for benchmarking
            print($"took {endTime - startTime} seconds to redraw map");
        }
        // ------------------------------------------------------------
    }

    // --------------------------------------------------------------------------------------------------------------------------

    public Country GetCountryByTag(string tag) // doesnt need to exist, but who cares? I CARE
    {
        if(MapParent.mapState.CountryTagToCountry.TryGetValue(tag, out Country country))
        {
            return country;
        }
        return null;
    }
    
    public Country GetCountryAtCoords(Vector2Int coords, bool skipLandCheck=false)
    {
        if (!skipLandCheck && !AreCoordsOnLand(coords)) { return null; }
        string countryID = GetTileAtMouse().OccupiedByCountryTag;

        return GetCountryByTag(countryID);
    }

    public void SetTileOccupier(GameTile tile, Country newOccupier, bool redrawTile=true)
    {
        tile.OccupiedByCountryTag = newOccupier.Tag; // update tile

        // remove tile from old occupier
        Country oldOccupier = GetCountryByTag(tile.OccupiedByCountryTag); // stop being null gay boy
        if (oldOccupier != null)
        {
            oldOccupier.OccupyingTilesID.Remove(tile.ID);
        }
         // add tile to new occupier
        newOccupier.OccupyingTilesID.Add(tile.ID);

        if (redrawTile) { RedrawTile(tile, false); }
    }
}