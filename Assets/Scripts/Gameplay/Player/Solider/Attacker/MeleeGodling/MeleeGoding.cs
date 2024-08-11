using System.Diagnostics;
using Gameplay.Player.Solider.Attacker.MeleeGodling;

namespace Gameplay.Player.Solider.MeleeGodling
{
    public class MeleeGoding : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new MeleeGodingLogic(this);
        }
        
        
    }
}
