using UnityEngine;
using UnityEngine.Events;

namespace Systems.Buff
{
    [CreateAssetMenu(fileName = "NewBuff", menuName = "Buff", order = 0)]
    public class BuffDataSO : ScriptableObject
    {
        //public Texture buffIcon;
        public string description;
    
        public int id;
        public int 最大层数;
        public int priority;
        
        public float attackGainEffect;  //攻击增益
        public float defendGainEffect;  //免伤增益
        
        public float durationTime;
        public float tickTime;
    
        public BuffType        buff触发类型;
        public BuffTag         buff效果类型;
        public BuffRefreshType buff刷新类型;

        public UnityEvent 在创建时;
        public UnityEvent 在移除时;
        public UnityEvent 在Hit触发时;
        public UnityEvent 在Tick触发时;
        public UnityEvent 在Stack触发时;
    }
    
}