using Systems;
using Systems.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Canvas
{
	public class PauseMenu : MonoBehaviour {

		public GameObject ui;

		public string menuSceneName = "MainMenu";

		public SceneFader sceneFader;

		void Update ()
		{
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
			{
				Toggle();
			}
		}

		public void Toggle ()
		{
			ui.SetActive(!ui.activeSelf);

			if (ui.activeSelf)
			{
				Time.timeScale = 0f;
			} else
			{
				Time.timeScale = 1f;
			}
		}

		public void Retry ()
		{
			Toggle();
			sceneFader.FadeTo(SceneManager.GetActiveScene().name);
		}

		public void Menu ()
		{
			Toggle();
			sceneFader.FadeTo(menuSceneName);
		}

	}
}
