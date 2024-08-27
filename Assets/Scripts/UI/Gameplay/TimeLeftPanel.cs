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
            timeLeftText.text = $"Resource Left: \n {PlayerStats.Instance.levelResourceLimit:F2}";
        }


        public void UpdateTime(float time)
        {
            timeLeftText.text = $"Resource Left: \n {time:F2}";
        }
    }
}