using System.Collections.Generic;
using UnityEngine;

namespace _3DlevelEditor_GYS
{
    [CreateAssetMenu(fileName = "BlockData", menuName = "LevelEditor/BlockData")]
    public class BlockData : ScriptableObject
    {
        public List<GameObject> blockPrefabs = new List<GameObject>();
    }
}
