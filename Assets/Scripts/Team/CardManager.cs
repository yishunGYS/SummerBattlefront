//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CardManager : MonoBehaviour
//{
//    public List<CardData> availableCards;  // 所有可用卡牌的数据列表
//    public Transform cardPanel;  // 放置卡牌UI的面板
//    public GameObject cardPrefab;  // 卡牌预制体
//    public Transform queuePanel;  // 放置已选卡牌的面板

//    private List<CardData> selectedCards = new List<CardData>();  // 已选择的卡牌队列

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
//            // 如果卡牌已经选中，则从队列中移除
//            selectedCards.Remove(card);
//            // 更新UI，移除卡牌从队列中
//        }
//        else
//        {
//            // 如果没有选中，添加到队列
//            selectedCards.Add(card);
//            // 更新UI，将卡牌添加到队列中
//            GameObject queueCard = Instantiate(cardPrefab, queuePanel);
//            queueCard.GetComponent<Image>().sprite = card.cardImage;
//            queueCard.transform.Find("SunCostText").GetComponent<Text>().text = card.sunCost.ToString();
//            // 你可以添加移除卡牌的按钮和逻辑
//        }
//    }

//    public void OnConfirmSelection()
//    {
//        // 将选择的卡牌应用到游戏逻辑中，例如传递给下一个场景或战斗系统
//    }
//}

