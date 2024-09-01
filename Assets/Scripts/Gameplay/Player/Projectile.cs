using System;
using Gameplay.Enemy;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Gameplay.Player
{
    public class Projectile : MonoBehaviour
    {
        public bool isAOE;
        //public PrejectileAoe aoePrefab;
        private UnitAgent atkAgent;
        private UnitAgent beAtkAgent;
        //private Vector3 moveDir;
        public float moveSpeed;
        public void OnInit(UnitAgent atkAgent,UnitAgent beAtkAgent)
        {
            this.atkAgent = atkAgent;
            this.beAtkAgent = beAtkAgent;
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
                        if (isAOE)
                        {
                            enemyCmpt.enemyLogic.OnTakeAOEDamage(
                                soliderAgent,
                                soliderAgent.soliderModel.attackAoeRange);
                        }
                        else
                        {
                            enemyCmpt.enemyLogic.OnTakeDamage(soliderAgent);
                        }
                        //enemyCmpt.enemyLogic.OnTakeDamage(soliderAgent);
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
                        if (isAOE)
                        {
                            // PrejectileAoe go =Instantiate(aoePrefab,soliderCmpt.transform.position -new Vector3(0,0.5f,0),Quaternion.identity);
                            // go.OnInit(enemyAgent);
                            soliderCmpt.soliderLogic.OnTakeAOEDamage(
                                enemyAgent,
                                enemyAgent.enemyModel.attackAoeRange);
                        }
                        else
                        {
                            soliderCmpt.soliderLogic.OnTakeDamage(enemyAgent);
                        }
                        //soliderCmpt.soliderLogic.OnTakeDamage(enemyAgent);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
        
    }
}
