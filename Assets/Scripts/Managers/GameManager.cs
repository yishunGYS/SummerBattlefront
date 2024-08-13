using Gameplay.Enemy;
using Gameplay.Features.EnemyFeature;
using Systems;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager> {

		//public static bool GameIsOver;

		//public GameObject gameOverUI;
		//public GameObject completeLevelUI;
		private CsvReader csvReader;

		public GameObject EnemyContainer;
		void Start ()
		{
			//GameIsOver = false;
			
			InitCsvReader();
			InitEnemy(EnemyContainer.transform);
		}
		
		
		// Update is called once per frame
		void Update () {
			//if (GameIsOver)
			//	return;

			//if (PlayerStats.Lives <= 0)
			//{
			//	EndGame();
			//}
		}

		void EndGame ()
		{
			//GameIsOver = true;
			//gameOverUI.SetActive(true);
		}

		public void WinLevel ()
		{
			//GameIsOver = true;
			//completeLevelUI.SetActive(true);
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
