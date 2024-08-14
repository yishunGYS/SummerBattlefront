using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //public RectTransform TeamUnselectedPanel;
    void Start() 
    {
        GenerateCards();
    }

    void GenerateCards() 
    {
        foreach (var item in DataManager.Instance.GetSoliderBaseModels().Values)
        {
            var prefabPath = item.uiPrefabPath;
            var prefab = Resources.Load<GameObject>(prefabPath);
            GameObject card = Instantiate(prefab, transform);
        }
    }
    
}

