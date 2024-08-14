using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public PlayerCardSO playerCardSO;

    // Start is called before the first frame update
    void Start()
    {
        if (playerCardSO != null)
        {
            playerCardSO.LoadPrefabData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
