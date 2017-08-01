using UnityEngine;
using System.Collections.Generic;
using NCalc;

public class CharacterManager: IRPCDicObserver
{
	private static CharacterModel character;
	private static float calculateValue;
	private static CharacterModel[] currentCharacterInEquip = new CharacterModel[3];
	private static Queue<CharacterModel> characterQueue = new Queue<CharacterModel> (8);

	#region Receive character from firebase and activate
	void Start ()
	{
		RPCDicObserver.AddObserver (this);
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey ("param")) {

				bool userHome = (bool)rpcReceive ["userHome"];
				SystemGlobalDataController.Instance.isSender = userHome;

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
				if (param.ContainsKey ("CharacterRPC")) {

					character = (CharacterModel)JsonConverter.StringToObject (param ["CharacterRPC"].ToString ());
					Calculate();
				}

			}
		} catch (System.Exception e) {
			//do something with exception in future
		}
	}
		

	//calculate the skill effect of character from csv
	private static void Calculate ()
	{
		float variable = 0;
		switch (character.characterAmountVariable) {
		case "none":
			variable = 0;
			break;
		case "enemyHP":
			variable = ScreenBattleController.Instance.partState.enemy.playerHP;
			break;
		case "playerHP":
			variable = ScreenBattleController.Instance.partState.player.playerHP;
			break;
		case "enemyDamage":
			variable = ScreenBattleController.Instance.partState.enemy.playerBaseDamage;
			break;
		case "playerDamage":
			variable = ScreenBattleController.Instance.partState.player.playerBaseDamage;
			break;
		case "correctAnswer":
			break;
		}

		//parses the string formula from csv
		Expression e = new Expression (character.characterAmount);
		e.Parameters ["N"] = variable;  
		calculateValue = float.Parse (e.Evaluate ().ToString ());
	}


	private static void Activate ()
	{
		SkillCalcEnum skillCalc = (SkillCalcEnum)character.characterCalculationType;

		switch ((SkillEnum)character.characterSkillID) {
		case SkillEnum.DecreaseEnemyBaseDamage:
			break;
		case SkillEnum.DereaseEnemyHP:
			break;
		case SkillEnum.IncreasePlayerBaseDamage:
			break;
		case SkillEnum.IncreasePlayerGP:
			break;
		case SkillEnum.IncreasePlayerHP:
			break;
		case SkillEnum.LimitEnemySkill:
			break;
		case SkillEnum.Stop:
			break;
		}
	}

	#endregion


	#region get character list and assign to equip

	/// <summary>
	/// Load skill list from parsed CSV
	/// </summary>
	/// <returns>The skill list.</returns>
	public static List<CharacterModel>  GetCharacterList ()
	{
		List<CharacterModel> characterList = MyConst.GetCharacterList();

		return characterList;
	}

	//TEST ONLY FOR NOW!!!!! Add 8 characters to equip
	public static List<CharacterModel>  GetEquipSkillList ()
	{
		List<CharacterModel> equipCharacterList = new List<CharacterModel> (8);
		List<CharacterModel> characterList = GetCharacterList ();
		equipCharacterList.Add (characterList [0]);
		equipCharacterList.Add (characterList [1]);
		equipCharacterList.Add (characterList [2]);
		equipCharacterList.Add (characterList [3]);
		equipCharacterList.Add (characterList [4]);
		equipCharacterList.Add (characterList [5]);
		equipCharacterList.Add (characterList [6]);
		equipCharacterList.Add (characterList [7]);
		return equipCharacterList;
	}

	public static void ActivateCharacter (int characterNumber)
	{
		StartCharacter (currentCharacterInEquip [characterNumber - 1]);
	}

	public static void StartCharacter (CharacterModel characterModel)
	{
		ScreenBattleController.Instance.partState.player.playerGP -= characterModel.characterGPCost;
		SystemFirebaseDBController.Instance.SetParam("CharacterRPC", (characterModel));
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
			SystemFirebaseDBController.Instance.SkillPhase ();
		} 
	}

	//set the skill in the UI
	public static void SetCharacterUI (int characterIndex, CharacterModel characterModel)
	{
		currentCharacterInEquip [characterIndex] = characterModel;
		ScreenBattleController.Instance.partSkill.SetCharacterUI (characterIndex, characterModel);
	}

	//receive skill list from prepare phase and shuffle for random skill in start and put in queue
	public static void SetCharacterEnqueue (List<CharacterModel> characterList)
	{
		characterQueue.Clear ();
		ListShuffleUtility.Shuffle (characterList);


		for (int i = 0; i < characterList.Count; i++) {
			Debug.Log (characterList [i].characterName);
			characterQueue.Enqueue (characterList [i]);
		}
	}




	//Default 3 starting skills when starting the game
	public static void SetStartCharacters ()
	{
		for (int i = 0; i < 3; i++) {
			SetCharacterUI (i, characterQueue.Dequeue ());
		}
	}

	//When skill is used, remove previous skill and enqueue replace with new skill in queue
	public static void UseCharacterUI (int characterIndex)
	{
		//remove this if you want skill will be gone after use
		characterQueue.Enqueue (currentCharacterInEquip [characterIndex]);

		SetCharacterUI (characterIndex, characterQueue.Dequeue ());
	}

	public static CharacterModel GetCharacter (int characterNumber)
	{
		return currentCharacterInEquip [characterNumber - 1];
	}

	#endregion
}
