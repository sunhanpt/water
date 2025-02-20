using UnityEngine;

namespace Game
{
    public static class DataConf
    {
        public static Vector3 SelectMoveOffset = new Vector3(0, 2f, 0f);                // 选中瓶子之后往上移动的位置。
        public static Vector3 TargetTopOffset = new Vector3(-2, 2f, 0f);                // 倒水时瓶子移动的目标点上的便宜。
        public static float WaterHeight = 0.8f;                                               // 单个水的高度。
        public static float BottleWidth = 1.5f;
        public static float BottleBottom = -2.5f;
        public static float BottleTop = 2.0f;
    }
}