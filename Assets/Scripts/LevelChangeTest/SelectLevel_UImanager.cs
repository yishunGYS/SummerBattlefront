using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using Systems.Level;
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
    }
    //关卡简介界面初始化,绑定点击事件
    public void Init(LevelInformationSO levelInfoSO)
    {
        //为开始关卡按钮增加点击事件
        enterLevelBtn.onClick.AddListener(LevelManager.Instance.EnterLevel);
        
        levelIntro.text = levelInfoSO.levelIntro;
        levelName.text = levelInfoSO.levelName;

        foreach (Image enemy in levelInfoSO.enemies)
        {
            Instantiate(enemy, enemies);
        }
    }
    //选择关卡按钮被按下
    public void SelectLevelBtnClick(LevelInformationSO levelInfoSO)
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
			
        this.enterLevelBtn.onClick.RemoveListener(LevelManager.Instance.EnterLevel);

        for (int i = 0; i < enemies.childCount; ++i)
        {
            Destroy(enemies.GetChild(i).gameObject);
        }
    }
}
