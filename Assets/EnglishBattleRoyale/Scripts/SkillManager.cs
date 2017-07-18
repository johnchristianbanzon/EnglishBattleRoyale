using UnityEngine;
using System.Collections.Generic;

public static class SkillManager
{
	
	private static SkillModel[] skill = new SkillModel[3];

	public static List<SkillModel> skillList = new List<SkillModel> ();

	//TESTING ONLY!!!!
	static void Start ()
	{
		//test skill 1
		List<SkillParameter> skillData1 = new List<SkillParameter> ();
		skillData1.Add (new SkillParameter (ParamNames.Damage.ToString(), 10));
		SkillParameterList splist1 = new SkillParameterList ();
		splist1.skillList = skillData1;
		string skillParam1 = JsonUtility.ToJson (splist1);
		skillList.Add (new SkillModel (ParamNames.BicPunch, 3, "Deals a straight blow to opponent's guts!", skillParam1));
	
		//test skill 2
		List<SkillParameter> skillData2 = new List<SkillParameter> ();
		skillData2.Add (new SkillParameter (ParamNames.Damage.ToString(), 15));
		skillData2.Add (new SkillParameter (ParamNames.Recover.ToString(), 10));
		SkillParameterList splist2 = new SkillParameterList ();
		splist2.skillList = skillData2;
		string skillParam2 = JsonUtility.ToJson (splist2);
		skillList.Add (new SkillModel (ParamNames.Sunder, 9, "Deals a considerable amount of damage while absorbing life points at the same time.", skillParam2));

		//test skill 3
		List<SkillParameter> skillData3 = new List<SkillParameter> ();
		skillData3.Add (new SkillParameter (ParamNames.Recover.ToString(), 10));
		SkillParameterList splist3 = new SkillParameterList ();
		splist3.skillList = skillData3;
		string skillParam3 = JsonUtility.ToJson (splist3);
		skillList.Add (new SkillModel (ParamNames.Rejuvination, 4, "Regenerates HP which is highly affected by number of correct answers", skillParam3));

	//------------

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
		BattleController.Instance.PlayerGP -= skillmodel.skillGpCost;
		SystemFirebaseDBController.Instance.SetSkillParam (skillmodel);
		if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
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
		BattleController.Instance.SetSkillUI (skillIndex + 1, skillmodel.skillName, skillmodel.skillGpCost);
	}

	public static SkillModel GetSkill (int skillNumber)
	{
		return skill [skillNumber -1];
	}


}
