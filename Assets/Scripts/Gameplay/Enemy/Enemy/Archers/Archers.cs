namespace Gameplay.Enemy.Enemy.Archers
{
    public class Archers : EnemyAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            enemyLogic = new ArchersLogic(this);
        }
    }
}


