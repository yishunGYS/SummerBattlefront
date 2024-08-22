using UnityEngine;

namespace UI.Gameplay
{
    public class UIBasePanel : MonoBehaviour
    {
        public virtual void Init()
        {
            ClosePanel();
        }

        public virtual void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        public virtual void ClosePanel()
        {
            gameObject.SetActive(false);
        }
        
    }
}