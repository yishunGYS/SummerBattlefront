using UnityEngine;

namespace _3DlevelEditor_GYS
{
    [CreateAssetMenu(fileName = "BlockData", menuName = "LevelEditor/BlockData")]
    public class BlockData : ScriptableObject
    {
        public GameObject blockPrefab;
    }
}