using UnityEngine;
using System.Collections.Generic;

public static class SkillManager
{
	
	private static SkillModel[] skill = new SkillModel[3];



	/// <summary>
	/// Load skill list from parsed CSV
	/// </summary>
	/// <returns>The skill list.</returns>
	public static List<SkillModel>  GetCSVSkillList ()
	{
		List<Dictionary<string,System.Object>>  csvSkillList = CSVParser.ParseCSV ("Skills");
		List<SkillModel> skillList = new List<SkillModel> ();
		//count -1 because it counts also the header, we need not to count it
		for (int i = 0; i < csvSkillList.Count - 1; i++) {
			string skillName = csvSkillList [i] ["SkillName"].ToString ();
			string skillDescription = csvSkillList [i] ["SkillDescription"].ToString ();
			int skillGpCost = int.Parse (csvSkillList [i] ["SkillGPCost"].ToString ());
			string skillParam = csvSkillList [i] ["SkillParam"].ToString ();

			skillList.Add (new SkillModel (skillName, skillGpCost, skillDescription, skillParam));
		}
		return skillList;
	}

	//TEST ONLY!!!!!
	public static List<SkillModel>  GetEquipSkillList ()
	{
		List<SkillModel> equipSkillList = new List<SkillModel> (8);
		List<SkillModel> skillList = GetCSVSkillList ();
		equipSkillList.Add (skillList [0]);
		equipSkillList.Add (skillList [1]);
		equipSkillList.Add (skillList [2]);
		equipSkillList.Add (skillList [3]);
		equipSkillList.Add (skillList [4]);
		equipSkillList.Add (skillList [5]);
		equipSkillList.Add (skillList [6]);
		equipSkillList.Add (skillList [7]);
		return equipSkillList;
	}



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
	/// Set the skill to placeholder UI in Battle
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
