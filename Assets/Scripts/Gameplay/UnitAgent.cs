using UnityEngine;

namespace Gameplay.Player
{

    public abstract class UnitAgent : MonoBehaviour
    { 
        public abstract UnitAttackData GetAttackPoint();
        
        public abstract UnitDefendData GetDefendPoint();
    }
}
