using Gameplay.Player;
using Gameplay.Player.Solider.SkeletonArmy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.Valkyrie
{
    public class Valkyrie : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new ValkyrieLogic(this);
        }
    }
}