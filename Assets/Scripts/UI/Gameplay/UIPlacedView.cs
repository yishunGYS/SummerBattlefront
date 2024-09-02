using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class UIPlacedView : MonoBehaviour
    {
        public Image selectImg;
        private Image img;
        private Color originalColor;
        private Color darkenedColor;
        private TextMeshProUGUI  costText;
        public void OnInit()
        {
            img = GetComponent<Image>();
            originalColor = img.color;
            darkenedColor = originalColor * 0.1f;
            costText = transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            selectImg.gameObject.SetActive(false);
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


        public void OnSelectInBattle(bool isSelect)
        {
            selectImg.gameObject.SetActive(isSelect);
        }
    }
    

}