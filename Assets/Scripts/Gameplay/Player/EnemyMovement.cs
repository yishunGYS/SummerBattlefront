using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {

        private Transform target;
        private int waypointIndex = 0;

        private Enemy enemy;
        private Transform[] pathPoints;

        void Start()
        {
            enemy = GetComponent<Enemy>();
        }

        void Update()
        {
            if (pathPoints == null || pathPoints.Length == 0) return;

            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.4f)
            {
                GetNextWaypoint();
            }

            enemy.speed = enemy.startSpeed;
        }

        public void SetPath(int pathIndex)
        {
            if (pathIndex < 0 || pathIndex >= Waypoints.paths.Count)
            {
                Debug.LogError("Invalid path index");
                return;
            }
            pathPoints = Waypoints.paths[pathIndex];
            waypointIndex = 0;
            target = pathPoints[0];
        }

        void GetNextWaypoint()
        {
            if (waypointIndex >= pathPoints.Length - 1)
            {
                EndPath();
                return;
            }

            waypointIndex++;
            target = pathPoints[waypointIndex];
        }

        void EndPath()
        {
            PlayerStats.Lives--;
            WaveSpawner.EnemiesAlive--;
            Destroy(gameObject);
        }
    }
}
