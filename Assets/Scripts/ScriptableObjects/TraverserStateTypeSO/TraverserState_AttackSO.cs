using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;
using ScriptableObjects.SoliderStateTypeSO;
using UnityEngine;

[CreateAssetMenu(fileName = "TraverserState_AttackSO", menuName = "ScriptableObjects/TraverserState/TraverserState_AttackSO")]
public class TraverserState_AttackSO : SoliderState_MeleeAttackSO
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
