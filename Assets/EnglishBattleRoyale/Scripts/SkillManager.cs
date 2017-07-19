using UnityEngine;
using System.Collections.Generic;

public static class SkillManager
{
	
	private static SkillModel[] skill = new SkillModel[3];

	public static List<SkillModel> skillList = new List<SkillModel> ();

	private static List<Dictionary<string,System.Object>> csvSkillList;

	static void Start ()
	{
		csvSkillList = CSVParser.ParseCSV ("Skills");
		for (int i = 0; i < csvSkillList.Count; i++) {
			string skillName = csvSkillList [i] ["SkillName"].ToString ();
			string skillDescription = csvSkillList [i] ["SkillDescription"].ToString ();
			int skillGpCost = int.Parse (csvSkillList [i] ["SkillGPCost"].ToString ());
			string skillParam = csvSkillList [i] ["SkillParam"].ToString ();

			skillList.Add (new SkillModel (skillName, skillGpCost, skillDescription, skillParam));
		}

		SetSkill (0, skillList [0]);
		SetSkill (1, skillList [1]);
		SetSkill (2, skillList [2]);
	}

	/// <summary>
	/// Activates the skills.
	/// </summary>
	public static void ActivateSkill (int skillNumber)
	{
		StartSkill (skill [skillNumber - 1]);
	}

	public static void StartSkill (SkillModel skillmodel)
	{
		ScreenBattleController.Instance.PlayerGP -= skillmodel.skillGpCost;
		SystemFirebaseDBController.Instance.SetSkillParam (skillmodel);
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
			SystemFirebaseDBController.Instance.SkillPhase ();
		} 
	}

	/// <summary>
	/// Set the skill to placeholder UI
	/// </summary>
	/// <param name="skill1">Skill1.</param>

	public static void SetSkill (int skillIndex, SkillModel skillmodel)
	{
		skill [skillIndex] = skillmodel;
		PartBattleUIController.Instance.SetSkillUI (skillIndex + 1, skillmodel.skillName, skillmodel.skillGpCost);
	}

	public static SkillModel GetSkill (int skillNumber)
	{
		return skill [skillNumber - 1];
	}


}
