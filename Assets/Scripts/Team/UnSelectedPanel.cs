using Managers;
using UnityEngine;
using Utilities;

namespace Team
{
    public class UnSelectedPanel : MonoBehaviour
    {
        public RectTransform TeamUnselectedPanel;
        
        
        public void OnStart() 
        {
            GenerateCards();
        }

        private void GenerateCards() 
        {
            foreach (var item in DataManager.Instance.GetSoliderBaseModels().Values)
            {
                var prefabPath = item.uiPrefabPath;
                var prefab = Resources.Load<GameObject>(prefabPath);
                GameObject card = Instantiate(prefab, transform);
            }
        }
    
    }
}

