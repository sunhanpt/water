using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class WaterAnimationEvent : MonoBehaviour
{
    public Action OnBeginWaterOutAction;
    public Action OnEndWaterOutAction;
    public Action OnBeginWaterInAction;
    public Action OnEndWaterInAction;
    public void OnBeginWaterOut()
    {
        if (OnBeginWaterOutAction != null)
        {
            OnBeginWaterOutAction.Invoke();
        }
    }

    public void OnEndWaterOut()
    {
        if (OnEndWaterOutAction != null)
        {
            OnEndWaterOutAction.Invoke();
        }
    }

    public void OnBeginWaterIn()
    {
        
    }
    
    public void OnEndWaterIn()
    {
        
    }
}
