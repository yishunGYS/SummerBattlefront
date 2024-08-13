using _3DlevelEditor_GYS;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Features
{
    public class TraverserFeature : MonoBehaviour
    {
        [HideInInspector] public SoliderAgent agent;
        //[HideInInspector] 
        public bool canTraver = true; //��ǰ�Ƿ���Դ�Խ����
        //[HideInInspector] 
        public bool isTraver; //��ǰ�Ƿ��ڴ�Խ״̬-��Խ״̬���ɽ��й���/���ɱ��赲-����Ŀ�����/����һ����ɫ�赲ʱ�˳���Խ״̬
        [HideInInspector] public GridCell targetCell; //��Ҫ��Խ���ĸ���
        [HideInInspector] public float preSpeed; //���봩Խ״̬ǰ���ٶ�
        //[HideInInspector] 
        public EnemyAgent preEnemy; //������Խ״̬�ĵз�ʿ��

        [Header("��Խ�����ٶ�")] public float traverSpeed;

        private void Awake()
        {
            agent = transform.GetComponent<SoliderAgent>();
            canTraver = true;
            isTraver = false;
        }
    }
}


