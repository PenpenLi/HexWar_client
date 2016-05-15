public class HeroSDS : CsvBase,IHeroSDS
{
	public int hp;
	public int attackTimes;
	public int damage;
	public int power;
	public int cost;
	public int heroType;
	public int[] skills;
	
	public HeroTypeClientSDS heroTypeSDS;
	
	public override void Fix()
	{
		heroTypeSDS = StaticData.GetData<HeroTypeClientSDS>(heroType);
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
	public int[] GetSkills()
	{
		return skills;
	}
}

