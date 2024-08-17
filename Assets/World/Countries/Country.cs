using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Country
{
    public string Tag { get; set; } // unique string identifier of this country
    public string Name { get; set; } // name displayed in game
    public string LongName { get; set; } // long form of the country's name displayed in game
    [JsonConverter(typeof(ColorHandler))] public Color Colour { get; set; } // the colour of the country in-game and in related map files

    public float Stability { get; set; } // stability of the country, todo: split this into 4 or 5 or 6 or whatever the plan is
    public string StabilityPercent()
    {
        return $"{Math.Round(Stability * 100.0f, 1)}%";
    }

    public List<int> OccupyingTilesID { get; set; } // list of tiles this country is occupying
    // public int[] CoreStatesID { get; set; } // the core states of the country by id
    
    [JsonConstructor]
    public Country(string tag, string name, string longName, Color colour, float stability)
    {
        Tag = tag;
        Name = name;
        LongName = longName;
        Colour = colour;

        Stability = stability;

        OccupyingTilesID = new List<int>();
    }

    public override string ToString()
    {
        return $"Tag: {Tag}\n" +
               $"Name: {Name}\n" +
               $"LongName: {LongName}\n" +
               $"Colour: {Colour}\n" +
               $"OccupyingTilesID.Count: {OccupyingTilesID.Count}\n" +
               $"Stability: {Stability*100}%";
    }
}
