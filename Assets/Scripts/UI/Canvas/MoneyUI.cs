using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Canvas
{
	public class MoneyUI : MonoBehaviour {

		public Text moneyText;

		// Update is called once per frame
		void Update () {
			moneyText.text = "$" + PlayerStats.Money.ToString();
		}
	}
}
