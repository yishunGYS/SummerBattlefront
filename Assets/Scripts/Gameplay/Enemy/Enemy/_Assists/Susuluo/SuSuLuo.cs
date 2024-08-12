namespace Gameplay.Enemy.Enemy._Assists.Susuluo
{
    public class SuSuLuo : EnemyAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            enemyLogic = new SuSuLuoLogic(this);
        }
    }
}
