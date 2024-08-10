using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Country
{
    public string Tag { get; set; } // unique string identifier of this country
    public string Name { get; set; } // name displayed in game
    public string LongName { get; set; } // long form of the country's name displayed in game
    [JsonConverter(typeof(ColorHandler))] public Color Colour { get; set; } // the colour of the country in-game and in related map files

    public List<int> OccupyingTilesID { get; set; } // list of tiles this country is occupying
    // public int[] CoreStatesID { get; set; } // the core states of the country by id
    
    [JsonConstructor]
    public Country(string tag, string name, string longName, Color colour)
    {
        Tag = tag;
        Name = name;
        LongName = longName;
        Colour = colour;

        OccupyingTilesID = new List<int>();
    }

    public override string ToString()
    {
        return $"Tag: {Tag}\n" +
               $"Name: {Name}\n" +
               $"LongName: {LongName}\n" +
               $"Colour: {Colour}\n" +
               $"OccupyingTilesID.Count: {OccupyingTilesID.Count}";
    }
}
