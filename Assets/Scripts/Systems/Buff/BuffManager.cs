using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Player;
using UnityEngine;

public class BuffManager
{
    private readonly UnitAgent _agent;
    private LinkedList<BuffInstance> _buffs = new();

    public BuffManager(UnitAgent agent)
    {
        _agent = agent;
    }

    public void AddBuff(BuffInfo buffInfo)
    {
        var newBuff = FindBuff(buffInfo);
        if (newBuff == null)
        {
            Debug.Log("unExist");
            newBuff = new BuffInstance(buffInfo, this);
            _buffs.AddLast(newBuff);
            SortBuff(_buffs);
        }
        else
        {
            Debug.Log("Exist");
            switch (buffInfo.buff刷新类型)
            {
                case BuffRefreshType.Replace:
                    newBuff.durationTimer = buffInfo.durationTime;
                    break;
                case BuffRefreshType.Add:
                    newBuff.durationTimer += buffInfo.durationTime;
                    break;
            }
        }
    }

    public void RemoveBuff(BuffInstance buffInstance)
    {
        buffInstance.buffInfo.在移除时?.Invoke();
        _buffs.Remove(buffInstance);
        SortBuff(_buffs);
    }

    //链表排序
    public void SortBuff(LinkedList<BuffInstance> buffs)
    {
        var current = buffs.First?.Next; // 从第二个节点开始
        while (current != null)
        {
            var insertionPoint = current.Previous;
            while (insertionPoint != null && current.Value.buffInfo.priority < insertionPoint.Value.buffInfo.priority)
            {
                // 交换节点值
                (current.Value, insertionPoint.Value) = (insertionPoint.Value, current.Value);

                insertionPoint = insertionPoint.Previous;
            }

            current = current.Next;
        }
    }

    public BuffInstance FindBuff(BuffInfo buffInfo)
    {
        foreach (var buff in _buffs)
            if (buff?.buffInfo.id == buffInfo.id)
                return buff;
        return null;
    }

    public void UpdateBuff()
    {
        for (var i = _buffs.First; i != null; i = i.Next)
            i.Value.OnBuffUpdate();
    }


    public int CalculateDamage(DamageInfo damageInfo)
    {
        var attackData = CalculateAttack(damageInfo.attacker);

        var defendData = CalculateDefend(damageInfo.beAttacker);
        var temp = attackData.attackPoint * (1 - defendData.defendReducePercent) +
                   attackData.magicAttackPoint * (1 - defendData.magicDefendReducePercent);


        return (int)Math.Floor(temp);
    }


    public UnitAttackData CalculateAttack(UnitAgent attacker)
    {
        var tempAttackData = new UnitAttackData(0, 0);
        //复制数据
        var attackerAttackData = attacker.GetAttackPoint();
        tempAttackData.attackPoint = attackerAttackData.attackPoint;
        tempAttackData.magicAttackPoint = attackerAttackData.magicAttackPoint;
        foreach (var item in _buffs)
        {
            if (item.buffInfo.buff效果类型 == BuffTag.伤害)
            {
                if (attackerAttackData.attackPoint != 0)
                {
                    tempAttackData.attackPoint *= (int)item.buffInfo.attackGainEffect;
                }

                if (attackerAttackData.magicAttackPoint != 0)
                {
                    tempAttackData.magicAttackPoint *= (int)item.buffInfo.attackGainEffect;
                }
            }
        }

        return tempAttackData;
    }

    public UnitDefendData CalculateDefend(UnitAgent beAttacker)
    {
        var tempDefenseData = new UnitDefendData(0, 0);
        //复制数据
        var beAttackerDefenseData = beAttacker.GetDefendPoint();
        tempDefenseData.defendReducePercent = beAttackerDefenseData.defendReducePercent;
        tempDefenseData.magicDefendReducePercent = beAttackerDefenseData.magicDefendReducePercent;

        foreach (var item in _buffs)
        {
            if (item.buffInfo.buff效果类型 == BuffTag.免伤)
            {
                var tempDefense = tempDefenseData.defendReducePercent + item.buffInfo.defendGainEffect;
                tempDefenseData.defendReducePercent = Math.Clamp(tempDefense, 0, 1);


                if (Math.Abs(tempDefenseData.defendReducePercent - 1) < 0.1f)
                {
                    Debug.Log("无敌");
                }


                var tempMagicDefense = tempDefenseData.magicDefendReducePercent + item.buffInfo.defendGainEffect;
                tempDefenseData.magicDefendReducePercent = Math.Clamp(tempMagicDefense, 0, 1);
            }
        }

        return tempDefenseData;
    }
}