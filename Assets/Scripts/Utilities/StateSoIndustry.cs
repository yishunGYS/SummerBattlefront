using ScriptableObjects.SoliderStateTypeSO;
using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using ScriptableObjects.EnemyStateTypeSO;

public class StateSoIndustry
{
    public static UnitStateSO CreateSolideStateSo(UnitStateType stateType)
    {
        if (stateType == UnitStateType.Idle)
        {
            return ScriptableObject.CreateInstance<SoliderState_IdleSO>();
        }
        else if (stateType == UnitStateType.Move)
        {
            return ScriptableObject.CreateInstance<SoliderState_MoveSO>();
        }
        else if (stateType == UnitStateType.Attack)
        {
            return ScriptableObject.CreateInstance<SoliderState_AttackSO>();
        }

        Debug.LogError($"Unsupported UnitStateType: {stateType}");
        return null;
    }


    public static UnitStateSO CreateEnemyStateSo(UnitStateType stateType)
    {
        if (stateType == UnitStateType.Idle)
        {
            return ScriptableObject.CreateInstance<EnemytState_IdleSO>();
        }
        else if (stateType == UnitStateType.Attack)
        {
            return ScriptableObject.CreateInstance<EnemyState_AttackSO>();
        }

        Debug.LogError($"Unsupported UnitStateType: {stateType}");
        return null;
    }
}
