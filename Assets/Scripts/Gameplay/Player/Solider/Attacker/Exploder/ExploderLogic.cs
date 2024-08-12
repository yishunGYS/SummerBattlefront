using Gameplay.Player.Solider.Attacker;
using Gameplay.Player.Solider.Attacker.Exploder;
using UnityEngine;

namespace Gameplay.Player.Solider
{
    public class ExploderLogic : AttackerSoliderLogic
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
            Die();
        }

        //死亡时获取周围友军和敌军,对其造成伤害
        protected override void Die()
        {
            var bomb = Object.Instantiate(exploderAgent.bomb, soliderAgent.transform.position, Quaternion.identity);
            bomb.GetComponent<Bomb>().OnInit(exploderAgent);
            base.Die();
        }
    }
}