using System;
using System.Collections.Generic;

[Serializable]
public class SkillParameter
{
	public string skillKey;

	public int skillValue;

	public SkillParameter (string skillKey, int skillValue)
	{
		this.skillKey = skillKey;
		this.skillValue = skillValue;
	}


}

[Serializable]
public class SkillParameterList
{
	public List<SkillParameter> skillList;
}
