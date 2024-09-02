using System.Collections.Generic;
using Managers;
using Systems.Edu;
using UI.Gameplay;
using UnityEngine;
using Utilities;

namespace Systems.ShortCut
{
    public class ShortcutSystem : Singleton<ShortcutSystem>
    {
        private SpawnSoliderPanel spawnSoliderPanel;
        private List<KeyCode> shortcuts = new List<KeyCode>(){KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8};

        private bool isEdu;
        private void Update()
        {
            if (spawnSoliderPanel == null)
            {
                spawnSoliderPanel = FindObjectOfType<SpawnSoliderPanel>();
            }
            
            for (int i = 0; i < shortcuts.Count; i++)
            {
                if (Input.GetKeyDown(shortcuts[i]))
                {
                    var soliderInBattle = DataManager.Instance.GetSolidersInBattle();
                    var soliderId = soliderInBattle[i];
                    SpawnManager.Instance.ChangeSelectSolider(soliderId);
                    spawnSoliderPanel.OnSelectCard(spawnSoliderPanel.soliderUILists[i].view);
                    
                    if (FindObjectOfType<EduSystem>() && !spawnSoliderPanel.isPlaceEdued)
                    {
                        UIManager.Instance.OnChangeEduPanelText();
                        spawnSoliderPanel.isPlaceEdued = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.OpenLevelFailPanel();
            }
        }
    }
}
