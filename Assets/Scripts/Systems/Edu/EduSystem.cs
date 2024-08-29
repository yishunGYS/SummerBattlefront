using System;
using System.Threading.Tasks;
using Managers;
using UnityEngine;
using Utilities;

namespace Systems.Edu
{
    public class EduSystem : MonoBehaviour
    {
        public bool isInEdu;
        public static EduSystem Instance;
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        //��ӽ�ѧ
        public async void OnTeachClickTeamAssemble()
        {
            await UIManager.Instance.OnShowTipPanel("����������ʼ���  (1/1)",2500);
        }
        
        
        public async void OnTeachTeamAssemble()
        {
            await UIManager.Instance.OnShowTipPanel("̫���������ڵ��������е�ʿ������������������  (1/1)",5000);
        } 
        
        public async void OnTeachTeamCancelAssemble()
        {
            await UIManager.Instance.OnShowTipPanel("���������е�ʿ��,����ȡ������  (1/1)",4000);
        } 
        
        //ս���ڽ�ѧ
        public async void OnTeachResourceLimit()
        {
            await UIManager.Instance.OnShowTipPanel("ע������ÿһ�ض�����Դ����,�뿴���½�  (1/4)",4000);

            await Task.Delay(1500);
            OnTeachResourceRecover();
        }
    
        public async void OnTeachResourceRecover()
        {
            await UIManager.Instance.OnShowTipPanel("���·��������Դ�ۣ���Դ�۵�������10  (2/4)",4000);
            await Task.Delay(1500);
            await UIManager.Instance.OnShowTipPanel("����Դ�۲�����ʱ��ÿ���ָ�1����Դ  (3/4)",4000);
            await Task.Delay(1500);
            OnTeachSelect();
        }
        
        public async void OnTeachSelect()
        {
            await UIManager.Instance.OnShowTipPanel("�·�������ɱ�������ѡ����Ҫ�ɳ���ʿ��  (4/4)",4000);
            isInEdu = false;
        }
    
        public async void OnTeachPlace()
        {
            await UIManager.Instance.OnShowTipPanel("�ð��������ڵ����ͼ�ϵĸ����ؿ飬�����ɳ�ʿ����  (1/1)",5000);
        }
    }
}
