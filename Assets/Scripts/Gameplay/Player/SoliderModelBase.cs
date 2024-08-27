using System;
using UnityEngine;

namespace Gameplay.Player
{
    [Flags]
    public enum UnitType
    {
        None = 0,
        Ground = 1 << 0, //1
        Tower = 1 << 1, //2
        Block = 1<<2   //4
    }

    public enum AttackTargetType
    {
        OtherSide,
        SelfSide,
        None,
    }

    public class SoliderModelBase
    {
        public int soliderId;
        public string soliderName;
        public string soliderDes;
        public UnitType soliderType;
        public int cost;
        public int maxHp;
        public int spawnNum;
        public int attackPoint;
        public int magicAttackPoint;
        public float defendReducePercent;
        public float magicDefendReducePercent;

        public float attackInterval;
        public float attackRange;
        public int attackNum;
        public float attackAoeRange;
        public AttackTargetType attackTargetType;       //攻击目标是敌方还是友方（辅助角色的攻击目标是己方）
        
        public UnitType attackEnemyType;  //若attackEnemyType是多种，那么待扩展

        public float moveSpeed;
        public float relocateCd;
        
        public string scenePrefabPath;
        public string uiPrefabPath;

        public SoliderModelBase DeepCopy()
        {
            return new SoliderModelBase
            {
                soliderId = this.soliderId,
                soliderName = this.soliderName,
                soliderDes = this.soliderDes,
                soliderType = this.soliderType,
                cost = this.cost,
                maxHp = this.maxHp,
                spawnNum = this.spawnNum,
                attackPoint = this.attackPoint,
                magicAttackPoint = this.magicAttackPoint,
                defendReducePercent = this.defendReducePercent,
                magicDefendReducePercent = this.magicDefendReducePercent,
                attackInterval = this.attackInterval,
                attackRange = this.attackRange,
                attackNum = this.attackNum,
                attackAoeRange = this.attackAoeRange,
                attackTargetType = this.attackTargetType,
                attackEnemyType = this.attackEnemyType,
                moveSpeed = this.moveSpeed,
                relocateCd = this.relocateCd,
                scenePrefabPath = this.scenePrefabPath,
                uiPrefabPath = this.uiPrefabPath
            };
        }
    }
    
}
