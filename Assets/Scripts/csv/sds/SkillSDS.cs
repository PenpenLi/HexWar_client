public class SkillSDS : CsvBase, ISkillSDS
{
	public int eventName;
	public int addType;
	public int conditionType;
	public int conditionData;
	public int targetType;
	public int targetNum;
	public int effectType;
	public int[] effectData;
	
	public SkillEventName GetEventName()
	{
		return (SkillEventName)eventName;
	}
	public SkillAddType GetAddType()
	{
		return (SkillAddType)addType;
	}
	public SkillConditionType GetConditionType()
	{
		return (SkillConditionType)conditionType;
	}
	public int GetConditionData()
	{
		return conditionData;
	}
	public SkillTargetType GetTargetType()
	{
		return (SkillTargetType)targetType;
	}
	public int GetTargetNum()
	{
		return targetNum;
	}
	public SkillEffectType GetEffectType()
	{
		return (SkillEffectType)effectType;
	}
	public int[] GetEffectData()
	{
		return effectData;
	}
}

