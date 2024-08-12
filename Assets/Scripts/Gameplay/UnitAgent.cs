using Ilumisoft.Health_System.Scripts.UI;
using UnityEngine;

namespace Gameplay.Player
{

    public abstract class UnitAgent : MonoBehaviour
    {
        [HideInInspector]public int curHp;
        
        public abstract UnitAttackData GetAttackPoint();
        
        public abstract UnitDefendData GetDefendPoint();

        public abstract int GetMaxHp();

        public virtual void OnInit()
        {
            InitHealthBar();
        }

        private void InitHealthBar()
        {
            transform.Find("Healthbar").GetComponent<Healthbar>().OnInit(this);
        }
    }
}
