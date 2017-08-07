using UnityEngine;
using System.Collections.Generic;
using NCalc;
using System;



public class CharacterManager: IRPCDicObserver
{
	private static CharacterModel[] currentCharacterInEquip = new CharacterModel[3];
	private static Queue<CharacterModel> characterQueue = new Queue<CharacterModel> (8);
	private static int characterReceiveCount = 0;

	public void Init ()
	{
		RPCDicObserver.AddObserver (this);
	}

	#region send character to firebase

	//send characters used to firebase
	public static void StartCharacter (bool[] characterButtonToggleOn)
	{
		List<CharacterModel> charactersToSend = new List<CharacterModel> ();
		for (int i = 0; i < characterButtonToggleOn.Length; i++) {
			if (characterButtonToggleOn [i] == true) {
				charactersToSend.Add (currentCharacterInEquip [i]);
			} else {
				charactersToSend.Add (null);
			}
		}

		CharacterModelList characterList = new CharacterModelList ();
		characterList.list = charactersToSend;

		SystemFirebaseDBController.Instance.SetParam (MyConst.RPC_DATA_CHARACTER, (characterList));
	}

	#endregion

	#region Receive character from firebase and also get characters to activate

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {

				bool userHome = (bool)rpcReceive [MyConst.RPC_DATA_USERHOME];
				SystemGlobalDataController.Instance.isSender = userHome;

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];
				if (param.ContainsKey (MyConst.RPC_DATA_CHARACTER)) {

					CharacterModelList characterList = JsonUtility.FromJson<CharacterModelList> (param [MyConst.RPC_DATA_CHARACTER].ToString ());
					Queue<Action> characterActionList = new Queue<Action> ();

					for (int i = 0; i < characterList.list.Count; i++) {
						characterActionList.Enqueue (delegate() {
							CharacterActivate (characterList.list [i]);
						});
					}

					if (SystemGlobalDataController.Instance.isSender.Equals (SystemGlobalDataController.Instance.isHost)) {
						BattleManager.SetPlayerActionQueue (characterActionList);

					} else {
						BattleManager.SetEnemyActionQueue (characterActionList);
					}

				}

			}
		} catch (System.Exception e) {
			//do something with exception in future
		}
	}


	#endregion


	#region activate a character

	public static void CharacterActivate (CharacterModel character)
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

		CharacterCompute (character, float.Parse (e.Evaluate ().ToString ()));
	}
		
	//activates the character and calculate the respective skills
	private static void CharacterCompute (CharacterModel character, float calculateCharAmount)
	{
		switch ((SkillEnum)character.characterSkillID) {


		case SkillEnum.DecreaseEnemyBaseDamage:
			ScreenBattleController.Instance.partState.enemy.playerBaseDamage -= calculateCharAmount;
			break;
		case SkillEnum.DereaseEnemyHP:
			ScreenBattleController.Instance.partState.enemy.playerHP -= calculateCharAmount;
			break;
		case SkillEnum.IncreasePlayerBaseDamage:
			ScreenBattleController.Instance.partState.player.playerBaseDamage += calculateCharAmount;
			break;
		case SkillEnum.IncreasePlayerGP:
			ScreenBattleController.Instance.partState.player.playerGP += calculateCharAmount;
			break;
		case SkillEnum.IncreasePlayerHP:
			ScreenBattleController.Instance.partState.enemy.playerHP += calculateCharAmount;
			break;
		case SkillEnum.LimitEnemySkill:
			//not yet
			break;
		case SkillEnum.Stop:
			//not yet
			break;
		default:
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
		List<CharacterModel> characterList = MyConst.GetCharacterList ();

		return characterList;
	}

	//TEST ONLY FOR NOW!!!!! Add 8 characters to equip
	public static List<CharacterModel>  GetEquipCharacterList ()
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


	//set the skill in the UI
	private static void SetCharacterUI (int characterIndex, CharacterModel character)
	{
		currentCharacterInEquip [characterIndex] = character;
		ScreenBattleController.Instance.partCharacter.SetCharacterUI (characterIndex, character);
	}

	//receive skill list from prepare phase and shuffle for random skill in start and put in queue
	public static void SetCharacterEnqueue (List<CharacterModel> characterList)
	{
		characterQueue.Clear ();
		ListShuffleUtility.Shuffle (characterList);


		for (int i = 0; i < characterList.Count; i++) {
			characterQueue.Enqueue (characterList [i]);
		}
	}

	//Default 3 starting characters when starting the game
	public static void SetStartCharacters ()
	{
		for (int i = 0; i < 3; i++) {
			SetCharacterUI (i, characterQueue.Dequeue ());
		}
	}

	//When characters is used, remove previous characters and enqueue replace with new characters in queue
	public static void UseCharacterUI (int characterIndex)
	{
		//Reminders: Remove this if you want characters will be gone after use
//		characterQueue.Enqueue (currentCharacterInEquip [characterIndex]);

		SetCharacterUI (characterIndex, characterQueue.Dequeue ());
	}

	public static CharacterModel GetCharacter (int characterNumber)
	{
		return currentCharacterInEquip [characterNumber - 1];
	}

	#endregion



}
	


