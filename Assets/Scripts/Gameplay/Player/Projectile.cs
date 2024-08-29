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
            DetectCollide();
        }

        //Õ∂÷¿ŒÔ∑…––
        private void Move()
        {
            if (beAtkAgent == null)
            {
                Destroy(gameObject);
                return;
            }
            var dir = beAtkAgent.transform.position - transform.position;
            transform.position += moveSpeed * Time.deltaTime * dir.normalized;
        }


        public void DetectCollide()
        {
            if (atkAgent == null)
            {
                return;
            }
            
            if (atkAgent.transform.CompareTag("Solider"))
            {
                SoliderAgent soliderAgent = atkAgent as SoliderAgent;
                Collider[] hitColliders =
                    Physics.OverlapSphere(transform.position, transform.localScale.x/2,
                        LayerMask.GetMask("Enemy"));
                foreach (var collider in hitColliders)
                {
                    var enemyCmpt = collider.GetComponent<EnemyAgent>();
                    if (enemyCmpt&& soliderAgent.soliderLogic.attackTargets.Contains(enemyCmpt))
                    {
                        enemyCmpt.enemyLogic.OnTakeDamage(soliderAgent);
                        Destroy(gameObject);
                        break;
                    }
                }
            }

            if (atkAgent.transform.CompareTag("Enemy"))
            {
                EnemyAgent enemyAgent = atkAgent as EnemyAgent;
                Collider[] hitColliders =
                    Physics.OverlapSphere(transform.position, transform.localScale.x/2,
                        LayerMask.GetMask("Solider"));
                foreach (var collider in hitColliders)
                {
                    var soliderCmpt = collider.GetComponent<SoliderAgent>();
                    if (soliderCmpt&& enemyAgent.enemyLogic.attackTargets.Contains(soliderCmpt))
                    {
                        soliderCmpt.soliderLogic.OnTakeDamage(enemyAgent);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
        
    }
}
