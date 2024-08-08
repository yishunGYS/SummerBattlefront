using Gameplay.Player;

namespace Gameplay.Enemy
{
    public class EnemyModelBase
    {
        public int enemyId;
        public string enemyName;
        public string enemyDes;
        public UnitType enemyType;
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
        
        public UnitType attackSoliderType;  //若attackEnemyType是多种，那么待扩展
        public int blockNum;

        public int deadCoin;
        public string scenePrefabPath;
        public string uiPrefabPath;
        public EnemyModelBase DeepCopy()
        {
            return new EnemyModelBase
            {
                enemyId = this.enemyId,
                enemyName = this.enemyName,
                enemyDes = this.enemyDes,
                enemyType = this.enemyType,
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
                attackSoliderType = this.attackSoliderType,
                blockNum = this.blockNum,
                deadCoin = this.deadCoin,
                scenePrefabPath = this.scenePrefabPath,
                uiPrefabPath = this.uiPrefabPath
            };
        }
    }
}