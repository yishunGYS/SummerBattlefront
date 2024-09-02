using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using Systems.Level;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public class SelectLevel_UImanager : Singleton<SelectLevel_UImanager>
{
    public Transform levelBtns;
    
    public Transform levelInfoUI;
    public Transform levelSelectUI;

    public Text levelIntro;
    public Text levelName;
    public Transform enemies;
    public GameObject enemyPrefab;
    public Button enterLevelBtn;
    //根据当前通过关卡数,将部分关卡显示为灰,不可点击
    public void Awake()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 0);
        Button[] levelButtons = levelBtns.GetComponentsInChildren<Button>();
        
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > levelReached)
            {
                levelButtons[i].interactable = false;
            }
        }
        Debug.Log("selectLevel_UImanager Awake");
    }
    //关卡简介界面初始化,绑定点击事件
    public void Init(LevelInformationSo levelInfoSO)
    {
        //为开始关卡按钮增加点击事件
        enterLevelBtn.onClick.AddListener(LevelManager.Instance.EnterLevel);
        
        levelIntro.text = levelInfoSO.levelIntro;
        levelName.text = levelInfoSO.levelName;

        foreach (int enemyId in levelInfoSO.unlockEnemyId)
        {
            var enemyModel = DataManager.Instance.GetEnemyDataById(enemyId);
            GameObject go = Instantiate(enemyPrefab, enemies);
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(enemyModel.uiSpritePath);
            go.transform.Find("Des").GetComponent<TextMeshProUGUI>().text = enemyModel.enemyDes;
        }
        
    }
    //选择关卡按钮被按下
    public void SelectLevelBtnClick(LevelInformationSo levelInfoSO)
    {
        LevelManager.Instance.ShowLevelIntro(levelInfoSO);
        
        levelInfoUI.gameObject.SetActive(true);
        levelSelectUI.gameObject.SetActive(false);
    }
    //取消选关按钮被按下
    public void CancelSelectBtnClick()
    {
        levelInfoUI.gameObject.SetActive(false);
        levelSelectUI.gameObject.SetActive(true);

        LevelManager.Instance.nowLevelId = -1;
        LevelManager.Instance.nowLevelName = null;
        //LevelManager.Instance.nowLevelTime = -1;
			
        this.enterLevelBtn.onClick.RemoveListener(LevelManager.Instance.EnterLevel);

        for (int i = 0; i < enemies.childCount; ++i)
        {
            Destroy(enemies.GetChild(i).gameObject);
        }
    }


    public void UnlockAllLevel()
    {
        PlayerPrefs.SetInt("LevelReached", 28);
        
        var runtimeSoliderDict = DataManager.Instance.GetRuntimeSoliderModel();
        List<int> allSoliderIds = new List<int>() { 2, 4, 6, 1, 10 };
        foreach (var id in allSoliderIds)
        {
            runtimeSoliderDict.TryAdd(id,
                DataManager.Instance.GetSoliderDataById(id));
        }

        LevelManager.Instance.LevelFail();
    }
}
