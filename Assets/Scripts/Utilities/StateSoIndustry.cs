using ScriptableObjects.SoliderStateTypeSO;
using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using ScriptableObjects.EnemyStateTypeSO;

public class StateSoIndustry
{
    
    public static T Clone<T>(T original) where T : ScriptableObject
    {
        T clone = ScriptableObject.Instantiate(original);
        return clone;
    }
    
}
