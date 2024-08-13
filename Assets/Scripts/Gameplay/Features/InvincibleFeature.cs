using System;
using Gameplay.Player;
using Managers;
using Systems.Buff;
using UnityEngine;

namespace Gameplay.Features
{
    [RequireComponent(typeof(UnitAgent))]
    public class InvincibleFeature : MonoBehaviour
    {
        private UnitAgent agent;
        private BuffModel invincibleBuff;
        
        public float invincibleTime = 4f;
        private void Start()
        {
            agent = GetComponent<UnitAgent>();
            invincibleBuff = new BuffModel
            {
                defendGainEffect = 1,
                buff效果类型 = BuffTag.免伤,
                buff触发类型 = BuffType.持续一段时间,
                durationTime = invincibleTime
            };
            agent.buffManager.AddBuff(invincibleBuff);
        }
        
        
    }
}
