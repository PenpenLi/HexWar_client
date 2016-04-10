public class HeroTypeSDS : CsvBase,IHeroTypeSDS
{
    public bool canCharge;
    public bool canMove;
    public bool canAttack;
    public int minRange;
    public int maxRange;

    public bool GetCanCharge()
    {
        return canCharge;
    }
    public bool GetCanMove()
    {
        return canMove;
    }
    public bool GetCanAttack()
    {
        return canAttack;
    }
    public int GetMinRange()
    {
        return minRange;
    }
    public int GetMaxRange()
    {
        return maxRange;
    }
}

