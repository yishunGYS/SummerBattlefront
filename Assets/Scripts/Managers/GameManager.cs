using Gameplay.Enemy;
using Systems;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : Singleton<GameManager> {

		public static bool GameIsOver;

		public GameObject gameOverUI;
		public GameObject completeLevelUI;
		private CsvReader csvReader;

		public GameObject EnemyContainer;
		void Start ()
		{
			GameIsOver = false;
			
			InitCsvReader();
			InitEnemy();
		}
		
		
		// Update is called once per frame
		void Update () {
			if (GameIsOver)
				return;

			if (PlayerStats.Lives <= 0)
			{
				EndGame();
			}
		}

		void EndGame ()
		{
			GameIsOver = true;
			gameOverUI.SetActive(true);
		}

		public void WinLevel ()
		{
			GameIsOver = true;
			completeLevelUI.SetActive(true);
		}


		private void InitCsvReader()
		{
			csvReader = new CsvReader();
			csvReader.OnStart();
		}

		private void InitEnemy()
		{
			foreach (var item in EnemyContainer.GetComponentsInChildren<EnemyAgent>())
			{
				item.OnInit();
			}
		}
	}
}
