using TMPro;

namespace UI.Gameplay
{
    public class TipPanel : UIBasePanel
    {
        private TextMeshProUGUI tipText;
        public override void Init()
        {
            base.Init();
            tipText = transform.Find("TipText").GetComponent<TextMeshProUGUI>();
        }

        public void OnShowText(string texts)
        {
            tipText.text = texts;
        }
    }
}