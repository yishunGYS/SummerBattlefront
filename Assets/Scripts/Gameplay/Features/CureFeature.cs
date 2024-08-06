using System;
using Gameplay.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Features
{
    [RequireComponent(typeof(SoliderAgent))]
    public class CureFeature : MonoBehaviour
    {
        public enum HealingFormula
        {
            BaseAttack,
            BaseMaxHp,
        }
        
        [EnumToggleButtons]
        public HealingFormula healingFormula;
        
        
        [ShowIf("healingFormula", HealingFormula.BaseAttack)]
        public float baseAttackPercentage;

        [ShowIf("healingFormula", HealingFormula.BaseMaxHp)]
        public float baseMaxHpPercentage;

        private SoliderAgent agent;

        private void Awake()
        {
            agent = GetComponent<SoliderAgent>();
        }



        // public void Cure()
        // {
        //     foreach (var solider in agent.soliderLogic.attackTargets)
        //     {
        //         solider.
        //     }
        //     
        // }
    }
}