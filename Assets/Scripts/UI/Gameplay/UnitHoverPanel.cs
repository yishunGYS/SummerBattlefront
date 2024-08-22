using TMPro;
using UnityEngine;

namespace UI.Gameplay
{
    public class UnitHoverPanel : UIBasePanel
    {
        private TextMeshProUGUI unitName;
        private TextMeshProUGUI unitDescribe;

        public override void Init()
        {
            base.Init();
            unitName = transform.Find("SoliderName").GetComponent<TextMeshProUGUI>();
            unitDescribe = transform.Find("SoliderDes").GetComponent<TextMeshProUGUI>();
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
        }

        
        public void OnHoverEnter(Vector3 pos,string soliderName,string des)
        {
            GetComponent<RectTransform>().position = pos;
            unitName.text = soliderName;
            unitDescribe.text = des;
        }

        
    }
}