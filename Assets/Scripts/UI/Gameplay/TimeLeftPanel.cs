using Systems;
using TMPro;

namespace UI.Gameplay
{
    public class TimeLeftPanel : UIBasePanel
    {
        private TextMeshProUGUI timeLeftText;

        private int resourceLimit;

        public override void Init()
        {
            base.Init();
            timeLeftText = transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            resourceLimit = PlayerStats.Instance.levelResourceLimit;
            timeLeftText.text = $"{resourceLimit}";
        }


        public void UpdateTime(float time)
        {
            timeLeftText.text = $"{time}";
        }
    }
}