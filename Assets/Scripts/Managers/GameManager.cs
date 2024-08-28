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
				// ������״̬
				PlayerStats.Instance.ResetPlayerStats();

				// Ȼ���ʼ���ؿ���ص�����
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
				// ��������屾����EnemyAgent���������OnInit
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
				
				
				// �ݹ���ô˺����Լ������������
				InitEnemy(child);
			}
		}
	}
}
