using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace Systems.Level
{
    public class LevelManager : Singleton<LevelManager>
    {
        public int levelReached = 0; // 通过的关卡数
        public int maxLevelId = 0; // 通过的最大关卡Id数

        public int nowLevelId;
        public string nowLevelName;
        //public float nowLevelTime;

        public int nowLevelResource;
        
        public List<int> nowUnlockSoliderIds = new List<int>();
        public List<int> nowLockedSoliderIds = new List<int>();
        public SceneFader fader;

        private void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);

            PlayerPrefs.SetInt("LevelReached", 0); // 测试用

            fader.FadeTo("LevelSelect");
        }

        // 开始关卡按钮被按下，切换场景
        public void EnterLevel()
        {
            if (nowLevelName == null)
            {
                return;
            }

            // 移除按钮监听器并切换场景
            SelectLevel_UImanager.Instance.enterLevelBtn.onClick.RemoveListener(this.EnterLevel);
            fader.ChangeScene(nowLevelName);
        }

        // 展示当前所选关卡的介绍，记录当前选择的关卡id与关卡名（感觉不应该放这，之后再改）
        public void ShowLevelIntro(LevelInformationSo levelInfo)
        {
            // 出现关卡简介页面UI，将Info中的信息展示上去
            SelectLevel_UImanager.Instance.Init(levelInfo);

            nowLevelId = levelInfo.levelID;
            nowLevelName = levelInfo.levelName;
            //nowLevelTime = levelInfo.levelTime;
            nowLevelResource = levelInfo.levelResource;
            nowUnlockSoliderIds = levelInfo.unlockSoliderId;
            nowLockedSoliderIds = levelInfo.lockSoliderIds;
        }

        // 关卡通过，切换场景
        public void LevelEnd()
        {
            if (nowLevelId > maxLevelId)
            {
                levelReached++;
                maxLevelId = nowLevelId;
                PlayerPrefs.SetInt("LevelReached", levelReached);
                Debug.Log(PlayerPrefs.GetInt("LevelReached"));
            }

            //解锁兵种
            var runtimeSoliderDict = DataManager.Instance.GetRuntimeSoliderModel();
            foreach (var id in nowUnlockSoliderIds)
            {
                runtimeSoliderDict.TryAdd(id,
                    DataManager.Instance.GetSoliderDataById(id));
            }
            //锁上兵种
            foreach (var id in nowLockedSoliderIds)
            {
                if (runtimeSoliderDict.ContainsKey(id))
                {
                    runtimeSoliderDict.Remove(id);
                }
            }

            fader.ChangeScene("LevelSelect");
        }

        public void LevelFail()
        {
            fader.ChangeScene("LevelSelect");
        }

        // public float GetCurrentLevelTime()
        // {
        //     return nowLevelTime;
        // }

        public int GetCurrentLevelResource()
        {
            return nowLevelResource;
        }
    }
}