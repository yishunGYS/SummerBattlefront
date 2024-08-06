using System;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player
{
    public class Projectile : MonoBehaviour
    {
        private SoliderAgent soliderAgent;
        private Vector3 moveDir;
        public float moveSpeed;

        public void OnInit(Vector3 targetPosition,SoliderAgent soliderAgent)
        {
            this.soliderAgent = soliderAgent;
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
                var enemyCmpt =  other.GetComponent<EnemyAgent>();
                enemyCmpt.enemyLogic.OnTakeDamage(soliderAgent);
                Destroy(gameObject);
            }
            
            
        }
        
        
    }
}
