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
                buffЧ������ = BuffTag.����,
                buff�������� = BuffType.����һ��ʱ��,
                durationTime = invincibleTime
            };
            agent.buffManager.AddBuff(invincibleBuff);
        }
        
        
    }
}
