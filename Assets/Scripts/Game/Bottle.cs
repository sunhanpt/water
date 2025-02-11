using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class Bottle : MonoBehaviour
    {
        public GameObject bottleBody;
        public GameObject[] waters;
        public GameObject waterSplash;
        public GameObject waterJet;
        public GameObject waterJetEnd;
        public Animator bottleAnimator;
        public Transform bottleTransform;
        public Transform rootTransform;
        
        private int _validWaterCount = 0;
        private BottleInfo _bottleInfo;

        public void InitBottle(BottleInfo bottleInfo)
        {
            _bottleInfo = bottleInfo;
            for (int i = 0; i < waters.Length; i++)
            {
                waters[i].SetActive(false);
            }
            
            // 根据配置数据初始化瓶子
            for (int i = 0; i < bottleInfo.waters.Length; i++)
            {
                if (bottleInfo.waters[i] == WaterColor.None)
                    continue;
                waters[i].SetActive(true);
                waters[i].GetComponent<SpriteRenderer>().color = GetWaterColor(bottleInfo.waters[i]);
                _validWaterCount = i;
            }
            _validWaterCount++;
            
            // 修正水面位置
            CorrectSplashPosition(_validWaterCount);
        }
        
        public WaterColor GetTopWaterColor()
        {
            if (_validWaterCount == 0)
                return WaterColor.None;
            return _bottleInfo.waters[_validWaterCount - 1];
        }

        public void WaterOut(Bottle otherBottle, Action<Bottle> onComplete)
        {
            MoveToOtherAnim(otherBottle, otherBottle.bottleTransform.position);
        }
        
        private void MoveToOtherAnim(Bottle otherBottle, Vector3 movePosition)
        {
            // 播放瓶子移动动画
            bottleAnimator.Play("BottleOut");
            rootTransform.DOMove(movePosition, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
            {
                // 添加完成回调
            });
        }

        public void PlayPourWaterAnim()
        {
            var waterColor = GetTopWaterColor();
            if (waterColor == WaterColor.None)
                return;
            string spineAnimName = "ruchanghuangdong_cl";
            switch (waterColor)
            {
                case WaterColor.Red:
                    
                    break;
            } 
        }
        
        public void PlaySplashAnim()
        {
            waterSplash.SetActive(true);
            waterSplash.GetComponent<Animator>().SetTrigger("Splash");
        }

        private Color GetWaterColor(WaterColor waterColor)
        {
            switch (waterColor)
            {
                case WaterColor.Red:
                    return Color.red;
                case WaterColor.Green:
                    return Color.green;
                case WaterColor.Blue:
                    return Color.blue;
                case WaterColor.Yellow:
                    return Color.yellow;
                case WaterColor.Purple:
                    return new Color(0.5f, 0, 0.5f);
                case WaterColor.Orange:
                    return new Color(1, 0.5f, 0);
                default:
                    return Color.white;
            }
        }
        
        private void CorrectSplashPosition(int validWaterCount)
        {
            Vector3 splashPos = waters[validWaterCount - 1].transform.position + new Vector3(0, 0.5f, 0);
            waterSplash.transform.position = splashPos;
        }
    }
}