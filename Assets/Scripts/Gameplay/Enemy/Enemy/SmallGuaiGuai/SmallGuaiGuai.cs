namespace Gameplay.Enemy.Enemy.SmallGuaiGuai
{
    public class SmallGuaiGuai : EnemyAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            enemyLogic = new SmallGuaiGuaiLogic(this);
            enemyLogic.OnInitEnemyFeatures();
        }
    }
}
