using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;
using System.Collections;

namespace Gameplay.Features.EnemyFeature
{
    public class PotatoMine : MonoBehaviour
    {
        [Header("±¨’®ºÏ≤‚∑∂Œß")]
        [SerializeField] private float boomRange = 5f;

        [Header("±¨’®—”≥Ÿ ±º‰")]
        [SerializeField] private float explosionDelay = 2f;

        private bool isExploded = false;
        private bool isActivated = false;
        private Renderer mineRenderer;

        private void Awake()
        {
            mineRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!isActivated)
            {
                Collider[] hitSoliders = Physics.OverlapSphere(transform.position, boomRange, LayerMask.GetMask("Solider"));
                foreach (var collider in hitSoliders)
                {
                    SoliderAgent solider = collider.GetComponent<SoliderAgent>();
                    if (solider != null && !solider.GetComponent<TraverserFeature>()) // ºÏ≤È «∑Ò”– TraverserFeature
                    {
                        StartCoroutine(ActivateMine());
                        return;
                    }
                }
            }

            DrawExplosionRange();
        }

        private IEnumerator ActivateMine()
        {
            isActivated = true;

            float elapsed = 0f;
            while (elapsed < explosionDelay)
            {
                mineRenderer.material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                mineRenderer.material.color = Color.white;
                yield return new WaitForSeconds(0.1f);
                elapsed += 0.2f;
            }

            Explode();
        }

        private void Explode()
        {
            if (isExploded) return;

            isExploded = true;

            Collider[] hitSoliders = Physics.OverlapSphere(transform.position, boomRange, LayerMask.GetMask("Solider"));
            foreach (var collider in hitSoliders)
            {
                SoliderAgent solider = collider.GetComponent<SoliderAgent>();
                if (solider != null)
                {
                    Debug.Log(solider.name + " ±ª’®ªŸ");
                    solider.soliderLogic.Die();
                }
            }

            Destroy(gameObject);
        }

        private void DrawExplosionRange()
        {
            Vector3 start = transform.position;
            int segments = 20;
            float angle = 0f;
            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                Vector3 offset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * boomRange;
                Vector3 nextOffset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep)), 0, Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep))) * boomRange;

                Debug.DrawLine(start + offset, start + nextOffset, Color.blue);

                angle += angleStep;
            }
        }
    }
}
