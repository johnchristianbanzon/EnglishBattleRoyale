using UnityEngine;
using System;
using System.Collections.Generic;
using NCalc;
using System.Linq;

public class CharacterLogic
{
	private static bool isPlayer;
	private static List<Queue<NCalcFunctionModel>> playerQueueList = new List<Queue<NCalcFunctionModel>> ();
	private static List<Queue<NCalcFunctionModel>> enemyQueueList = new List<Queue<NCalcFunctionModel>> ();

	public static void ResetCharacterLogic ()
	{
		playerQueueList.Clear ();
		enemyQueueList.Clear ();
	}


	public static void CharacterActivate (bool _isPlayer, CharacterModel character)
	{
		isPlayer = _isPlayer;
		string calculateString = "";

		//replace variables from string
		calculateString = character.skillCalculation.
			Replace ("PHP", PlayerManager.GetPlayer (isPlayer).hp.ToString ()).
			Replace ("PGP", PlayerManager.GetPlayer (isPlayer).gp.ToString ()).
			Replace ("PBD", PlayerManager.GetPlayer (isPlayer).bd.ToString ()).
			Replace ("PSD", PlayerManager.GetPlayer (isPlayer).sdm.ToString ()).
			Replace ("PTD", PlayerManager.GetPlayer (isPlayer).td.ToString ()).
			Replace ("PRotten", PlayerManager.GetQuestionResultCount (isPlayer).speedyRottenCount.ToString ()).
			Replace ("PAwesome", PlayerManager.GetQuestionResultCount (isPlayer).speedyAwesomeCount.ToString ()).

			Replace ("EHP", PlayerManager.GetPlayer (!isPlayer).hp.ToString ()).
			Replace ("EyGP", PlayerManager.GetPlayer (!isPlayer).gp.ToString ()).
			Replace ("EBD", PlayerManager.GetPlayer (!isPlayer).bd.ToString ()).
			Replace ("ESD", PlayerManager.GetPlayer (!isPlayer).sdm.ToString ()).
			Replace ("ETD", PlayerManager.GetPlayer (!isPlayer).td.ToString ()).
			Replace ("ERotten", PlayerManager.GetQuestionResultCount (!isPlayer).speedyRottenCount.ToString ()).
			Replace ("EAwesome", PlayerManager.GetQuestionResultCount (!isPlayer).speedyAwesomeCount.ToString ());
		

		//split multiple skill in character
		string[] calculateStringArray = StringSplitToArray (calculateString);

		for (int i = 0; i < calculateStringArray.Length - 1; i++) {

			Expression e = new Expression (calculateStringArray [i]);

			e.EvaluateFunction += NCalcExtensionFunctions;
		}
	}



	//Check turns
	private static void NCalcExtensionFunctions (string name, FunctionArgs args)
	{
		int turns = 0;

		if (args.Parameters.Length == 1) {
			turns = int.Parse (args.Parameters [0].Evaluate ().ToString ());

		}

		if (args.Parameters.Length == 2) {
			turns = int.Parse (args.Parameters [1].Evaluate ().ToString ());

		}

		if (args.Parameters.Length == 3) {
			turns = int.Parse (args.Parameters [1].Evaluate ().ToString ());
		}

		NCalcFunctionModel nCalcFunction = new NCalcFunctionModel (name, args);
		Queue<NCalcFunctionModel> characterQueue = new Queue<NCalcFunctionModel> ();

		//Depending on turn, add to queue
		for (int i = 0; i < turns; i++) {
			characterQueue.Enqueue (nCalcFunction);
		}
		if (isPlayer) {
			playerQueueList.Add (characterQueue);
		} else {
			enemyQueueList.Add (characterQueue);
		}
		CheckTurns ();
	}



	//activate character if turn is existing
	private static void CheckTurns ()
	{
		if (isPlayer) {
			for (int i = 0; i < playerQueueList.Count; i++) {
				//remove item in list if no queues
				if (playerQueueList [i].Count == 0) {
					playerQueueList.RemoveAt (i);
					return;
				}

				NCalcFunctionModel nCalcFunction = playerQueueList [i].Dequeue ();
				CalculateCharacter (nCalcFunction.name, nCalcFunction.args);

				if (playerQueueList [i].Count == 0) {
					ResetPlayer (nCalcFunction.name);
				}
			}
		} else {
			for (int i = 0; i < enemyQueueList.Count; i++) {
				//remove item in list if no queues
				if (enemyQueueList [i].Count == 0) {
					enemyQueueList.RemoveAt (i);
					return;
				}

				NCalcFunctionModel nCalcFunction = enemyQueueList [i].Dequeue ();
				CalculateCharacter (nCalcFunction.name, nCalcFunction.args);

				if (enemyQueueList [i].Count == 0) {
					ResetPlayer (nCalcFunction.name);
				}
			}
		}
	}

