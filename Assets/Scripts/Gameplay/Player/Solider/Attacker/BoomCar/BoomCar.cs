namespace Gameplay.Player.Solider.Attacker.BoomCar
{
    public class BoomCar : SoliderAgent
    {
        public override void OnInit()
        {
            base.OnInit();
            soliderLogic = new BoomCarLogic(this);
        }
    }
}
