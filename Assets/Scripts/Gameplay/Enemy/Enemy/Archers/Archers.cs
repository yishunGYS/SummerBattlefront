using Gameplay.Player;

namespace Gameplay.Enemy.Enemy.Archers
{
    public class Archers : EnemyAgent
    {
        public Projectile projectile;
        public override void OnInit()
        {
            base.OnInit();
            enemyLogic = new ArchersLogic(this);
            enemyLogic.OnInitEnemyFeatures();
        }
    }
}


