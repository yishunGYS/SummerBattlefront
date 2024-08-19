using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Item
{
    public enum RiseStats
    {
        Attack,
        Defence,
        AttackSpeed
    }

    public class RiseSoliderStatsItem : ItemBase
    {
        public RiseStats RiseStats;
        public float RiseAmount;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.gameObject.GetComponent<SoliderAgent>())
            {

                ItemManager.instance.AddItem(this);

                ItemEffect();

                Destroy(this.gameObject);
            }
        }

        protected override void ItemEffect()
        {
            ItemManager.instance.UseSoliderItem(this);
        }
    }
}
