using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelInformationSO", menuName = "ScriptableObjects/LevelInformationSO")]
    public class LevelInformationSo : ScriptableObject
    {
        //public int sceneId;
        public int levelID;
        //public float levelTime;
        public float levelResource;
        public string levelName;
        public string levelIntro;
        public List<Image> enemies;
        public List<int> unlockSoliderId = new List<int>();
        public List<int> lockSoliderIds = new List<int>();
    }
}


