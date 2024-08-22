using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Item
{
    public enum ItemType
    {
        Resource,
        Solider
    }
    public class ItemBase : MonoBehaviour
    {
        public ItemType Type;

        protected virtual void ItemEffect() { }
        protected virtual void ItemEffect(int num) { }
        protected virtual void ItemEffect(float num) { }


        protected virtual void OnTriggerEnter(Collider other) { }
    }

}