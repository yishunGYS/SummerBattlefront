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

        //编队教学
        public async void OnTeachClickTeamAssemble()
        {
            await UIManager.Instance.OnShowTipPanel("点击左边栏开始编队  (1/1)",2500);
        }
        
        
        public async void OnTeachTeamAssemble()
        {
            await UIManager.Instance.OnShowTipPanel("太棒啦！现在点击左边栏中的士兵，即可置入编队栏中  (1/1)",5000);
        } 
        
        public async void OnTeachTeamCancelAssemble()
        {
            await UIManager.Instance.OnShowTipPanel("点击编队栏中的士兵,即可取消编入  (1/1)",4000);
        } 
        
        //战斗内教学
        public async void OnTeachResourceLimit()
        {
            await UIManager.Instance.OnShowTipPanel("注意啦，每一关都有资源限制,请看右下角  (1/4)",4000);

            await Task.Delay(1500);
            OnTeachResourceRecover();
        }
    
        public async void OnTeachResourceRecover()
        {
            await UIManager.Instance.OnShowTipPanel("在下方是你的资源槽，资源槽的上限是10  (2/4)",4000);
            await Task.Delay(1500);
            await UIManager.Instance.OnShowTipPanel("到资源槽不满的时候，每秒会恢复1点资源  (3/4)",4000);
            await Task.Delay(1500);
            OnTeachSelect();
        }
        
        public async void OnTeachSelect()
        {
            await UIManager.Instance.OnShowTipPanel("下方是你的派兵栏，请选择你要派出的士兵  (4/4)",4000);
            isInEdu = false;
        }
    
        public async void OnTeachPlace()
        {
            await UIManager.Instance.OnShowTipPanel("好棒啊！现在点击地图上的高亮地块，即可派出士兵啦  (1/1)",5000);
        }
    }
}
