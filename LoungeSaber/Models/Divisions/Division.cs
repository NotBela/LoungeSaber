using System;
using LoungeSaber.Models.Maps;
using Newtonsoft.Json;
using UnityEngine;

namespace LoungeSaber.Models.Divisions
{
    public class Division
    {
        public int MinMMR { get; private set; }
        public int MaxMMR { get; private set; }

        public string DivisionName { get; private set; }
        public Color DivisionColor { get; private set; }
    
        public MapDifficulty.MapCategory[] DisallowedMapTypes { get; private set; }

        // TODO: make constructor private
        public Division(int minMMR, int maxMMR, string divisionName, Color divisionColor, MapDifficulty.MapCategory[] disallowedMapTypes)
        {
            MinMMR = minMMR;
            MaxMMR = maxMMR;
            DivisionName = divisionName;
            DivisionColor = divisionColor;
            DisallowedMapTypes = disallowedMapTypes;
        }

        public static Division Parse(string json)
        {
            var division = JsonConvert.DeserializeObject<Division>(json);
        
            if (division == null) 
                throw new Exception("Could not deserialize division from config!");
        
            return division;
        }
    }

    public struct Color
    {
        public Color(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        
        public UnityEngine.Color ToUnity() => new UnityEngine.Color(r, g, b);

        public string ToHexidecimal() => $"#{FloatNormalizedToHex(r)}{FloatNormalizedToHex(g)}{FloatNormalizedToHex(b)}";

        private string FloatNormalizedToHex(float value) => DecToHex(Mathf.RoundToInt(value));
        private string DecToHex(int value) => value.ToString("X2");
    }
}