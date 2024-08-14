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
        //// �����ƬҪ�����������ҵ�ǰ״̬��������
        //if (newState == CardState.InLeftPanel && currentState == CardState.InTopPanel)
        //{
        //    // ��ֹ�������ԭ״̬
        //    Debug.Log("��ֹ�������ԭ״̬");
        //    ShowErrorMessageText("Cannot activate card in Left Panel when it is already in the Top Panel.");
        //    return;
        //}

        //// �����ƬҪ�����������ҵ�ǰ״̬��������
        //if (newState == CardState.InTopPanel && currentState == CardState.InLeftPanel)
        //{
        //    // ��ֹ�������ԭ״̬
        //    Debug.LogWarning("Cannot activate card in Top Panel when it is already in the Left Panel.");
        //    return;
        //}

        // ���¿�Ƭ��״̬
        currentState = newState;
        UpdateCardPosition();
    }

    private void UpdateCardPosition()
    {
        // ���ݿ�Ƭ��״̬����λ�û����
        switch (currentState)
        {
            case CardState.InLeftPanel:
                // ���¿�Ƭ��λ�õ�������
                transform.SetParent(GameObject.Find("TeamUnselectPanel").transform);
                break;
            case CardState.InTopPanel:
                // ���¿�Ƭ��λ�õ�������
                transform.SetParent(GameObject.Find("TopPanel").transform);
              
                break;
            //case CardState.InGamePanel:
            //    // ���¿�Ƭ��λ�õ���Ϸ�ڵı�����
            //    transform.SetParent(GameObject.Find("GamePanel").transform);
            //    break;
            case CardState.Inactive:
                // ���ػ���ÿ�Ƭ
                gameObject.SetActive(false);
                break;
        }
    }

    private void ShowErrorMessageText(string message)
    {
        // ��ʾ��ʾ��Ϣ
        if (ErrorMessageText != null)
        {
            ErrorMessageText.text = message;
            ErrorMessageText.gameObject.SetActive(true);

            // ��ѡ������һ����ʱ����������ʾ��Ϣ
            Invoke("HideErrorMessage", 2.0f); // 2���������Ϣ
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
