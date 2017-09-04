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


	public static void CharacterActivate (bool _isPlayer, CharacterModel character)
	{
		isPlayer = _isPlayer;
		string calculateString = "";

		//replace variables from string
		calculateString = character.skillCalculation.
			Replace ("PlayerHP", PlayerManager.GetPlayer (isPlayer).hp.ToString ()).
			Replace ("PlayerGP", PlayerManager.GetPlayer (isPlayer).gp.ToString ()).
			Replace ("PlayerBD", PlayerManager.GetPlayer (isPlayer).bd.ToString ()).
			Replace ("PlayerSDM", PlayerManager.GetPlayer (isPlayer).sdm.ToString ()).
			Replace ("PlayerTD", PlayerManager.GetPlayer (isPlayer).td.ToString ()).
			Replace ("PlayerRotten", PlayerManager.GetQuestionResultCount (isPlayer).speedyRottenCount.ToString ()).
			Replace ("PlayerAwesome", PlayerManager.GetQuestionResultCount (isPlayer).speedyAwesomeCount.ToString ()).


			Replace ("EnemyHP", PlayerManager.GetPlayer (!isPlayer).hp.ToString ()).
			Replace ("EnemyGP", PlayerManager.GetPlayer (!isPlayer).gp.ToString ()).
			Replace ("EnemyBD", PlayerManager.GetPlayer (!isPlayer).bd.ToString ()).
			Replace ("EnemySDM", PlayerManager.GetPlayer (!isPlayer).sdm.ToString ()).
			Replace ("EnemyTD", PlayerManager.GetPlayer (!isPlayer).td.ToString ()).
			Replace ("EnemyRotten", PlayerManager.GetQuestionResultCount (!isPlayer).speedyRottenCount.ToString ()).
			Replace ("EnemyAwesome", PlayerManager.GetQuestionResultCount (!isPlayer).speedyAwesomeCount.ToString ());
		
		//split multiple skill in character
		string[] calculateStringArray = StringSplitToArray (calculateString);

		for (int i = 0; i < calculateStringArray.Length; i++) {
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

	private static void ResetPlayer(string name){

		if (name.Contains ("PlayerSD")) {
			SetPlayerTarget (true);
			PlayerManager.Player.sdm = MyConst.player.sdm;
			PlayerManager.Player.sdb = false;
		}

		if (name.Contains ("EnemySD")) {
			SetPlayerTarget (false);
			PlayerManager.Player.sdm = MyConst.player.sdm;
			PlayerManager.Player.sdb = false;
		}

		if (name.Contains ("PlayerBD")) {
			SetPlayerTarget (true);
			PlayerManager.Player.bd = MyConst.player.bd;
		}

		if (name.Contains ("EnemyBD")) {
			SetPlayerTarget (false);
			PlayerManager.Player.bd = MyConst.player.bd;
		}

		if (name.Contains ("PlayerTD")) {
			SetPlayerTarget (true);
			PlayerManager.Player.td = MyConst.player.td;
		}

		if (name.Contains ("EnemyTD")) {
			SetPlayerTarget (false);
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
			
		switch (name) {
		case "AddPlayerHP":
			SetPlayerTarget (true);
			//if has skill damage multiplier
			if (PlayerManager.Player.sdb) {
				PlayerManager.Player.hp += value * PlayerManager.Player.sdb;
			} else {
				PlayerManager.Player.hp += value;
			}

			break;
		case "AddEnemyHP":
			SetPlayerTarget (false);
			PlayerManager.Player.hp += value;
			//if has skill damage multiplier
			if (PlayerManager.Player.sdb) {
				PlayerManager.Player.hp += value * PlayerManager.Player.sdb;
			} else {
				PlayerManager.Player.hp += value
			}
			break;
		case "AddPlayerGP":
			SetPlayerTarget (true);
			PlayerManager.Player.gp += value;
			break;
		case "AddEnemyGP":
			SetPlayerTarget (false);
			PlayerManager.Player.gp += value;
			break;
		case "AddPlayerSD":
			SetPlayerTarget (true);
			PlayerManager.Player.sdm += value;
			PlayerManager.Player.sdb = true;
			break;
		case "AddEnemySD":
			SetPlayerTarget (false);
			PlayerManager.Player.sdm += value;
			PlayerManager.Player.sdb = true;
			break;
		case "AddPlayerBD":
			SetPlayerTarget (true);
			PlayerManager.Player.bd += value;
			break;
		case "AddEnemyBD":
			SetPlayerTarget (false);
			PlayerManager.Player.bd += value;
			break;
		case "AddPlayerTD":
			SetPlayerTarget (true);
			PlayerManager.Player.td += value;
			break;
		case "AddEnemyTD":
			SetPlayerTarget (false);
			PlayerManager.Player.td += value;
			break;
		case "MultiplyPlayerHP":
			SetPlayerTarget (true);
			PlayerManager.Player.hp *= value;
			break;
		case "MultiplyEnemyHP":
			SetPlayerTarget (false);
			PlayerManager.Player.hp *= value;
			break;
		case "MultiplyPlayerGP":
			SetPlayerTarget (true);
			PlayerManager.Player.gp *= value;
			break;
		case "MultiplyEnemyGP":
			SetPlayerTarget (false);
			PlayerManager.Player.gp *= value;
			break;
		case "MultiplyPlayerSD":
			SetPlayerTarget (true);
			PlayerManager.Player.sdm *= value;
			PlayerManager.Player.sdb = true;
			break;
		case "MultiplyEnemySD":
			SetPlayerTarget (false);
			PlayerManager.Player.sdm *= value;
			PlayerManager.Player.sdb = true;
			break;
		case "MultiplyPlayerBD":
			SetPlayerTarget (true);
			PlayerManager.Player.bd *= value;
			break;
		case "MultiplyEnemyBD":
			SetPlayerTarget (false);
			PlayerManager.Player.bd *= value;
			break;
		case "MultiplyPlayerTD":
			SetPlayerTarget (true);
			PlayerManager.Player.td *= value;
			break;
		case "MultiplyEnemyTD":
			SetPlayerTarget (false);
			PlayerManager.Player.td *= value;
			break;
		case "EnemySlot":
			//not yet implemented
			break;

		//TO-DO find a way for just ailments to debuff not own powerups
		case "PlayerDebuff":
			playerQueueList.Clear ();
			break;
		}
	}

	//set which player to affect the character skill
	private static void SetPlayerTarget (bool isPlayerTarget)
	{
		if (isPlayer) {
			if (isPlayerTarget) {
				PlayerManager.SetIsPlayer (true);
			} else {
				PlayerManager.SetIsPlayer (false);
			}
		} else {
			if (!isPlayerTarget) {
				PlayerManager.SetIsPlayer (true);
			} else {
				PlayerManager.SetIsPlayer (false);
			}
		}
	}

	private static string[] StringSplitToArray (string stringToSplit)
	{
		string[] newResult = stringToSplit.Split (';');
		newResult = newResult.Skip (1).ToArray ();

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