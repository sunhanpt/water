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
        public GameObject bottleMouth;
        public GameObject bottleMask;
        
        private int _validWaterCount = 0;
        private BottleInfo _bottleInfo;
        private bool _bWaterOut = false;
        private bool _bWaterIn = false;
        float _waterOutSlider = 0.0f;
        private Bottle _waterInBottle;

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
                waters[i].GetComponent<WaterItem>().Color = GetWaterColor(bottleInfo.waters[i]);
                _validWaterCount = i;
            }
            _validWaterCount++;
            
            // 修正水面位置
            CorrectSurfacePosition(_validWaterCount);
            
            var animationEvent = bottleAnimator.gameObject.GetComponent<WaterAnimationEvent>();
            if (animationEvent != null)
            {
                animationEvent.OnBeginWaterOutAction = OnBeginWaterOutAnimEvent;
                animationEvent.OnEndWaterOutAction = OnEndWaterOutAnimEvent;
                animationEvent.OnBeginWaterInAction = OnBeingWaterInAnimEvent;
                animationEvent.OnEndWaterInAction = OnEndWaterInAnimEvent;
            }
            // TODO：跟进顶部水得颜色播放不同动画。
            PlayWaterSurfaceAnim("ruchanghuangdong_hs");
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
            _waterInBottle = otherBottle;
            
        }

        public void WaterIn(int waterCount, Action<Bottle> onComplete)
        {
            // TODO: 跟进当前倒水得颜色修改
            PlayWaterSurfaceAnim("daoshui_hs");
        }

        private void PlayWaterSurfaceAnim(string anim)
        {
            var waterSpine = waterSurface.GetComponent<SkeletonAnimation>();
            waterSpine.AnimationState.SetAnimation(0, anim, false);
        }

        private void OnBeginWaterOutAnimEvent()
        {
            _bWaterOut = true;
            _waterOutSlider = 0.0f;
            _waterInBottle?.WaterIn(0, null);
        }

        private void OnEndWaterOutAnimEvent()
        {
            _bWaterOut = false;
        }

        private void OnBeingWaterInAnimEvent()
        {
            _bWaterIn = true;
        }

        private void OnEndWaterInAnimEvent()
        {
            _bWaterIn = false;
        }

        private void Update()
        {
            CorrectWaterScale();
            if (_bWaterOut)
            {
                fillWaterTop.SetActive(true);
                fillWaterTop.transform.position = bottleMouth.transform.position;
            }
            else
            {
                fillWaterTop.SetActive(false);
            }
        }

        private void MoveToOtherAnim(Bottle otherBottle, Vector3 movePosition)
        {
            rootTransform.DOMove(movePosition, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
            {
                // 播放瓶子移动动画
                bottleAnimator.Play("BottleOut");
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
            var waterTransform = waters[validWaterCount - 1].transform;
            Vector3 splashPos = waterTransform.position + new Vector3(0, DataConf.WaterHeight / 2, 0) * waterTransform.localScale.y;
            waterSurface.transform.position = splashPos;
        }

        private void CorrectWaterScale()
        {
            // 瓶子旋转，对水体横向放大。
            var axis = Vector3.zero;
            bottleTransform.rotation.ToAngleAxis(out float angle, out axis);
            
            float xScale = 1.0f / Mathf.Max(0.001f,Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad)));
            
            // // 水面随着瓶子旋转缩放x方向
            // foreach (var water in waters)
            // {
            //     Transform waterTransform = water.transform.transform;
            //     var position = waterTransform.position;
            //     waterTransform.localRotation = Quaternion.Inverse(bottleTransform.rotation);
            //     // var localPosition = waterTransform.localPosition;
            //     // localPosition = new Vector3( DataConf.WaterHeight / 2 * Mathf.Sin(angle * Mathf.Deg2Rad), localPosition.y, localPosition.z);
            //     // waterTransform.localPosition = localPosition;
            //
            //     waterTransform.localScale = new Vector3(xScale * 1f, 1, 1);
            // }
            
            waterSurface.transform.localRotation = Quaternion.Inverse(bottleTransform.rotation);
            waterSurface.transform.localScale = new Vector3(xScale * 1.15f, 1, 1);
        }
    }
}