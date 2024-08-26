using System;
using Gameplay.Enemy;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Gameplay.Player
{
    public class Projectile : MonoBehaviour
    {
        private UnitAgent unitAgent;
        private Vector3 moveDir;
        public float moveSpeed;

        public void OnInit(Vector3 targetPosition,UnitAgent agent)
        {
            unitAgent = agent;
            moveDir = targetPosition - transform.position;
        }

        private void Update()
        {
            Move();
        }

        //Õ∂÷¿ŒÔ∑…––
        private void Move()
        {
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Enemy"))
            {
                SoliderAgent soliderAgent = unitAgent as SoliderAgent;
                var enemyCmpt =  other.GetComponent<EnemyAgent>();
                
                enemyCmpt.enemyLogic.OnTakeDamage(soliderAgent);
                Destroy(gameObject);
            }
            
            
        }
        
        
    }
}
