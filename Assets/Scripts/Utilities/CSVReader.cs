using System;
using System.IO;
using System.Linq;
using Gameplay.Enemy;
using Gameplay.Player;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class CsvReader 
    {

        public void OnStart()
        {
            //Solider
            string soliderFileName = "Solider.csv";
            string soliderFilePath = Path.Combine(Application.streamingAssetsPath, soliderFileName);
            if (soliderFilePath.Length != 0)
            {
                ReadSoliderFile(soliderFilePath);
            }
            
            //Enemy
            string enemyFileName = "Enemies.csv";
            string enemyFilePath = Path.Combine(Application.streamingAssetsPath, enemyFileName);
            if (enemyFilePath.Length != 0){
                ReadEnemyFile(enemyFilePath);
            }
        }




        private void ReadSoliderFile(string path)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines.Skip(2))
                {
                    string[] value = line.Split(',');
                    SoliderModelBase soliderModel = new SoliderModelBase();
                    
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
                    soliderDatabase.TryAdd(soliderModel.soliderId, soliderModel);
                }
            }
        }

        private void ReadEnemyFile(string enemyFilePath)
        {
            if (File.Exists(enemyFilePath))
            {
                string[] lines = File.ReadAllLines(enemyFilePath);
                foreach (var line in lines.Skip(2))
                {
                    string[] value = line.Split(',');
                    EnemyModelBase enemyModel = new EnemyModelBase();

                    //解析
                    enemyModel.enemyId = ParseInt(value[0]);
                    enemyModel.enemyName = value[1];
                    enemyModel.enemyDes = value[2];

                    enemyModel.enemyType = (UnitType)ParseInt(value[3]);
                    enemyModel.maxHp = ParseFloat(value[4]);
                    enemyModel.spawnNum = ParseInt(value[5]);
                    enemyModel.attackPoint = ParseInt(value[6]);
                    enemyModel.magicAttackPoint = ParseInt(value[7]);
                    enemyModel.defendPoint = ParseInt(value[8]);
                    enemyModel.magicDefendPoint = ParseInt(value[9]);
                    
                    enemyModel.attackInterval = ParseFloat(value[10]);
                    enemyModel.attackRange = ParseFloat(value[11]);
                    enemyModel.attackNum = ParseInt(value[12]);
                    enemyModel.attackEnemyType = (UnitType)ParseInt(value[13]);
                    enemyModel.blockNum = ParseInt(value[14]);
                    
                    enemyModel.scenePrefabPath = value[15];
                    enemyModel.uiPrefabPath = value[16];
                    
                    var enemyDatabase = DataManager.Instance.GetEnemyBaseModels();
                    enemyDatabase.TryAdd(enemyModel.enemyId, enemyModel);
                }
            }
        }

        private int ParseInt(string value)
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

        private float ParseFloat(string value)
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
