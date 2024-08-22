using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class UIPlacedView : MonoBehaviour
    {
        private Image img;
        private Color originalColor;
        private Color darkenedColor;
        private TextMeshProUGUI  costText;
        // private RectTransform hoverPanel;
        // private TextMeshProUGUI unitName;
        // private TextMeshProUGUI unitDescribe;
        public void OnInit()
        {
            img = GetComponent<Image>();
            originalColor = img.color;
            darkenedColor = originalColor * 0.1f;
            costText = transform.Find("Cost").GetComponent<TextMeshProUGUI>();

            // hoverPanel = Resources.Load<RectTransform>("UIPanel/UnitHoverPanel");
            // hoverPanel = Instantiate(hoverPanel);
            //
            // hoverPanel.gameObject.SetActive(false);
            // unitName = hoverPanel.transform.Find("SoliderName").GetComponent<TextMeshProUGUI>();
            // unitDescribe = hoverPanel.transform.Find("SoliderDes").GetComponent<TextMeshProUGUI>();
        }

        public void ChangeUIColor(bool isSelect)
        {
            img.color = isSelect ? darkenedColor : originalColor;
        }

        public void SetCostText(int cost)
        {
            costText.text = cost.ToString();
        }

        public void OnHoverUI(string thisName,string des)
        {
            //hoverPanel.gameObject.SetActive(true);
            //unitName.text = thisName;
            //unitDescribe.text = des;
            
            
        }

        public void OnExitHover()
        {
            //hoverPanel.gameObject.SetActive(false);
        }
    }
    

}