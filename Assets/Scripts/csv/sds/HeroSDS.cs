public class HeroSDS : CsvBase,IHeroSDS
{
    public int hp;
    public int attackTimes;
    public int damage;
    public int power;
    public int cost;
    public int heroType;

    private HeroTypeSDS heroTypeSDS;

    public override void Fix()
    {
        heroTypeSDS = StaticData.GetData<HeroTypeSDS>(heroType);
    }

    public int GetHp()
    {
        return hp;
    }
    public int GetAttackTimes()
    {
        return attackTimes;
    }
    public int GetDamage()
    {
        return damage;
    }
    public int GetPower()
    {
        return power;
    }
    public int GetCost()
    {
        return cost;
    }
    public int GetHeroType()
    {
        return heroType;
    }
    public IHeroTypeSDS GetHeroTypeSDS()
    {
        return heroTypeSDS;
    }
}

