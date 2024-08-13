using System;
using Ilumisoft.Health_System.Scripts.UI;
using UnityEngine;
using Utilities;

namespace Gameplay.Player
{

    public abstract class UnitAgent : MonoBehaviour
    {
        [HideInInspector]public int curHp;
        public BuffManager buffManager;
        private StateMachine fsm;
        public abstract UnitAttackData GetAttackPoint();
        
        public abstract UnitDefendData GetDefendPoint();

        public abstract int GetMaxHp();

        public virtual void OnInit()
        {
            InitHealthBar();
            fsm = GetComponent<StateMachine>();
            fsm.OnInit();
            buffManager = new BuffManager(this);
        }

        public void Update()
        {
            if (fsm!=null)
            {
                fsm.OnUpdate();
            }

            if (buffManager!=null)
            {
                buffManager.UpdateBuff();
            }
            
        }

        private void InitHealthBar()
        {
            transform.Find("Healthbar").GetComponent<Healthbar>().OnInit(this);
        }
        
        
    }
}
