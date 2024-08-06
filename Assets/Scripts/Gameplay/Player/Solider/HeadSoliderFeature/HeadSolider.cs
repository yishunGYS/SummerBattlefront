using Gameplay.Player.Solider.MeleeGodling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public class HeadSolider : SoliderAgent
    {

        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new HeadSoliderLogic(this);
        }
    }
}

