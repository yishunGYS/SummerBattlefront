using UnityEngine;

namespace Gameplay.Player.Solider.Assist
{
    class CureSoliderTarget
    {
        public int hp;
        public SoliderAgent target;

        public CureSoliderTarget(int hp, SoliderAgent target)
        {
            this.hp = hp;
            this.target = target;
        }
    }
    
    public class AssistSoliderLogic : SoliderLogicBase
    {
        protected AssistSoliderLogic(SoliderAgent agent) : base(agent)
        {
            
        }
        
        
        //辅助/治疗获取目标 override


    }
}
