using Newtonsoft.Json;
using UnityEngine;

public class State
{
    public int ID { get; set; } // from 0 to ...
    public string Name { get; set; } // display name of the state
    [JsonConverter(typeof(ColorHandler))] public Color StatesMapColour { get; set; } // colour on the states map associated with this state
    public int[] TilesID { get; set; } // list of all tiles that are part of the state by id
    
    [JsonConstructor]
    public State(int id, string name, Color statesMapColour, int[] tilesID)
    {
        ID = id;
        Name = name;
        StatesMapColour = statesMapColour;
        TilesID = tilesID;
    }

    public State(int id, string name, Color statesMapColour)
    {
        ID = id;
        Name = name;
        StatesMapColour = statesMapColour;
        TilesID = new int[0];
    }

    public override string ToString()
    {
        return $"ID: {ID}\n" +
               $"Name: {Name}\n" +
               $"StatesMapColour: {StatesMapColour}\n" +
               $"TilesID.Length: {TilesID.Length}";
    }
}
