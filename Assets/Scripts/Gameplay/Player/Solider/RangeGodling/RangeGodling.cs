using UnityEngine;

namespace Gameplay.Player.Solider
{
    [RequireComponent(typeof(Projectile))]
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