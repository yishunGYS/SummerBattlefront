using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Card;

public class CardDragHandler : MonoBehaviour, IPointerClickHandler
{
    private Card card;


    private void Start()
    {
        card = GetComponent<Card>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnMouseDown");
        if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("TeamUnselectPanel").GetComponent<RectTransform>(), Input.mousePosition))
        {
            card.SetCardState(CardState.InTopPanel);
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("TopPanel").GetComponent<RectTransform>(), Input.mousePosition))
        {
            card.SetCardState(CardState.InLeftPanel);
        }
    }
    

    //private void OnMouseDown()
    //{
    //    Debug.Log("OnMouseDown");
    //    if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("TeamUnselectPanel").GetComponent<RectTransform>(), Input.mousePosition))
    //    {
    //        card.SetCardState(CardState.InLeftPanel);
    //    }
    //    else if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("TopPanel").GetComponent<RectTransform>(), Input.mousePosition))
    //    {
    //        card.SetCardState(CardState.InTopPanel);
    //    }
    //}

}
