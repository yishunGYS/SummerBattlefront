using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SoliderStateMachine : StateMachine
{
    public override void CopeStateSo()
    {
        base.CopeStateSo();
        foreach (var item in stateDicts.Values)
        {
            if (item == null)
            {
                Debug.LogError("StateSO in stateDicts is null.");
                continue;
            }
            var tempSo = StateSoIndustry.Clone(item);
            if (tempSo == null)
            {
                Debug.LogError($"Failed to create StateSO for {item.stateType}.");
                continue;
            }
            runtimeStateDict.Add(item.stateType, tempSo);
        }
    }
}
