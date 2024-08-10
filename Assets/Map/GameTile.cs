using Newtonsoft.Json;
using UnityEngine;

public class GameTile
{
    public int ID { get; set; } // from 0 to ...
    public int x { get; set; } // x position on tilemap
    public int y { get; set; } // y position on tilemap
    public Vector2Int Position() { return new Vector2Int(x, y); }

    public string OccupiedByCountryTag { get; set; } // the country which occupies this tile by id

    [JsonConstructor]
    public GameTile(int id, int X, int Y)
    {
        ID = id;
        x = X;
        y = Y;
    }

    public override string ToString()
    {
        return $"ID: {ID}\n" +
               $"Position(): ({x}, {y})\n" +
               $"OccupiedByCountryID: {OccupiedByCountryTag}";
    }
}