	private static void ResetPlayer (string name)
	{

		if (name.Contains ("PlayerSD")) {
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.sdm = MyConst.player.sdm;
			PlayerManager.Player.sdb = false;
		}

		if (name.Contains ("EnemySD")) {
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.sdm = MyConst.player.sdm;
			PlayerManager.Player.sdb = false;
		}

		if (name.Contains ("PlayerBD")) {
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.bd = MyConst.player.bd;
		}

		if (name.Contains ("EnemyBD")) {
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.bd = MyConst.player.bd;
		}

		if (name.Contains ("PlayerTD")) {
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.td = MyConst.player.td;
		}

		if (name.Contains ("EnemyTD")) {
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.td = MyConst.player.td;
		}

		switch (name) {
	
		case "EnemySlot":
			//not yet implemented
			break;
		}

	}
		

	//calculation of skills done here
	private static void CalculateCharacter (string name, FunctionArgs args)
	{
		float value = 0;
		if (args.Parameters.Length > 0) {
			value = float.Parse (args.Parameters [0].Evaluate ().ToString ());

		}

		if (isPlayer) {
			Debug.Log ("PLAYER SKILL EFFECT: " + name + " VALUE: " + value);
		} else {
			Debug.Log ("ENEMY SKILL EFFECT: " + name + " VALUE: " + value);
		}
			
		//TEST ONLY REMOVE LATER
		bool isTargetPlayer = isPlayer;


		switch (name) {
		case "AddPlayerHP":

			PlayerManager.SetIsPlayer (isPlayer);
			//if has skill damage multiplier
			if (PlayerManager.Player.sdb) {
				PlayerManager.Player.hp += value * PlayerManager.Player.sdm;
			} else {
				PlayerManager.Player.hp += value;
			}
				
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
		

			break;
		case "AddEnemyHP":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.hp += value;
			//if has skill damage multiplier
			if (PlayerManager.Player.sdb) {
				PlayerManager.Player.hp += value * PlayerManager.Player.sdm;
			} else {
				PlayerManager.Player.hp += value;
			}
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "AddPlayerGP":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.gp += value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "AddEnemyGP":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.gp += value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "AddPlayerSD":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.sdm += value;
			PlayerManager.Player.sdb = true;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "AddEnemySD":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.sdm += value;
			PlayerManager.Player.sdb = true;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "AddPlayerBD":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.bd += value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "AddEnemyBD":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.bd += value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "AddPlayerTD":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.td += value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "AddEnemyTD":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.td += value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "MultiplyPlayerHP":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.hp *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "MultiplyEnemyHP":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.hp *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "MultiplyPlayerGP":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.gp *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "MultiplyEnemyGP":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.gp *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "MultiplyPlayerSD":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.sdm *= value;
			PlayerManager.Player.sdb = true;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "MultiplyEnemySD":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.sdm *= value;
			PlayerManager.Player.sdb = true;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "MultiplyPlayerBD":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.bd *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "MultiplyEnemyBD":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.bd *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "MultiplyPlayerTD":
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.td *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		case "MultiplyEnemyTD":
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.td *= value;
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			break;
		case "EnemySlot":
			PlayerManager.SetIsPlayer (!isPlayer);
			//TEST ONLY REMOVE LATER
			isTargetPlayer = !isPlayer;
			//not yet implemented
			break;

		//TO-DO find a way for just ailments to debuff not own powerups
		case "PlayerDebuff":
			PlayerManager.SetIsPlayer (isPlayer);
			playerQueueList.Clear ();
			//TEST ONLY REMOVE LATER
			isTargetPlayer = isPlayer;
			break;
		}

		//TEST ONLY REMOVE LATER
		if (isTargetPlayer) {
			Debug.Log ("TARGET: PLAYER");
		} else {
			Debug.Log ("TARGET: ENEMY");
		}

		PlayerManager.UpdateStateUI (isPlayer);
	}

	private static string[] StringSplitToArray (string stringToSplit)
	{
		string[] newResult = stringToSplit.Split (';');
		newResult = newResult.ToArray ();

		return newResult;
	}
}

class NCalcFunctionModel
{
	public string name;
	public FunctionArgs args;

	public NCalcFunctionModel (string name, FunctionArgs args)
	{
		this.name = name;
		this.args = args;
	}
}