using Newtonsoft.Json;
using UnityEngine;

public class BuildingType // todo: maybe a class for coastal, land buildings, sea buildings? idfk
{
    public int ID { get; set; } // from 0 to ...
    public int x { get; set; } // x position on tilemap
    public int y { get; set; } // y position on tilemap
    public Vector2Int Position() { return new Vector2Int(x, y); }

    [JsonConstructor]
    public BuildingType(int id, int X, int Y)
    {
        ID = id;
        x = X;
        y = Y;
    }

    public override string ToString()
    {
        return $"ID: {ID}\n" +
               $"Position(): ({x}, {y})";
    }

    // todo: public void DoTick() that gets called every tick, we can then override the tick function for each building
    // we can then have the game tick system go through every building and call this function
}
