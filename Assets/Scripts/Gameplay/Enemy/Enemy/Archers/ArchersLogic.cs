using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemy;
using UnityEngine;

public class ArchersLogic : EnemyLogicBase
{
    public ArchersLogic(EnemyAgent agent) : base(agent)
    {


    }

    public override void GetTarget()
    {
        base.GetTarget();
        DistanceBasedEnemyGetTarget();
    }

    public override void Attack()
    {
        base.Attack();
        FocusAttack();
    }
}
