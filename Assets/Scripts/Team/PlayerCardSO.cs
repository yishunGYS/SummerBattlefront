using Gameplay.Player;
using Gameplay.Player.Solider.Attacker.Traverser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCardSO", menuName = "ScriptableObjects/TeamCard/PlayerCardSO")]

public class PlayerCardSO : ScriptableObject
{
    public GameObject playerPrefab;// ����ʿ����Ԥ����
    public Sprite cardImage;
    public string cardName;
    public int Cost;

    // ��̬��ȡԤ������Ϣ
    public void LoadPrefabData()
    {
        if (playerPrefab != null)
        {
            cardName = playerPrefab.name;

            var solider = playerPrefab.GetComponent<SoliderAgent>();
            if (solider != null)
            {
                Cost = solider.soliderModel.cost;
                Debug.Log("��ǰʿ����cost�� " + Cost);
            }
        }
    }
}
