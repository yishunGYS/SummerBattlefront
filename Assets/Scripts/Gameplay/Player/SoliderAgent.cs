using System;
using Ilumisoft.Health_System.Scripts.UI;
using Managers;
using UnityEngine;
using Utilities;

namespace Gameplay.Player
{
    public class SoliderAgent : UnitAgent
    {
        public SoliderModelBase soliderModel;
        public int soliderId;

        public SoliderLogicBase soliderLogic;
        private StateMachine fsm;

        public StartPoint birthPonit;

        //instantiate时
        public override void OnInit()
        {
            InitData();
            fsm = GetComponent<StateMachine>();
            fsm.OnInit();
            curHp = soliderModel.maxHp;
            
            base.OnInit();
        }

        private void Update()
        {
            if (!fsm)
            {
                return;
            }
            fsm.OnUpdate();
            birthPonit = soliderLogic.birthPoint;
            //soliderLogic.OnUpdateBuff();
        }
        
        private void InitData()
        {
            if (DataManager.Instance.GetSoliderBaseModels().TryGetValue(soliderId, out SoliderModelBase model))
            {
                soliderModel = model.DeepCopy();
                print(soliderModel.soliderName);
                print("获取到该数据");
            }
            else
            {
                print("没有获取到该数据");
            }
        }


        public override UnitAttackData GetAttackPoint()
        {
            return new UnitAttackData(soliderModel.attackPoint, soliderModel.magicAttackPoint);
        }

        public override UnitDefendData GetDefendPoint()
        {
            return new UnitDefendData(soliderModel.defendReducePercent, soliderModel.magicDefendReducePercent);
        }

        public override int GetMaxHp()
        {
            return soliderModel.maxHp;
        }
    }
}
