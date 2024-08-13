using _3DlevelEditor_GYS;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Features
{
    public class TraverserFeature : MonoBehaviour
    {
        [HideInInspector] public SoliderAgent agent;
        //[HideInInspector] 
        public bool canTraver = true; //当前是否可以穿越敌人
        //[HideInInspector] 
        public bool isTraver; //当前是否处于穿越状态-穿越状态不可进行攻击/不可被阻挡-到达目标格子/被另一个角色阻挡时退出穿越状态
        [HideInInspector] public GridCell targetCell; //将要穿越到的格子
        [HideInInspector] public float preSpeed; //进入穿越状态前的速度
        //[HideInInspector] 
        public EnemyAgent preEnemy; //触发穿越状态的敌方士兵

        [Header("穿越过程速度")] public float traverSpeed;

        private void Awake()
        {
            agent = transform.GetComponent<SoliderAgent>();
            canTraver = true;
            isTraver = false;
        }
    }
}


