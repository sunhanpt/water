using System;
using DG.Tweening;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Game
{
    public class Bottle : MonoBehaviour
    {
        public GameObject bottleBody;
        public GameObject[] waters;
        public GameObject waterSurface;
        public GameObject waterJet;
        public GameObject waterJetEnd;
        public Animator bottleAnimator;
        public Transform bottleTransform;
        public Transform rootTransform;
        public GameObject fillWater;
        public GameObject fillWaterTop;
        
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
            CorrectSurfacePosition(_validWaterCount);
        }
        
        public WaterColor GetTopWaterColor()
        {
            if (_validWaterCount == 0)
                return WaterColor.None;
            return _bottleInfo.waters[_validWaterCount - 1];
        }

        public void WaterOut(Bottle otherBottle, int waterCount, Action<Bottle> onComplete)
        {
            MoveToOtherAnim(otherBottle, otherBottle.bottleTransform.position + DataConf.TargetTopOffset);
            otherBottle.WaterIn(waterCount, null);
        }

        public void WaterIn(int waterCount, Action<Bottle> onComplete)
        {
            var waterSpine = waterSurface.GetComponent<SkeletonAnimation>();
            waterSpine.AnimationState.SetAnimation(0, "daoshui_cl", false);
        }

        private void Update()
        {
            // 瓶子旋转，对水体横向放大。
            var axis = Vector3.zero;
            bottleTransform.rotation.ToAngleAxis(out float angle, out axis);
            
            float xScale = 1.0f / Mathf.Max(0.001f,Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad)));
            
            // 水面随着瓶子旋转缩放x方向
            foreach (var water in waters)
            {
                Transform transform1;
                (transform1 = water.transform.transform).localRotation = Quaternion.Inverse(bottleTransform.rotation);
                transform1.localScale = new Vector3(xScale * 1.25f, 1, 1);
            }
            
            waterSurface.transform.localRotation = Quaternion.Inverse(bottleTransform.rotation);
            waterSurface.transform.localScale = new Vector3(xScale * 1.15f, 1, 1);
        }

        private void MoveToOtherAnim(Bottle otherBottle, Vector3 movePosition)
        {
            // 播放瓶子移动动画
            bottleAnimator.Play("BottleOut");
            rootTransform.DOMove(movePosition, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
            {
                // 添加完成回调
                fillWater.SetActive(true);
                fillWaterTop.SetActive(true);
                
                fillWaterTop.transform.position = bottleTransform.position + bottleTransform.up * 2;
            });
        }

        private void PlayWaterAnimatinOut()
        {
            
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
            waterSurface.SetActive(true);
            waterSurface.GetComponent<Animator>().SetTrigger("Splash");
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
        
        private void CorrectSurfacePosition(int validWaterCount)
        {
            Vector3 splashPos = waters[validWaterCount - 1].transform.position + new Vector3(0, 0.4f, 0);
            waterSurface.transform.position = splashPos;
        }
    }
}