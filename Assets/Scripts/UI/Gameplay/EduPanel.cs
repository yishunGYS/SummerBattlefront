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
            "注意啦，每一关都有资源限制,请看右下角  (1/5)",
            "在下方是你的资源槽，资源槽的上限是10  (2/5)",
            "到资源槽不满的时候，每秒会恢复1点资源  (3/5)",
            "下方是你的派兵栏，请选择你要派出的士兵  (4/5)",
            "好棒啊！现在点击地图上的高亮地块，即可派出士兵啦  (5/5)"
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
                if (battleIndex == 3 )
                {
                    okButton.gameObject.SetActive(false);
                }
                if (battleIndex == battleEduList.Count)
                {
                    UIManager.Instance.OnCloseEduPanel();
                }
            }

            OnChangeText();
        }
    }
}