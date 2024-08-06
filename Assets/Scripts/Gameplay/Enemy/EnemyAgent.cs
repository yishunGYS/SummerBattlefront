using System;
using Gameplay.Player;
using Managers;
using UnityEngine;
using Utilities;

namespace Gameplay.Enemy
{
    public class EnemyAgent : UnitAgent
    {
        public EnemyModelBase enemyModel;
        public EnemyLogicBase enemyLogic;
        public int enemyId;

        private StateMachine fsm;
        
        //关卡开始一开始/第x阶段更新敌方防线的时候调用
        public virtual void OnInit()
        {
            InitData();
            fsm = GetComponent<StateMachine>();
            fsm.OnInit();
            //enemyLogic = new EnemyLogicBase(this);
        }
        
        
        private void InitData()
        {
            if (DataManager.Instance.GetEnemyBaseModels().TryGetValue(enemyId, out EnemyModelBase model))
            {
                enemyModel = model.DeepCopy();
                print(enemyModel.enemyName);
                print("获取到该数据");
            }
            else
            {
                print("没有获取到该数据");
            }
        }


        private void Update()
        {
            fsm.OnUpdate();
        }


        public override UnitAttackData GetAttackPoint()
        {
            return new UnitAttackData(enemyModel.attackPoint, enemyModel.magicAttackPoint);
        }

        public override UnitDefendData GetDefendPoint()
        {
            return new UnitDefendData(enemyModel.defendReducePercent, enemyModel.magicDefendReducePercent);
        }
        
    }
}
