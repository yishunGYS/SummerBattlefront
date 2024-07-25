using UnityEngine;

namespace Gameplay.Player
{
    public enum UnitType
    {
        Ground,
        Air,
        Tower,
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
        public int defendPoint;
        public int magicDefendPoint;

        public float attackInterval;
        public float attackRange;
        public int attackNum;
        public UnitType attackEnemyType;  //若attackEnemyType是多种，那么待扩展

        public float moveSpeed;
        public string scenePrefabPath;
        public string uiPrefabPath;
    }
}
