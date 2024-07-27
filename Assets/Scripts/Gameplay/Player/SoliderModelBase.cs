using UnityEngine;

namespace Gameplay.Player
{
    public enum UnitType
    {
        Ground,
        Air,
        Tower,
    }

    public enum AttackTargetType
    {
        OtherSide,
        SelfSide,
    }

    public class SoliderModelBase
    {
        public int soliderId;
        public string soliderName;
        public string soliderDes;
        public UnitType soliderType;
        public int cost;
        public float maxHp;
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
        public string scenePrefabPath;
        public string uiPrefabPath;
    }
}
