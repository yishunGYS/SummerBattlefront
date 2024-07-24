using Systems;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class GameManager : MonoBehaviour {

		public static bool GameIsOver;

		public GameObject gameOverUI;
		public GameObject completeLevelUI;
		private CsvReader csvReader;
		void Start ()
		{
			GameIsOver = false;
			
			InitCsvReader();
			//test
			var temp = DataManager.Instance.GetSoliderBaseModels();
			print(temp[1].soliderDes);
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
	}
}
