using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public enum EduState
    {
        InTeam,
        InBattle
    }

    public class EduPanel : UIBasePanel
    {
        public TextMeshProUGUI eduText;
        public Button okButton;
        public int teamIndex;
        public int battleIndex;
        public EduState curState;

        private List<string> teamEduList = new List<string>()
        {
            "点击左边栏开始编队 \n (1/4)",
            "太棒啦！现在点击左边栏中的士兵，即可置入编队栏中 \n (2/4)",
            "点击编队栏中的士兵,即可取消编入 \n (3/4)",
            "点击开始战斗按钮，就能进入战斗啦 \n (4/4)"
        };


        private List<string> battleEduList = new List<string>()
        {
            "注意啦，每一关都有资源限制,请看左上角  (1/8)",
            "在资源下方是你的资源槽，资源槽的上限是10  (2/8)",
            "当资源槽不满的时候，每秒会恢复1点资源  (3/8)",
            "点击敌方单位，能够看到它的攻击范围   (4/8)",
            "派兵可以点击对应士兵 也可以用快捷键，例如现在你的中兵兵在派兵栏的第一个，那么选中它的快捷键就是'1' (5/8)",
            "派出士兵需要消耗其对应的资源数 (6/8)",
            "现在点击地图上的高亮地块，即可派出士兵啦  (7/8)",
            "对了，重开快捷键是'R'，跳过关卡的快捷键是'P'  (8/8)"
        };


        public override void OpenPanel()
        {
            base.OpenPanel();
            if (curState == EduState.InTeam)
            {
                okButton.gameObject.SetActive(false);
            }
            else if (curState == EduState.InBattle)
            {
                okButton.gameObject.SetActive(true);
            }

            OnChangeText();
        }

        public void OnChangeText()
        {
            if (curState == EduState.InTeam)
            {
                eduText.text = teamEduList[teamIndex];
            }
            else if (curState == EduState.InBattle)
            {
                eduText.text = battleEduList[battleIndex];
            }
        }


        public void OnDoRightThing()
        {
            if (curState == EduState.InTeam)
            {
                teamIndex++;
                print(teamIndex + "Index");
                if (teamIndex >= teamEduList.Count)
                {
                    UIManager.Instance.OnCloseEduPanel();
                    return;
                }
            }
            else if (curState >= EduState.InBattle - 1)
            {
                battleIndex++;
                if (battleIndex == 5 ||battleIndex == 7)
                {
                    okButton.gameObject.SetActive(true);
                }
                if (battleIndex >= battleEduList.Count)
                {
                    UIManager.Instance.OnCloseEduPanel();
                    return;
                }
            }

            OnChangeText();
        }

        public void OnClickOkButton()
        {
            if (curState == EduState.InBattle)
            {
                battleIndex++;
                if (battleIndex == 4 || battleIndex == 6)
                {
                    okButton.gameObject.SetActive(false);
                }
                
                if (battleIndex >= battleEduList.Count)
                {
                    UIManager.Instance.OnCloseEduPanel();
                    return;
                }
            }

            OnChangeText();
        }
    }
}