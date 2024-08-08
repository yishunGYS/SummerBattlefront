using System.Collections;
using System.Collections.Generic;
using Gameplay.Player.Solider.Attacker.SkeletonArmy;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Player.Solider.SkeletonArmy
{
    public class SkeletonArmy : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new SkeletonArmyLogic(this);
        }
    }
}
