using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    void Awake()
    {
        WorldTilemap = GameObject.Find("WorldTilemap").GetComponent<Tilemap>();
        
        BuildingsParent = GameObject.Find("BuildingsParent").transform;
    }
    
    public Tile BaseTile  { get; private set; } // tilemap Tile, not GameTile
    public Tilemap WorldTilemap { get; private set; }
    public Texture2D BaseTexture { get; private set; }
    public void CreateTiles()
    {
        // load tile prefab
        BaseTile = Resources.Load("Map/BaseTile") as Tile;
        // load map texture
        BaseTexture = Resources.Load("Map/BaseMap") as Texture2D;

        MapParent.mapState.TileIDDictionary.Clear();
        int currentTileID = 0;
        
        List<GameTile> tempTiles = new List<GameTile>();
        for (int y = 0; y < BaseTexture.height; y++)
        {
            Dictionary<int, int> dictForThisRow = new Dictionary<int, int>();
            for (int x = 0; x < BaseTexture.width; x++)
            {
                Color pixelColour = BaseTexture.GetPixel(x, y);
                if (pixelColour.a == 0) { continue; } // if transparent pixel, skip
                
                // create new tile
                GameTile newTile = new GameTile(currentTileID, x, y);
                tempTiles.Add(newTile);

                // append to this row's tile dictionary
                dictForThisRow.Add(x, currentTileID);

                // set tile on tilemap
                WorldTilemap.SetTile(new Vector3Int(x, y, 0), BaseTile);

                currentTileID++;
            }
            // append row dictionary to mega dictionary
            MapParent.mapState.TileIDDictionary.Add(y, dictForThisRow);
        }
        MapParent.mapState.Tiles = tempTiles.ToArray();
    }

    private Transform BuildingsParent;
    private GameObject BuildingPrefab;
    public void CreateBuildings()
    {
        BuildingPrefab = Resources.Load("Map/Buildings/BuildingPrefab") as GameObject;

        foreach (BuildingType building in MapParent.mapState.Buildings)
        {
            Vector2 worldPos = MapUtils.CoordToWorld(building.Position());
            GameObject newBuilding = Instantiate(original: BuildingPrefab,
                                                 position: new Vector3(worldPos.x, worldPos.y, -1.0f),
                                                 rotation: Quaternion.identity,
                                                 parent: BuildingsParent);
        }
    }
}