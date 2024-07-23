using System;
using System.IO;
using System.Linq;
using Gameplay.Player;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class CSVReader : MonoBehaviour
    {
        
        [MenuItem("Tools/CSV To Script")]
        public static void ReadCSV()
        {
            //选择路径方法
            string path = EditorUtility.OpenFilePanel("Select CSV File", Application.dataPath, "csv");
            if (path.Length != 0)
            {
                ReadFile(path);
            }
        }


        public static void ReadFile(string path)
        {
            if (File.Exists(path))
            {
                
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines.Skip(2))
                {
                    string[] value = line.Split(',');
                    SoliderBaseModel soliderModel = new SoliderBaseModel();
                    
                    //解析
                    soliderModel.soliderId = ParseInt(value[0]);
                    soliderModel.soliderName = value[1];
                    soliderModel.soliderDes = value[2];
                    
                    soliderModel.soliderType = (UnitType)ParseInt(value[3]);
                    soliderModel.cost = ParseInt(value[4]);
                    soliderModel.maxHp = ParseFloat(value[5]);
                    soliderModel.spawnNum = ParseInt(value[6]);
                    soliderModel.attackPoint = ParseInt(value[7]);
                    soliderModel.magicAttackPoint = ParseInt(value[8]);
                    soliderModel.defendPoint = ParseInt(value[9]);
                    soliderModel.magicDefendPoint = ParseInt(value[10]);

                    soliderModel.attackInterval = ParseFloat(value[11]);
                    soliderModel.attackRange = ParseFloat(value[12]);
                    soliderModel.attackNum = ParseInt(value[13]);
                    soliderModel.attackEnemyType = (UnitType)ParseInt(value[14]);
                    soliderModel.moveSpeed = ParseFloat(value[15]);
                    soliderModel.scenePrefabPath = value[16];
                    soliderModel.uiPrefabPath = value[17];

                    var soliderDatabase = DataManager.Instance.GetSoliderBaseModels();
                    if (!soliderDatabase.ContainsKey(soliderModel.soliderId))
                    {
                        soliderDatabase[soliderModel.soliderId] = soliderModel;
                    }
                }
            }
            var temp = DataManager.Instance.GetSoliderBaseModels();
            print(temp[1].soliderName);
        }
        
        private static int ParseInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"Unable to parse ");
            }
        }

        private static float ParseFloat(string value)
        {
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"Unable to parse ");
            }
        }
        
    }
}
