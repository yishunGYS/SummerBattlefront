using System.Collections;
using System.Collections.Generic;
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