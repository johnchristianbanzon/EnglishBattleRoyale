using System.Collections.Generic;

public class SkillModel{

	public bool userHome;
	public ParamNames skillName;
	public int skillGpCost;
	public string skillDescription;
	public string skillParam;

	public SkillModel(){
	}

	public SkillModel(ParamNames skillName, int skillGpCost, string skillDescription, string skillParam){
		this.skillName = skillName;
		this.skillGpCost = skillGpCost;
		this.skillDescription = skillDescription;
		this.skillParam = skillParam;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();

		result ["SkillName"] = skillName.ToString();
		result ["SkillGPCost"] = skillGpCost;
		result ["SkillDescription"] = skillDescription;
		result ["SkillParam"] = skillParam;

		return result;
	}

}
