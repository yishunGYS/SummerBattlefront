using Gameplay.Player;

namespace Gameplay.Enemy.Enemy.SuperWoman
{
    public class SuperWoman : EnemyAgent
    {
        public Projectile projectile;
        public override void OnInit()
        {
            base.OnInit();
            enemyLogic = new SuperWomanLogic(this);
            enemyLogic.OnInitEnemyFeatures();
        }
    }
}


