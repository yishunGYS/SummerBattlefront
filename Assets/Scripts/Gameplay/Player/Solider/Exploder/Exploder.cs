using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.Exploder
{
    public class Exploder : SoliderAgent
    {
        [Header("死亡时生成的炸弹")] [SerializeField] public GameObject bomb;
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new ExploderLogic(this);
        }
    }
}