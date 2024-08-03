using _3DlevelEditor_GYS;
using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BlockManager : MonoBehaviour
    {
        public static BlockManager instance;

        public SerializableDictionary<GameObject, List<GridCell>> headSoliderBlocks;
        public SerializableDictionary<StartPoint, List<GridCell>> startPointBlocks;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one BlockManager in scene!");
                return;
            }
            instance = this;
        }


    }

}