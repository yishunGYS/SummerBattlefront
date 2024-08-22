using Gameplay.Enemy;
using Gameplay.Features.EnemyFeature;
using Gameplay.Item;
using Systems;
using Team;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager> {

		//public static bool GameIsOver;

		//public GameObject gameOverUI;
		//public GameObject completeLevelUI;
		private CsvReader csvReader;

		private GameObject EnemyContainer;
		void Start ()
		{
			//GameIsOver = false;
			//EnemyContainer = GameObject.Find("EnemyContainer");
			//InitCsvReader();
			//InitEnemy(EnemyContainer.transform);

			//UIManager.Instance.OnStart();
			//SpawnManager.Instance.OnStart();
		}

        public void OnLevelStart()
        {
            EnemyContainer = GameObject.Find("EnemyContainer");
            InitCsvReader();
            InitEnemy(EnemyContainer.transform);

			UIManager.Instance.OnStart();
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
