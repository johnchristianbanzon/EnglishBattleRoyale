using UnityEngine;
using System.Collections.Generic;

public static class SkillManager
{
	
	private static SkillModel[] skill = new SkillModel[3];
	private static Queue<SkillModel> skillQueue = new Queue<SkillModel> (8);

	/// <summary>
	/// Load skill list from parsed CSV
	/// </summary>
	/// <returns>The skill list.</returns>
	public static List<SkillModel>  GetCSVSkillList ()
	{

		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("Skills");
		List<List<string>> csvSkillList = CSVParser.Parse (csvData.ToString ());

		List<SkillModel> skillList = new List<SkillModel> ();
		for (int i = 1; i < csvSkillList.Count; i++) {
			string skillName = CSVParser.GetValueArrayFromKey(csvSkillList, "SkillName")[i].ToString ();
			string skillDescription = CSVParser.GetValueArrayFromKey(csvSkillList, "SkillDescription")[i].ToString ();
			int skillGpCost = int.Parse(CSVParser.GetValueArrayFromKey(csvSkillList, "SkillGPCost")[i].ToString ());
			string skillParam = CSVParser.GetValueArrayFromKey(csvSkillList, "SkillParam")[i].ToString ();
			skillList.Add (new SkillModel(skillName,skillGpCost,skillDescription,skillParam));
		}

		return skillList;
	}

	//TEST ONLY FOR NOW!!!!!
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
		ScreenBattleController.Instance.partState.PlayerGP -= skillmodel.skillGpCost;
		SystemFirebaseDBController.Instance.SetSkillParam (skillmodel);
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
			SystemFirebaseDBController.Instance.SkillPhase ();
		} 
	}
		
	//set the skill in the UI
	public static void SetSkillUI (int skillIndex, SkillModel skillmodel)
	{
		skill [skillIndex] = skillmodel;
		ScreenBattleController.Instance.partSkill.SetSkillUI (skillIndex, skillmodel);
	}

	//receive skill list from prepare phase and shuffle for random skill in start and put in queue
	public static void SetSkillEnqueue (List<SkillModel> skillList)
	{
		skillQueue.Clear ();
		skillList.Shuffle ();

		for (int i = 0; i < skillList.Count; i++) {
			Debug.Log (skillList [i].skillName);
			skillQueue.Enqueue (skillList [i]);
		}
	}

	//Fisher-Yates shuffle
	private static System.Random rng = new System.Random();  
	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}


	//Default 3 starting skills when starting the game
	public static void SetStartSkills ()
	{
		for (int i = 0; i < 3; i++) {
			SetSkillUI (i, skillQueue.Dequeue ());
		}
	}

	//When skill is used, remove previous skill and enqueue replace with new skill in queue
	public static void UseSkillUI (int skillIndex)
	{
		//remove this if you want skill will be gone after use
		skillQueue.Enqueue (skill [skillIndex]);

		SetSkillUI (skillIndex, skillQueue.Dequeue ());
	}

	public static SkillModel GetSkill (int skillNumber)
	{
		return skill [skillNumber - 1];
	}
}
