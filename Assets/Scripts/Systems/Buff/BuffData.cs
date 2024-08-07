using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff", order = 0)]
public class BuffInfo : ScriptableObject
{
    public Texture buffIcon;
    
    public string description;
    
    public int id;
    public int 最大层数;
    public int priority;
    
    public float cureGainEffect;  //治疗效果
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

public enum BuffType
{
    单次,
    持续一段时间,
    持续每秒生效
}

public enum BuffTag
{
    伤害,
    免伤,
    治疗,
}

public enum BuffRefreshType
{
    Add,
    Replace,
}
