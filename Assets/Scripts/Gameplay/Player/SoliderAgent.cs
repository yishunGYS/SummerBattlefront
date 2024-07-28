using System;
using Managers;
using UnityEngine;
using Utilities;

namespace Gameplay.Player
{
    public class SoliderAgent : MonoBehaviour,IAgent
    {
        public SoliderModelBase soliderModel;
        public int soliderId;

        public SoliderLogicBase soliderLogic;
        private StateMachine fsm;

        //instantiate时
        public virtual void OnInit()
        {

            InitData();
            fsm = GetComponent<StateMachine>();
            fsm.OnInit();
            
        }

        private void Update()
        {
            fsm.OnUpdate();
        }
        
        private void InitData()
        {
            if (DataManager.Instance.GetSoliderBaseModels().TryGetValue(soliderId, out SoliderModelBase model))
            {
                soliderModel = model;
                print(soliderModel.soliderName);
                print("获取到该数据");
            }
            else
            {
                print("没有获取到该数据");
            }
        }
    }
}
