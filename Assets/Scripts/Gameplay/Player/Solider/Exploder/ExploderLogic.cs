using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.Exploder
{
    public class ExploderLogic : SoliderLogicBase
    {
        private Exploder exploderAgent;

        //自爆兵
        //当攻击范围内存在敌人时,将血量设置为0->调用Die()函数->在当前位置生成一个炸弹Prefab->炸弹x秒后对范围内所有角色造成伤害(己方+敌方)
        public ExploderLogic(SoliderAgent agent) : base(agent)
        {
            exploderAgent = (Exploder)agent;
        }
        
        //自爆兵的攻击行为--暂时为空
        public override void Attack()
        {
            soliderAgent.soliderModel.maxHp = 0;
        }
        //死亡时获取周围友军和敌军,对其造成伤害
        public override void Die()
        {
            GameObject.Instantiate(exploderAgent.bomb,soliderAgent.transform.position,Quaternion.identity);
            base.Die();
        }


    }
}


