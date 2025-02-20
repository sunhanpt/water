using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Game
{
    public class Level
    {
        protected struct BottleRuntimeInfo
        {
            public Vector3 Position;
            public Bottle Bottle;
        }
        
        private readonly GameObject _bottlePrefab;
        private readonly LevelsLayout _levelsLayout;
        
        private LevelInfo _levelInfo;
        private GameObject[] _bottles;
        
        private readonly List<Bottle> _selectedBottles;
        private bool _bAnimationPlaying = false;
        private static readonly int StencilRef = Shader.PropertyToID("_StencilRef");


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
            _bottles = new GameObject[this._levelInfo.bottles.Length];
            for (int i = 0; i < this._levelInfo.bottles.Length; i++)
            {
                _bottles[i] = Object.Instantiate(_bottlePrefab);
                var bottleComponent = _bottles[i].GetComponent<Bottle>();
                bottleComponent.InitBottle(this._levelInfo.bottles[i]);

                var stencil = (i + 1) * 1.0f;
                bottleComponent.bottleMask.GetComponent<SpriteRenderer>().material.SetFloat(StencilRef, stencil);

                // var waterSurface = bottleComponent.waterSurface;
                // waterSurface.GetComponent<MeshRenderer>().material = new Material(waterSurface.GetComponent<MeshRenderer>().material);

                foreach (var bottleWater in bottleComponent.waters)
                {
                    bottleWater.GetComponent<MeshRenderer>().material.SetFloat(StencilRef, stencil);
                }
                
                var bottleSelect = _bottles[i].GetComponent<BottleSelectBehaviour>();
                if (bottleSelect != null)
                {
                    bottleSelect.OnBottleSelect = OnBottleSelect;
                    bottleSelect.OnBottleDeselect = OnBottleDeselect;
                }
                _bottles[i].transform.position = GetBottleLayout(i);
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
                if (_selectedBottles.Count == 1)
                {
                    var bottleTransform = bottle.gameObject.transform;
                    bottleTransform.position += DataConf.SelectMoveOffset;
                }
            }

            if (_selectedBottles.Count != 2) 
                return;
            _bAnimationPlaying = true;
            _selectedBottles[0].WaterOut(_selectedBottles[1], 1,null);
        }

        private void OnBottleDeselect(Bottle bottle)
        {
            foreach (var selectedBottle in _selectedBottles)
            {
                if (selectedBottle == bottle)
                {
                    int bottleId = 0;
                    for (int i = 0; i < _bottles.Length; i++)
                    {
                        if (_bottles[i].GetComponent<Bottle>() == bottle)
                        {
                            bottleId = i;
                            break;
                        }
                    }
                    var levelLayout = _levelsLayout.LevelLayouts[_levelInfo.levelLayout];

                    bottle.gameObject.transform.position = GetBottleLayout(bottleId);
                    _selectedBottles.Remove(bottle);
                    break;
                }
            }
        }

        private Vector3 GetBottleLayout(int bottleId)
        {
            var levelLayout = _levelsLayout.LevelLayouts[_levelInfo.levelLayout];
            return levelLayout.Positions[bottleId];
        }
        
        public void OnAnimationEnd()
        {
            _bAnimationPlaying = true;
            _selectedBottles.Clear();
        }
    }
}