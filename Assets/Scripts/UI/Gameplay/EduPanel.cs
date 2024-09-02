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
            "����������ʼ��� \n (1/4)",
            "̫���������ڵ��������е�ʿ������������������ \n (2/4)",
            "���������е�ʿ��,����ȡ������ \n (3/4)",
            "�����ʼս����ť�����ܽ���ս���� \n (4/4)"
        };


        private List<string> battleEduList = new List<string>()
        {
            "ע������ÿһ�ض�����Դ����,�뿴���Ͻ�  (1/8)",
            "����Դ�·��������Դ�ۣ���Դ�۵�������10  (2/8)",
            "����Դ�۲�����ʱ��ÿ���ָ�1����Դ  (3/8)",
            "����з���λ���ܹ��������Ĺ�����Χ   (4/8)",
            "�ɱ����Ե����Ӧʿ�� Ҳ�����ÿ�ݼ���������������б������ɱ����ĵ�һ������ôѡ�����Ŀ�ݼ�����'1' (5/8)",
            "�ɳ�ʿ����Ҫ�������Ӧ����Դ�� (6/8)",
            "���ڵ����ͼ�ϵĸ����ؿ飬�����ɳ�ʿ����  (7/8)",
            "���ˣ��ؿ���ݼ���'R'�������ؿ��Ŀ�ݼ���'P'  (8/8)"
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