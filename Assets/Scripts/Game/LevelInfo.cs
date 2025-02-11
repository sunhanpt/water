using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "LevelInfo", menuName = "Water/LevelInfo")]
    public class LevelInfo : ScriptableObject
    {
        public int levelLayout;
        public BottleInfo[] bottles;
    }
}