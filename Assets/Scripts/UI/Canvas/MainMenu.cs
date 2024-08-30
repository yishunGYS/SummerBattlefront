using Systems;
using Systems.Level;
using UnityEngine;

namespace UI.Canvas
{
	public class MainMenu : MonoBehaviour {

		public string levelToLoad;

		public SceneFader sceneFader;

		public void Play ()
		{
			//sceneFader.FadeTo(levelToLoad);
			sceneFader.ChangeScene(levelToLoad);
		}

		public void Quit ()
		{
			Debug.Log("Exciting...");
			Application.Quit();
		}

	}
}
