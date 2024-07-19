using Systems;
using Systems.Level;
using UnityEngine;

namespace UI.Canvas
{
	public class MainMenu : MonoBehaviour {

		public string levelToLoad = "MainLevel";

		public SceneFader sceneFader;

		public void Play ()
		{
			sceneFader.FadeTo(levelToLoad);
		}

		public void Quit ()
		{
			Debug.Log("Exciting...");
			Application.Quit();
		}

	}
}
