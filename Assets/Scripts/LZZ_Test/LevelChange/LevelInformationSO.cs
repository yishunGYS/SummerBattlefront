using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelInformationSO", menuName = "ScriptableObjects/LevelInformationSO")]
    public class LevelInformationSO : ScriptableObject
    {
        public int sceneId;
        public int levelID;
        public string levelName;
        public string levelIntro;
        public List<Image> enemies;
    }
}


