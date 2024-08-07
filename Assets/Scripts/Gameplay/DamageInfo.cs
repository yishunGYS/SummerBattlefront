using Gameplay.Player;

namespace Gameplay
{
    public class DamageInfo
    {
        public IAgent attacker;
        public IAgent beAttacker;

        public DamageInfo(IAgent attacker, IAgent beAttacker)
        {
            this.attacker = attacker;
            this.beAttacker = beAttacker;
        }
        
    }
}