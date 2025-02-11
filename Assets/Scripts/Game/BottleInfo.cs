using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public enum WaterColor
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange,
        None
    }
    [CreateAssetMenu(fileName = "BottleInfo", menuName = "Water/BottleInfo")]
    public class BottleInfo : ScriptableObject
    {
        public WaterColor[] waters = new WaterColor[4];
    }
}