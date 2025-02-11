using UnityEngine;

namespace Game
{
    // 存储各种关卡的布局信息（根据瓶子的个数对应不同布局）
    [CreateAssetMenu(fileName = "LevelsLayout", menuName = "Water/LevelsLayoutCollector")]
    public class LevelsLayout : ScriptableObject
    {
        public LevelLayout[] LevelLayouts;
    }
    
    [CreateAssetMenu(fileName = "LevelLayout", menuName = "Water/LevelLayout")]
    public class LevelLayout : ScriptableObject
    {
        public Vector3[] Positions;
    }
}