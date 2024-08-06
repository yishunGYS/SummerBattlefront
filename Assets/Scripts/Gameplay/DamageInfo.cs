using Gameplay.Player;

namespace Gameplay
{
    public class DamageInfo
    {
        public UnitAgent attacker;
        public UnitAgent beAttacker;

        public DamageInfo(UnitAgent attacker, UnitAgent beAttacker)
        {
            this.attacker = attacker;
            this.beAttacker = beAttacker;
        }
        
    }
}