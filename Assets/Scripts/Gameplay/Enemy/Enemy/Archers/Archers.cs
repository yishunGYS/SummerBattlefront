using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemy;
using UnityEngine;

public class Archers : EnemyAgent
{
    public override void OnInit()
    {
        base.OnInit();
        enemyLogic = new ArchersLogic(this);
    }
}
