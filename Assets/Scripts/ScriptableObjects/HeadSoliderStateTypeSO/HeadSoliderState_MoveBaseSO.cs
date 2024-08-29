using Gameplay.Player;
using Managers;
using ScriptableObjects.SoliderStateTypeSO;
using UnityEditor;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "HeadSoliderState_MoveBaseSO", menuName = "ScriptableObjects/HeadSoliderStateTypeSO/HeadSoliderState_MoveBaseSO")]
public class HeadSoliderState_MoveBaseSO : SoliderStateSO
{
    public HeadSoliderState_MoveBaseSO()
    {
        stateType = UnitStateType.Move;
    }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        if (soliderAgent.soliderLogic.CheckObstacle())
        {
            fsm.ChangeState(UnitStateType.Idle);
        }

        soliderAgent.soliderLogic.Move();
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}