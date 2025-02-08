using UnityEngine;

namespace Game
{
    public class Level
    {
        public GameObject BottlePrefab;
        public LevelsLayout LevelsLayout;
        
        private LevelInfo _levelInfo;
        private GameObject[] _bottles;
        
        
        public Level(LevelInfo levelInfo)
        {
            this._levelInfo = levelInfo;
        }
        
        public void LoadLevel(LevelInfo levelInfo)
        {
            this._levelInfo = levelInfo;
        }
        
        // 根据关卡信息初始化关卡
        public void InitLevel()
        {
            var levelLayout = LevelsLayout.LevelLayouts[_levelInfo.bottles.Length];
            _bottles = new GameObject[this._levelInfo.bottles.Length];
            for (int i = 0; i < this._levelInfo.bottles.Length; i++)
            {
                _bottles[i] = Object.Instantiate(BottlePrefab);
                _bottles[i].GetComponent<Bottle>().InitBottle(this._levelInfo.bottles[i]);
                var bottleSelect = _bottles[i].GetComponent<BottleSelectBehaviour>();
                if (bottleSelect != null)
                {
                    bottleSelect.OnBottleSelect = OnBottleSelect;
                    bottleSelect.OnBottleDeselect = OnBottleDeselect;
                }
                var transform = _bottles[i].transform;
                transform.position = levelLayout.Positions[i];
            }
        }
        
        public void StartLevel()
        {
            // Start the level
        }
        
        public void EndLevel()
        {
            // End the level
        }
        
        public void OnBottleSelect(Bottle bottle)
        {
            // Handle bottle select
        }
        
        public void OnBottleDeselect(Bottle bottle)
        {
            // Handle bottle deselect
        }
    }
}