﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class CharacterManager: IRPCDicObserver
{
	private static CharacterModel[] currentCharacterInEquip = new CharacterModel[3];
	private static Queue<CharacterModel> characterQueue = new Queue<CharacterModel> (8);
	private static Queue<CharacterModel> playerCharacterQueue = new Queue<CharacterModel> ();
	private static Queue<CharacterModel> enemyCharacterQueue = new Queue<CharacterModel> ();

	public static void ResetCharacterManager ()
	{
		characterQueue.Clear ();
		playerCharacterQueue.Clear ();
		enemyCharacterQueue.Clear ();
		Array.Clear (currentCharacterInEquip, 0, currentCharacterInEquip.Length);
	}

	public void Init ()
	{
		RPCDicObserver.AddObserver (this);
	}

	#region send character to firebase

	//send characters used to firebase
	public static void StartCharacters ()
	{
		List<CharacterModel> charactersToSend = new List<CharacterModel> ();
		Dictionary<int,int> result = new Dictionary<int, int> ();
		for (int i = 0; i < currentCharacterInEquip.Length; i++) {
			//if gp is enough, send character to firebase and remove from equip
			PlayerManager.SetIsPlayer (true);
			bool isCardValid = false;


			ScreenBattleController.Instance.partCharacter.GetCharCards () [i].CheckCard (delegate(bool arg1, int arg2) {
				if (arg1) {
					isCardValid = true;
					result.Add(arg2,i);
				} else {
					isCardValid = false;
				}
			});

			if (isCardValid == false) {
				continue;
			}
		}

		//sort result according to priority number of character in equip
		if (result.Count > 0) {
			

			var list = result.Keys.ToList();
			list.Sort ();

			foreach (var key in list) {


				if (PlayerManager.Player.gp >= currentCharacterInEquip [result[key]].gpCost) {
					Debug.Log ("SENDING TO FIREBASE CHARACTER " + currentCharacterInEquip [result[key]].name);
					PlayerManager.Player.gp -= currentCharacterInEquip [result[key]].gpCost;
					charactersToSend.Add (currentCharacterInEquip [result[key]]);
					ActivateCharacterUI (result[key]);
					PlayerManager.UpdateStateUI (true);
				} else {
					Debug.Log ("NOT ENOUGH GP FOR CHARACTER " + currentCharacterInEquip [result[key]].name);
				}
			}
		}



		CharacterModelList characterList = new CharacterModelList ();
		characterList.list = charactersToSend;

		SystemFirebaseDBController.Instance.SetParam (MyConst.RPC_DATA_CHARACTER, (characterList));

		//reset card used effect
		for (int i = 0; i < ScreenBattleController.Instance.partCharacter.GetCharCards().Length; i++) {
			ScreenBattleController.Instance.partCharacter.GetCharCards() [i].ResetCardUsed ();
		}
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
						if (characterList.list [i].iD != 0) {
							characterReceiveQueue.Enqueue (characterList.list [i]);
						}
					}

				
					if (characterReceiveQueue.Count > 0) {
						if (userHome.Equals (GameManager.isHost)) {
							playerCharacterQueue = characterReceiveQueue;

						} else {
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



	public static int GetCharacterCount (bool isPlayer)
	{
		if (isPlayer) {
			return playerCharacterQueue.Count;
		} else {
			return enemyCharacterQueue.Count;
		}
	}

	//TO-DO REFACTOR THIS CODE
	public static IEnumerator CharacterActivate (bool isPlayer)
	{
		CharacterModel character = null;
		if (isPlayer) {
			character = playerCharacterQueue.Dequeue ();
		} else {
			character = enemyCharacterQueue.Dequeue ();
		}

//		ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPlayer, "Cast");

		GameObject skillCastDetails = SystemResourceController.Instance.LoadPrefab ("SkillCastDetails", ScreenBattleController.Instance.partState.gameObject);
		yield return skillCastDetails.GetComponent<SkillCastDetailsController> ().SkillDetailCoroutine (character);

		//Show card skill effect
		yield return ScreenBattleController.Instance.partAvatars.LoadCardSkillEffect (isPlayer, character.particleID);

		//Do the calculation
		CharacterLogic.CharacterActivate (isPlayer, character);
	}

	#endregion


	#region get character list and assign to equip

	/// Load skill list from parsed CSV
	public static List<CharacterModel>  GetCharacterList ()
	{
		List<CharacterModel> characterList = MyConst.GetCharacterList ();

		return characterList;
	}

	//TO-DO: store in data, use pantoyprefs or something, if characters already present in data, no need to generate,
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
		

	//Receive skill list from prepare phase and shuffle for random skill in start and put in queue
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
	


