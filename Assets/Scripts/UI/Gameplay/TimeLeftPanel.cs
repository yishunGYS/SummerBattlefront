using Systems;
using TMPro;

namespace UI.Gameplay
{
    public class TimeLeftPanel : UIBasePanel
    {
        private TextMeshProUGUI timeLeftText;


        public override void Init()
        {
            base.Init();
            timeLeftText = transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            timeLeftText.text = $"Time Left: {PlayerStats.Instance.levelTimeLimit:F2} s";
        }


        public void UpdateTime(float time)
        {
            timeLeftText.text = $"Time Left: {time:F2} s";
        }
    }
}