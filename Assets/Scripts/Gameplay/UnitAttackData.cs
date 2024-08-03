namespace Gameplay
{
    public class UnitAttackData
    {
        public int attackPoint;
        public int magicAttackPoint;

        public UnitAttackData(int attackPoint, int magicAttackPoint)
        {
            this.attackPoint = attackPoint;
            this.magicAttackPoint = magicAttackPoint;
        }
    }

    public class UnitDefendData
    {
        public float defendReducePercent;
        public float magicDefendReducePercent;

        public UnitDefendData(float defendReducePercent, float magicDefendReducePercent)
        {
            this.defendReducePercent = defendReducePercent;
            this.magicDefendReducePercent = magicDefendReducePercent;
        }
    }
}