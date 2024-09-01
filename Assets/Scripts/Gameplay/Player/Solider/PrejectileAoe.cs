using System;
using DG.Tweening;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player
{
    public class PrejectileAoe : MonoBehaviour
    {
        

        public void OnInit(EnemyAgent agent)
        {
            var model = agent.enemyModel;
            transform.localScale = new Vector3(0, 0, 0);
            
            transform.DOScale(new Vector3(model.attackAoeRange*1.1f*2f, 0.0001f, model.attackAoeRange*1.1f*2f), 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                // 动画完成后销毁该物体
                Destroy(gameObject);
            });
        }
    }
}