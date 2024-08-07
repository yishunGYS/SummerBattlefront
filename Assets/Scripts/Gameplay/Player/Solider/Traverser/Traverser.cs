using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public class Traverser : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new SoliderLogicBase(this);
        }
    }
}


