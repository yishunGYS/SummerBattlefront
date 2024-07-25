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
        public int defendPoint;
        public int magicDefendPoint;

        public float attackInterval;
        public float attackRange;
        public int attackNum;
        public UnitType attackEnemyType;  //若attackEnemyType是多种，那么待扩展
        public int blockNum;
        
        public string scenePrefabPath;
        public string uiPrefabPath;
    }
}