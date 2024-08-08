using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "BlockData", menuName = "LevelEditor/BlockData")]
    public class BlockData : ScriptableObject
    {
        public List<GameObject> blockPrefabs = new List<GameObject>();
    }
}
