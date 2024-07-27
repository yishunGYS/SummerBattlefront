using Gameplay.Player;

namespace Gameplay.Enemy
{
    public class EnemyModelBase
    {
        public int enemyId;
        public string enemyName;
        public string enemyDes;
        public UnitType enemyType;
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
        
        public UnitType attackSoliderType;  //若attackEnemyType是多种，那么待扩展
        public int blockNum;
        
        public string scenePrefabPath;
        public string uiPrefabPath;
    }
}