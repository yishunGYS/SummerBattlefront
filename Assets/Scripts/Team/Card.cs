using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Text ErrorMessageText;

    public enum CardState
    {
        InLeftPanel,
        InTopPanel,
        InGamePanel,
        Inactive
    }
 

    public CardState currentState = CardState.Inactive;

    public void SetCardState(CardState newState)
    {
        //// 如果卡片要进入左栏框并且当前状态是上栏框
        //if (newState == CardState.InLeftPanel && currentState == CardState.InTopPanel)
        //{
        //    // 禁止激活，保持原状态
        //    Debug.Log("禁止激活，保持原状态");
        //    ShowErrorMessageText("Cannot activate card in Left Panel when it is already in the Top Panel.");
        //    return;
        //}

        //// 如果卡片要进入上栏框并且当前状态是左栏框
        //if (newState == CardState.InTopPanel && currentState == CardState.InLeftPanel)
        //{
        //    // 禁止激活，保持原状态
        //    Debug.LogWarning("Cannot activate card in Top Panel when it is already in the Left Panel.");
        //    return;
        //}

        // 更新卡片的状态
        currentState = newState;
        UpdateCardPosition();
    }

    private void UpdateCardPosition()
    {
        // 根据卡片的状态更新位置或外观
        switch (currentState)
        {
            case CardState.InLeftPanel:
                // 更新卡片的位置到左栏框
                transform.SetParent(GameObject.Find("TeamUnselectPanel").transform);
                break;
            case CardState.InTopPanel:
                // 更新卡片的位置到上栏框
                transform.SetParent(GameObject.Find("TopPanel").transform);
              
                break;
            //case CardState.InGamePanel:
            //    // 更新卡片的位置到游戏内的兵种栏
            //    transform.SetParent(GameObject.Find("GamePanel").transform);
            //    break;
            case CardState.Inactive:
                // 隐藏或禁用卡片
                gameObject.SetActive(false);
                break;
        }
    }

    private void ShowErrorMessageText(string message)
    {
        // 显示提示消息
        if (ErrorMessageText != null)
        {
            ErrorMessageText.text = message;
            ErrorMessageText.gameObject.SetActive(true);

            // 可选：设置一个定时器来隐藏提示消息
            Invoke("HideErrorMessage", 2.0f); // 2秒后隐藏消息
        }
    }

    private void HideErrorMessage()
    {
        if (ErrorMessageText != null)
        {
            ErrorMessageText.gameObject.SetActive(false);
        }
    }
}
