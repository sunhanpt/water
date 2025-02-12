using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Level
    {
        private readonly GameObject _bottlePrefab;
        private readonly LevelsLayout _levelsLayout;
        
        private LevelInfo _levelInfo;
        private GameObject[] _bottles;
        
        private readonly List<Bottle> _selectedBottles;
        private bool _bAnimationPlaying = false;
        
        
        public Level(LevelInfo levelInfo, LevelsLayout levelsLayout, GameObject bottlePrefab)
        {
            if (levelInfo.levelLayout >= levelsLayout.LevelLayouts.Length)
            {
                Debug.LogError("Level layout not found");
                return;
            }
            if (levelsLayout.LevelLayouts[levelInfo.levelLayout].Positions.Length != levelInfo.bottles.Length)
            {
                Debug.LogError("Level layout not match with bottles");
                return;
            }
            
            this._levelInfo = levelInfo;
            this._levelsLayout = levelsLayout;
            this._bottlePrefab = bottlePrefab;
            _selectedBottles = new List<Bottle>();
            InitLevel();
        }
        public void LoadLevel(LevelInfo levelInfo)
        {
            this._levelInfo = levelInfo;
            InitLevel();
        }
        
        // 根据关卡信息初始化关卡
        private void InitLevel()
        {
            var levelLayout = _levelsLayout.LevelLayouts[_levelInfo.levelLayout];
            _bottles = new GameObject[this._levelInfo.bottles.Length];
            for (int i = 0; i < this._levelInfo.bottles.Length; i++)
            {
                _bottles[i] = Object.Instantiate(_bottlePrefab);
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

        private void OnBottleSelect(Bottle bottle)
        {
            // Handle bottle select
            if(_bAnimationPlaying)
                return;

            if (_selectedBottles.Count < 2)
            {
                _selectedBottles.Add(bottle);
            }

            if (_selectedBottles.Count != 2) return;
            _bAnimationPlaying = true;
            _selectedBottles[0].WaterOut(_selectedBottles[1], null);
        }

        private void OnBottleDeselect(Bottle bottle)
        {
            foreach (var selectedBottle in _selectedBottles)
            {
                if (selectedBottle == bottle)
                {
                    _selectedBottles.Remove(bottle);
                    break;
                }
            }
        }
        
        public void OnAnimationEnd()
        {
            _bAnimationPlaying = true;
            _selectedBottles.Clear();
        }
    }
}