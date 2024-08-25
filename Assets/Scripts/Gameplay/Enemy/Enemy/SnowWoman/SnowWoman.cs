namespace Gameplay.Enemy.Enemy.SnowWoman
{
    public class SnowWoman : EnemyAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            enemyLogic = new SnowWomanLogic(this);
            enemyLogic.OnInitEnemyFeatures();
        }
    }
}


