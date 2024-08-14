using Gameplay.Player;
using Gameplay.Player.Solider.Attacker.Traverser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCardSO", menuName = "ScriptableObjects/TeamCard/PlayerCardSO")]

public class PlayerCardSO : ScriptableObject
{
    public GameObject playerPrefab;// 引用士兵的预制体
    public Sprite cardImage;
    public string cardName;
    public int Cost;

    // 动态读取预制体信息
    public void LoadPrefabData()
    {
        if (playerPrefab != null)
        {
            cardName = playerPrefab.name;

            var solider = playerPrefab.GetComponent<SoliderAgent>();
            if (solider != null)
            {
                Cost = solider.soliderModel.cost;
                Debug.Log("当前士兵的cost是 " + Cost);
            }
        }
    }
}
