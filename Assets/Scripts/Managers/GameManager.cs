using Gameplay.Enemy;
using Gameplay.Features.EnemyFeature;
using Gameplay.Item;
using Systems;
using Systems.Edu;
using Team;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager> {
		private CsvReader csvReader;

		private GameObject EnemyContainer;

        public void OnLevelStart()
        {
            EnemyContainer = GameObject.Find("EnemyContainer");
            InitCsvReader();
            InitEnemy(EnemyContainer.transform);

            DataManager.Instance.OnStart();
	
			SpawnManager.Instance.OnStart();
			ItemManager.instance.OnStart();
			
			if (PlayerStats.Instance != null)
			{
				// 先重置状态
				PlayerStats.Instance.ResetPlayerStats();

				// 然后初始化关卡相关的数据
				PlayerStats.Instance.OnLevelStart();

				Debug.Log("PlayerStats.Instance != null");
			}


			if (EduSystem.Instance)
			{
				EduSystem.Instance.OnTeachClickTeamAssemble();
			}
		}
        

		private void InitCsvReader()
		{
			csvReader = new CsvReader();
			csvReader.OnStart();
		}

		private void InitEnemy(Transform parent)
		{
			if (!EnemyContainer ) { return; }
			foreach (Transform child in parent)
			{
				// 如果子物体本身有EnemyAgent组件，调用OnInit
				var enemyAgent = child.GetComponent<EnemyAgent>();
				if (enemyAgent != null)
				{
					enemyAgent.OnInit();
				}
				//Test
				var enemySelfCureFeatureCmpt = child.GetComponent<EnemySelfCureFeature>();
				if (enemySelfCureFeatureCmpt != null)
				{
					enemySelfCureFeatureCmpt.OnInit();
				}
				
				
				// 递归调用此函数以检查所有子物体
				InitEnemy(child);
			}
		}
	}
}
