using System;
using Gameplay.Enemy;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Gameplay.Player
{
    public class Projectile : MonoBehaviour
    {
        private UnitAgent atkAgent;
        private UnitAgent beAtkAgent;
        //private Vector3 moveDir;
        public float moveSpeed;

        public void OnInit(UnitAgent atkAgent,UnitAgent beAtkAgent)
        {
            this.atkAgent = atkAgent;
            this.beAtkAgent = beAtkAgent;
            //moveDir = targetPosition - transform.position;
        }

        private void Update()
        {
            Move();
        }

        //Õ∂÷¿ŒÔ∑…––
        private void Move()
        {
            var dir = beAtkAgent.transform.position - transform.position;
            transform.position += moveSpeed * Time.deltaTime * dir;
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (atkAgent.transform.CompareTag("Solider"))
            {
                SoliderAgent soliderAgent = atkAgent as SoliderAgent;
                var enemyCmpt =  other.GetComponent<EnemyAgent>();
                if (enemyCmpt)
                {
                    enemyCmpt.enemyLogic.OnTakeDamage(soliderAgent);
                    Destroy(gameObject);
                }

            }

            if (atkAgent.transform.CompareTag("Enemy"))
            {
                EnemyAgent enemyAgent = atkAgent as EnemyAgent;
                var soliderCmpt =  other.GetComponent<SoliderAgent>();
                if (soliderCmpt)
                {
                    soliderCmpt.soliderLogic.OnTakeDamage(enemyAgent);
                    Destroy(gameObject);
                }
            }
        }
    }
}
