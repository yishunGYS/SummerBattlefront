//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CardManager : MonoBehaviour
//{
//    public List<CardData> availableCards;  // ���п��ÿ��Ƶ������б�
//    public Transform cardPanel;  // ���ÿ���UI�����
//    public GameObject cardPrefab;  // ����Ԥ����
//    public Transform queuePanel;  // ������ѡ���Ƶ����

//    private List<CardData> selectedCards = new List<CardData>();  // ��ѡ��Ŀ��ƶ���

//    void Start()
//    {
//        PopulateCardPanel();
//    }

//    void PopulateCardPanel()
//    {
//        foreach (var card in availableCards)
//        {
//            GameObject cardGO = Instantiate(cardPrefab, cardPanel);
//            cardGO.GetComponent<Image>().sprite = card.cardImage;
//            cardGO.transform.Find("SunCostText").GetComponent<Text>().text = card.sunCost.ToString();
//            cardGO.GetComponent<Button>().onClick.AddListener(() => OnCardSelected(card));
//        }
//    }

//    void OnCardSelected(CardData card)
//    {
//        if (selectedCards.Contains(card))
//        {
//            // ��������Ѿ�ѡ�У���Ӷ������Ƴ�
//            selectedCards.Remove(card);
//            // ����UI���Ƴ����ƴӶ�����
//        }
//        else
//        {
//            // ���û��ѡ�У���ӵ�����
//            selectedCards.Add(card);
//            // ����UI����������ӵ�������
//            GameObject queueCard = Instantiate(cardPrefab, queuePanel);
//            queueCard.GetComponent<Image>().sprite = card.cardImage;
//            queueCard.transform.Find("SunCostText").GetComponent<Text>().text = card.sunCost.ToString();
//            // ���������Ƴ����Ƶİ�ť���߼�
//        }
//    }

//    public void OnConfirmSelection()
//    {
//        // ��ѡ��Ŀ���Ӧ�õ���Ϸ�߼��У����紫�ݸ���һ��������ս��ϵͳ
//    }
//}

