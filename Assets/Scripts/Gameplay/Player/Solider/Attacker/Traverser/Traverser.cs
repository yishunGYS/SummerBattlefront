namespace Gameplay.Player.Solider.Attacker.Traverser
{
    public class Traverser : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new SoliderLogicBase(this);
        }
    }
}


