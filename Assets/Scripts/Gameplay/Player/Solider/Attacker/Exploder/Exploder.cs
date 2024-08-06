using System.Collections;
using System.Collections.Generic;
using Gameplay.Player.Solider.Attacker.Exploder;
using UnityEngine;

namespace Gameplay.Player.Solider.Exploder
{
    public class Exploder : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new ExploderLogic(this);
        }
    }
}