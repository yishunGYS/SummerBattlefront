using UnityEngine;

namespace Systems.Buff
{
    public class BuffInstance
    {
        public int tick;
        public int stack;

        public float tickTime;
        public float tickTimer;
        public float elapsedTimer;
        public float durationTime;
        public float durationTimer;

        public BuffModel buffInfo;
        public GameObject caster = null;
        public GameObject carrier;

        private BuffManager _buffManager;

        public BuffInstance(BuffModel buffInfo, BuffManager buffManager)
        {
            this.buffInfo = buffInfo;
            _buffManager = buffManager;
            tick = 0;


            switch (buffInfo.buff触发类型)
            {
                case BuffType.单次:
                    break;

                case BuffType.持续一段时间:
                    durationTimer = buffInfo.durationTime;
                    break;
                case BuffType.持续每秒生效:
                    tickTime = buffInfo.tickTime;
                    durationTime =buffInfo.durationTime;

                    tickTimer = tickTime;
                    durationTimer = durationTime;
                    break;
            }

            this.buffInfo.在创建时?.Invoke();
        }

        public void OnBuffUpdate()
        {
            switch (buffInfo.buff触发类型)
            {
                case BuffType.单次:
                    break;
                case BuffType.持续一段时间:
                    PersistBuffUpdate();
                    break;
                case BuffType.持续每秒生效:
                    DotBuffUpdate();
                    break;
            }
        }

        private void PersistBuffUpdate()
        {
            durationTimer -= Time.deltaTime;
            CheckDOTBuffEnd();
        }

        private void DotBuffUpdate()
        {
            tickTimer -= Time.deltaTime;
            durationTimer -= Time.deltaTime;
            if (tickTimer < 0.01)
            {
                tick++;
                tickTimer = tickTime;
                buffInfo.在Tick触发时?.Invoke();
                CheckDOTBuffEnd();
            }
        }

        public float CalculateDamage()
        {
            return 0;
        }

        public void ModifyStack(int m)
        {
            stack += m;
            buffInfo.在Stack触发时?.Invoke();
            CheckStackBuffEnd();
        }

        public void HitBuffUpdate()
        {
            buffInfo.在Hit触发时?.Invoke();
        }

        private void CheckDOTBuffEnd()
        {
            if (durationTimer < 0.01)
                _buffManager.RemoveBuff(this);
        }

        private void CheckStackBuffEnd()
        {
            if (stack <= 0)
                _buffManager.RemoveBuff(this);
        }
    }
}