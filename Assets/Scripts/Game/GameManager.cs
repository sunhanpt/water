using System;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public LevelInfo[] LevelInfos;
        public int CurLevel = 0;

        public LevelsLayout LevelsLayout;
        public GameObject BottlePrefab;
        
        private Level _level;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (CurLevel < LevelInfos.Length && LevelInfos[CurLevel] != null)
            {
                _level = new Level(LevelInfos[CurLevel], LevelsLayout, BottlePrefab);
            }
        }
    }
}