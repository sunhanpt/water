using System;
using Lean.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BottleSelectBehaviour : LeanSelectableBehaviour
    {
        public Action<Bottle> OnBottleSelect;
        public Action<Bottle> OnBottleDeselect;

        public void Update()
        {
            int a = 0;
        }

        protected override void OnSelected(LeanSelect select)
        {
            if (gameObject.TryGetComponent(out Bottle bottle))
            {
                OnBottleSelect?.Invoke(bottle);
            }
            
            //Debug.LogError("OnSelected");
        }
        
        protected override void OnDeselected(LeanSelect select)
        {
            if (gameObject.TryGetComponent(out Bottle bottle))
            {
                OnBottleDeselect?.Invoke(bottle);
            }
            
            //Debug.LogError("OnDeselected");
        }
    }
}