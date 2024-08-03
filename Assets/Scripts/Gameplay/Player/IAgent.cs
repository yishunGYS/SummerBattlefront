using UnityEngine;

namespace Gameplay.Player
{
    public interface IAgent
    {
        UnitAttackData GetAttackPoint();
        
        UnitDefendData GetDefendPoint();
    }
}
