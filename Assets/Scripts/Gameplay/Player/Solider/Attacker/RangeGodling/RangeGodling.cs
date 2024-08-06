using Gameplay.Player.Solider.Attacker.RangeGodling;
using UnityEngine;

namespace Gameplay.Player.Solider
{
    
    public class RangeGodling: SoliderAgent
    {
        public Projectile projectile;

        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new RangeGodlingLogic(this);
        }
    }
}