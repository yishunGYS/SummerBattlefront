namespace Gameplay.Player.Solider.Attacker.Valkyrie
{
    public class Valkyrie : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new ValkyrieLogic(this);
        }
    }
}