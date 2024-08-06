using Managers;

namespace Gameplay.Player.Solider.Attacker.MeleeGodling
{
    public class MeleeGodingLogic : AttackerSoliderLogic
    {
        public MeleeGodingLogic(SoliderAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            DistanceBasedGetTarget();
        }

 

        public override void Attack()
        {
            base.Attack();
            playerBuffManager.AddBuff(BuffInventoryManager.Instance.GetBuffById(0));  //无敌buff
            MeleeAttack();
            // BuffInfo cureBuff = BuffInventoryManager.Instance.GetBuffById(0);
            // //cureBuff.在Tick触发时 += SmallCure;
            //
            // SoliderAgent solider1 = new SoliderAgent();
            // solider1.soliderLogic.playerBuffManager.AddBuff(cureBuff);
        }


    }
}
