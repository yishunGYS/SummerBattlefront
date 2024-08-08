using UnityEngine;

namespace Gameplay.Player.Solider.Assist.Angel
{
    public class Angel : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new AngelLogic(this);
        }
    }
}
