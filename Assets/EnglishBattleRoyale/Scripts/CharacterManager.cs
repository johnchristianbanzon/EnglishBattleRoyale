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

				//if gp is enough, send character to firebase and remove from equip
				if (ScreenBattleController.Instance.partState.player.playerGP >= currentCharacterInEquip [i].characterGPCost) {
					Debug.Log ("SENDING TO FIREBASE CHARACTER " + currentCharacterInEquip [i].characterName);
					ScreenBattleController.Instance.partState.player.playerGP -= currentCharacterInEquip [i].characterGPCost;
					charactersToSend.Add (currentCharacterInEquip [i]);
					UseCharacterUI (i);
				} else {
					Debug.Log ("NOT ENOUGH GP FOR CHARACTER " + currentCharacterInEquip [i].characterName);
				}
					
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

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];
				if (param.ContainsKey (MyConst.RPC_DATA_CHARACTER)) {

					CharacterModelList characterList = JsonUtility.FromJson<CharacterModelList> (param [MyConst.RPC_DATA_CHARACTER].ToString ());
					Queue<CharacterModel> characterReceiveQueue = new Queue<CharacterModel> ();
				
					for (int i = 0; i < characterList.list.Count; i++) {
						if (characterList.list [i].characterID != 0) {
							characterReceiveQueue.Enqueue (characterList.list [i]);
						}
					}

					BattleManager.CountCharacters ();
					if (characterReceiveQueue.Count > 0) {
						if (userHome.Equals (GameManager.isHost)) {
							Debug.Log ("RECEIVE PLAYER CHARACTERS " + characterReceiveQueue.Count);
							playerCharacterQueue = characterReceiveQueue;

						} else {
							Debug.Log ("RECEIVE ENEMY CHARACTERS " + characterReceiveQueue.Count);
							enemyCharacterQueue = characterReceiveQueue;
						}
					}

				}

			}
		} catch (System.Exception e) {
			//do something with exception in future
		}
	}


	#endregion


	#region activate a character

	private static Queue<CharacterModel> playerCharacterQueue = new Queue<CharacterModel> ();
	private static Queue<CharacterModel> enemyCharacterQueue = new Queue<CharacterModel> ();

	public static void PlayerCharacterActivate ()
	{
		Debug.Log ("PLAYER CHARACTERS REMAINING: " +playerCharacterQueue.Count);
		if (playerCharacterQueue.Count > 0) {
			CharacterModel character = playerCharacterQueue.Dequeue ();
			Debug.Log ("ACTIVATING PLAYER CHARACTER - " + character.characterName);
			CharacterActivate (true, character);
			PlayerCharacterActivate ();
		}
	}

	public static void EnemyCharacterActivate ()
	{
		Debug.Log ("ENEMY CHARACTERS REMAINING: " +enemyCharacterQueue.Count);
		if (enemyCharacterQueue.Count > 0) {
			CharacterModel character = enemyCharacterQueue.Dequeue ();
			Debug.Log ("ACTIVATING ENEMY CHARACTER - " + character.characterName);
			CharacterActivate (false, character);
			EnemyCharacterActivate ();
		}
	}

	private static void CharacterActivate (bool isPlayer, CharacterModel character)
	{

		float variable = 0;
		switch (character.characterAmountVariable) {
		case "none":
			variable = 0;
			break;
		case "enemyHP":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.enemy.playerHP;
			} else {
				variable = ScreenBattleController.Instance.partState.player.playerHP;
			}


			break;
		case "playerHP":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.player.playerHP;
			} else {
				variable = ScreenBattleController.Instance.partState.enemy.playerHP;
			}


			break;
		case "enemyDamage":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.enemy.playerBaseDamage;
			} else {
				variable = ScreenBattleController.Instance.partState.player.playerBaseDamage;
			}


			break;
		case "playerDamage":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.player.playerBaseDamage;
			} else {
				variable = ScreenBattleController.Instance.partState.enemy.playerBaseDamage;
			}


			break;
		case "correctAnswer":
			if (isPlayer) {

			}
			break;
		}

		//parses the string formula from csv

		Expression e = new Expression (character.characterAmount);
		e.Parameters ["N"] = variable;  
//		string amountStr = "10 + enemyHP";
//		int enemyHP = 10;
//		amountStr.Replace ("enemyHP", enemyHP).Replace ("playerHP", playerHP);



		CharacterCompute (isPlayer, character.characterSkillID, float.Parse (e.Evaluate ().ToString ()));
	}
		
	//activates the character and calculate the respective skills
	private static void CharacterCompute (bool isPlayer, int skillID, float calculateCharAmount)
	{
		switch ((SkillEnum)skillID) {

		case SkillEnum.DecreaseEnemyBaseDamage:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerBaseDamage -= calculateCharAmount;
				Debug.Log ("ENEMY BASE DAMAGE DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerBaseDamage -= calculateCharAmount;
				Debug.Log ("PLAYER BASE DAMAGE DECREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.DereaseEnemyHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerHP -= calculateCharAmount;
				Debug.Log ("ENEMY HP DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerHP -= calculateCharAmount;
				Debug.Log ("PLAYER HP DECREASED BY " + calculateCharAmount);
			}
		

			break;
		case SkillEnum.IncreasePlayerBaseDamage:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerBaseDamage += calculateCharAmount;
				Debug.Log ("PLAYER BASE DAMAGE INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerBaseDamage += calculateCharAmount;
				Debug.Log ("ENEMY BASE DAMAGE INCREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.IncreasePlayerGP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerGP += calculateCharAmount;
				Debug.Log ("PLAYER GP INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerGP += calculateCharAmount;
				Debug.Log ("ENEMY GP INCREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.IncreasePlayerHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerHP += calculateCharAmount;
				Debug.Log ("PLAYER HP INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerHP += calculateCharAmount;
				Debug.Log ("ENEMY HP INCREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.LimitEnemySkill:
			Debug.Log ("CHARACTER EFFECT NOT IMPLEMENTED");
			break;
		case SkillEnum.Stop:
			Debug.Log ("CHARACTER EFFECT NOT IMPLEMENTED");
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

	//TO-DO: if characters already present in data, no need to generate
	//Add 8 random characters to equip by default
	public static List<CharacterModel>  GetEquipCharacterList ()
	{
		List<CharacterModel> equipCharacterList = new List<CharacterModel> (8);
		List<CharacterModel> characterList = GetCharacterList ();
		characterList = ListShuffleUtility.Shuffle (characterList);

		for (int i = 0; i < 8; i++) {
			equipCharacterList.Add (characterList [i]);
		}
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
		//Reminders: Remove this if you want characters will be gone after use and not put at the bottom of the queue
//		characterQueue.Enqueue (currentCharacterInEquip [characterIndex]);

		SetCharacterUI (characterIndex, characterQueue.Dequeue ());
	}

	public static CharacterModel GetCharacter (int characterNumber)
	{
		return currentCharacterInEquip [characterNumber - 1];
	}

	#endregion



}
	


