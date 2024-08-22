using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;

namespace Gameplay.Item
{
    public class RiseLimitItem : ItemBase
    {
        public int RiseAmount = 0;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.gameObject.GetComponent<SoliderAgent>())
            {
                ItemEffect(RiseAmount);
                Destroy(this.gameObject);
            }
        }

        protected override void ItemEffect(int num)
        {
            base.ItemEffect(num);
            PlayerStats.Instance.RiseLimit(num);
        }
    }

}
