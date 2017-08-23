using UnityEngine;
using System.Collections.Generic;
using System;

public class CharacterManager: IRPCDicObserver
{
	private static CharacterModel[] currentCharacterInEquip = new CharacterModel[3];
	private static Queue<CharacterModel> characterQueue = new Queue<CharacterModel> (8);

	public void Init ()
	{
		RPCDicObserver.AddObserver (this);
	}

	#region send character to firebase

	//send characters used to firebase
	public static void StartCharacters ()
	{
		List<CharacterModel> charactersToSend = new List<CharacterModel> ();
		for (int i = 0; i < currentCharacterInEquip.Length; i++) {
			//if gp is enough, send character to firebase and remove from equip
			if (ScreenBattleController.Instance.partState.player.playerGP >= currentCharacterInEquip [i].characterGPCost) {
				Debug.Log ("SENDING TO FIREBASE CHARACTER " + currentCharacterInEquip [i].characterName);
				ScreenBattleController.Instance.partState.player.playerGP -= currentCharacterInEquip [i].characterGPCost;
				charactersToSend.Add (currentCharacterInEquip [i]);
				ActivateCharacterUI (i);

			} else {
				Debug.Log ("NOT ENOUGH GP FOR CHARACTER " + currentCharacterInEquip [i].characterName);
				break;
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
					Debug.Log ("CHARACTER COUNT " + characterReceiveQueue.Count);

				
					if (characterReceiveQueue.Count > 0) {
						if (userHome.Equals (GameManager.isHost)) {
							Debug.Log ("RECEIVE PLAYER CHARACTERS " + characterReceiveQueue.Count);
							playerCharacterQueue = characterReceiveQueue;

						} else {
							Debug.Log ("RECEIVE ENEMY CHARACTERS " + characterReceiveQueue.Count);
							enemyCharacterQueue = characterReceiveQueue;
						}
					}

					BattleManager.CountCharacters ();

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

	public static int GetCharacterCount (bool isPlayer)
	{
		if (isPlayer) {
			return playerCharacterQueue.Count;
		} else {
			return enemyCharacterQueue.Count;
		}
	}

	public static void CharacterActivate (bool isPlayer)
	{
		
		//TO-DO REFACTOR THIS CODE
		CharacterModel character = null;
		if (isPlayer) {
			character = playerCharacterQueue.Dequeue ();

			GameObject cardActivate = SystemResourceController.Instance.LoadPrefab ("CharacterCardActivate",
				                         ScreenBattleController.Instance.partState.playerCardContainer);
			cardActivate.transform.position = ScreenBattleController.Instance.partState.playerCardContainer.transform.position;

			cardActivate.GetComponent<CHaracterCardActivateController> ().ShowCard (character.characterID);

			ScreenBattleController.Instance.partAvatars.LoadCardSkillEffect (true, character.characterID);

		} else {
			character = enemyCharacterQueue.Dequeue ();

			GameObject cardActivate = SystemResourceController.Instance.LoadPrefab ("CharacterCardActivate",
				ScreenBattleController.Instance.partState.enemyCardContainer);
			cardActivate.transform.position = ScreenBattleController.Instance.partState.enemyCardContainer.transform.position;
			cardActivate.GetComponent<CHaracterCardActivateController> ().ShowCard (character.characterID);

			ScreenBattleController.Instance.partAvatars.LoadCardSkillEffect (false, character.characterID);
		
		}


			
		SystemSoundController.Instance.PlaySFX ("SFX_SKILLACTIVATE");
		ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPlayer, "skill1");
		CharacterLogic.CharacterActivate (isPlayer, character);
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

	//When player switch reOrder characters when battle
	public static void SetCharacterOrder (CharacterModel[] indexArray)
	{
		Array.Copy (indexArray, currentCharacterInEquip, 3);

		for (int i = 0; i < currentCharacterInEquip.Length; i++) {
			Debug.Log (currentCharacterInEquip [i].characterName);
		}
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
	public static void ActivateCharacterUI (int characterIndex)
	{
		ScreenBattleController.Instance.partCharacter.ChangeCharacterCard (
			delegate() {
				//Reminders: Remove this if you want characters will be gone after use and not put at the bottom of the queue
				ScreenBattleController.Instance.partCharacter.ActivateCharacterUI (characterIndex);
				characterQueue.Enqueue (currentCharacterInEquip [characterIndex]);

			}, 
			delegate() {
				SetCharacterUI (characterIndex, characterQueue.Dequeue ());

			});
	
	}

	public static CharacterModel GetCharacter (int characterNumber)
	{
		return currentCharacterInEquip [characterNumber - 1];
	}

	#endregion



}
	


