using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class Bomb : MonoBehaviour
{
    [Header("爆炸所需时间")][SerializeField]private float clock;
    [Header("爆炸范围")][SerializeField]private float explodeRange;
    [Header("爆炸伤害")][SerializeField]private float explodeDamage;

    private void Update()
    {
        clock -= Time.deltaTime;

        if (clock <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        //获取爆炸范围内的全部敌军,对其造成伤害
        Collider[] hitEnemies =
            Physics.OverlapSphere(transform.position, explodeRange,
                LayerMask.GetMask("Enemy"));
        foreach (var collider in hitEnemies)
        {
            EnemyAgent nowEnemy = collider.GetComponent<EnemyAgent>();
            nowEnemy.enemyLogic.OnTakeDamage(explodeDamage,0,null);//使敌人血量降低
        }
        
        //获取爆炸范围内的全部友军,对其造成伤害
        Collider[] hitSoliders =
            Physics.OverlapSphere(transform.position, explodeRange,
                LayerMask.GetMask("Solider"));
        foreach (var collider in hitSoliders)
        {
            SoliderAgent nowEnemy = collider.GetComponent<SoliderAgent>();
            nowEnemy.soliderLogic.OnTakeDamage(explodeDamage,0,null);//使友方血量降低
        }
        
    }
}
