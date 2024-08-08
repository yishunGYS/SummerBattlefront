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

    private SoliderAgent soliderAgent;
    private void Update()
    {
        clock -= Time.deltaTime;

        if (clock <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    public void OnInit(SoliderAgent agent)
    {
        soliderAgent = agent;
    }

    private void Explode()
    {
        //获取爆炸范围内的全部敌军,对其造成伤害
        Collider[] hitEnemies =
            Physics.OverlapSphere(transform.position, soliderAgent.soliderModel.attackAoeRange,
                LayerMask.GetMask("Enemy"));
        foreach (var collider in hitEnemies)
        {
            EnemyAgent nowEnemy = collider.GetComponent<EnemyAgent>();
            nowEnemy.enemyLogic.OnTakeDamage(soliderAgent);//使敌人血量降低
        }
    }
}
