using System;
using Lean.Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class BottleSelectBehaviour : LeanSelectableBehaviour
    {
        public Action<Bottle> OnBottleSelect;
        public Action<Bottle> OnBottleDeselect;
        protected override void OnSelected(LeanSelect select)
        {
            if (select.TryGetComponent(out Bottle bottle))
            {
                OnBottleSelect?.Invoke(bottle);
            }
        }
        
        protected override void OnDeselected(LeanSelect select)
        {
            if (select.TryGetComponent(out Bottle bottle))
            {
                OnBottleDeselect?.Invoke(bottle);
            }
        }
    }
}