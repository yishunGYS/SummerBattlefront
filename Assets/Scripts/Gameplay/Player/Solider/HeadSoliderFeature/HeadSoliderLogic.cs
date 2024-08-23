using Managers;

namespace Gameplay.Player
{
    public class HeadSoliderLogic : SoliderLogicBase
    {
        public HeadSoliderLogic(SoliderAgent agent) : base(agent)
        {
        }

        protected override void Die()
        {
            if (BlockManager.instance.headSoliderBlocks.ContainsKey(soliderAgent))
            {
                BlockManager.instance.OnHeadSoliderDestory(soliderAgent);
            }
            base.Die();
        }
    }
}