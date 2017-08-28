using UnityEngine;
using System;
using System.Collections.Generic;
using NCalc;


public class CharacterLogic
{
	public static void CharacterActivate (bool isPlayer, CharacterModel character)
	{

		//REMOVES CHARACTER SKILLS TARGET TO THE PLAYER FROM ENEMY
		if (character.characterSkillCalculation.Contains ("Debuff")) {
			//nerf code here
			if (isPlayer) {
				for (int i = 0; i < characterQueueList.Count; i++) {
					if (characterQueueList [i].Count > 0) {
						CharacterComputeModel charCompute = characterQueueList [i].Peek ();
						if (!charCompute.isPlayer) {
							if ((CharacterEnums.Target)charCompute.character.characterTarget == CharacterEnums.Target.Enemy) {
								characterQueueList [i].Clear ();
							}
						}
					}
				}
				Debug.Log ("ACTIVATING PLAYER CHARACTER - " + character.characterName);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Debuff");
			} else {
				Debug.Log ("ACTIVATING ENEMY CHARACTER - " + character.characterName);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Debuff");
			}

			return;
			
		}

		if (character.characterSkillCalculation.Contains ("EnemyCharacterSlot")) {
			//EnemyCharacterSlot code here
			if (isPlayer) {
				Debug.Log ("ACTIVATING PLAYER CHARACTER - " + character.characterName);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Card Slot Disabled (Not yet implemented)");
			} else {
				Debug.Log ("ACTIVATING ENEMY CHARACTER - " + character.characterName);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Card Slot Disabled (Not yet implemented)");
			}
			return;

		}

		string calculateString = "";

		//parses the string formula from csv

		calculateString = character.characterSkillCalculation.
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
		
		Expression e = new Expression (calculateString);

		float calculatedChar = float.Parse (e.Evaluate ().ToString ());

	
		Queue<CharacterComputeModel> characterQueue = new Queue<CharacterComputeModel> ();
		//Depending on turn, add to queue
		for (int i = 0; i < character.characterTurn; i++) {
			characterQueue.Enqueue (new CharacterComputeModel (isPlayer, character, calculatedChar));
		}
		characterQueueList.Add (characterQueue);

		CheckTurns ();
	}

	static List<Queue<CharacterComputeModel>> characterQueueList = new List<Queue<CharacterComputeModel>> ();

	//activate character if turn is existing
	private static void CheckTurns ()
	{
		for (int i = 0; i < characterQueueList.Count; i++) {
			//remove item in list if no queues
			if (characterQueueList [i].Count == 0) {
				characterQueueList.RemoveAt (i);
				return;
			}
				
			CharacterComputeModel characterCompute = characterQueueList [i].Dequeue ();
			CharacterCompute (
				characterCompute.isPlayer,
				characterCompute.character,
				characterCompute.calculatedChar
			);

			//RESET BACK TO ORIGINAL VALUE
			if (characterQueueList [i].Count == 0) {
				ResetPlayer (characterCompute.isPlayer, characterCompute.character.characterSkillType);
			}
		}
		
	}

	//RESET VALUE OF PLAYER AFTER TURN IS DONE
	private static void ResetPlayer (bool isPLayer, int skillType)
	{
		
		CharacterEnums.SkillType skillTypeEnum = (CharacterEnums.SkillType)skillType;

		switch (skillTypeEnum) {
		case CharacterEnums.SkillType.PlayerSD:
			PlayerManager.SetIsPlayer (isPLayer);
			PlayerManager.Player.sdm = MyConst.player.sdm;
			PlayerManager.Player.sdb = false;

			break;

		case CharacterEnums.SkillType.EnemySD:
			PlayerManager.SetIsPlayer (!isPLayer);
			PlayerManager.Player.sdm = MyConst.player.sdm;
			PlayerManager.Player.sdb = false;
			break;

		case CharacterEnums.SkillType.PlayerBD:
			PlayerManager.SetIsPlayer (isPLayer);
			PlayerManager.Player.bd = MyConst.player.bd;
			break;

		case CharacterEnums.SkillType.EnemyBD:
			PlayerManager.SetIsPlayer (!isPLayer);
			PlayerManager.Player.bd = MyConst.player.bd;
			break;

		case CharacterEnums.SkillType.PlayerTD:
			PlayerManager.SetIsPlayer (isPLayer);
			PlayerManager.Player.td = MyConst.player.td;
			break;

		case CharacterEnums.SkillType.EnemyTD:
			PlayerManager.SetIsPlayer (!isPLayer);
			PlayerManager.Player.td = MyConst.player.td;
			break;
		}
	}
		

	//activates the character and calculate the respective skills
	private static void CharacterCompute (bool isPlayer, CharacterModel character, float calculatedChar)
	{
		if (isPlayer) {
			Debug.Log ("ACTIVATING PLAYER CHARACTER - " + character.characterName);
		} else {
			Debug.Log ("ACTIVATING ENEMY CHARACTER - " + character.characterName);
		}

		Debug.Log ("Calculated Char = " + calculatedChar);

		switch ((CharacterEnums.SkillType)character.characterSkillType) {

		case CharacterEnums.SkillType.PlayerHP:
			SkillCalculator (PlayerManager.Player.hp, character.characterSkillOperator, calculatedChar, isPlayer);	
			break;

		//DAMAGE
		case CharacterEnums.SkillType.EnemyHP:
			if (PlayerManager.Player.sdb) {
				PlayerManager.SetIsPlayer (!isPlayer);
				PlayerManager.Player.hp = OperatorCalculator (character.characterSkillOperator, 
					PlayerManager.Player.hp, calculatedChar) + (calculatedChar * PlayerManager.Player.sdm);
			
			} else {
				SkillCalculator (PlayerManager.Player.hp, character.characterSkillOperator, calculatedChar, !isPlayer);		
			}
			break;

		case CharacterEnums.SkillType.PlayerBD:
			SkillCalculator (PlayerManager.Player.bd, character.characterSkillOperator, calculatedChar, isPlayer);
			break;

		case CharacterEnums.SkillType.EnemyBD:
			SkillCalculator (PlayerManager.Player.bd, character.characterSkillOperator, calculatedChar, !isPlayer);
			break;

		case CharacterEnums.SkillType.PlayerTD:
			SkillCalculator (PlayerManager.Player.td, character.characterSkillOperator, calculatedChar, isPlayer);
			break;

		case CharacterEnums.SkillType.EnemyTD:
			SkillCalculator (PlayerManager.Player.td, character.characterSkillOperator, calculatedChar, !isPlayer);
			break;

		case CharacterEnums.SkillType.PlayerGP:
			SkillCalculator (PlayerManager.Player.gp, character.characterSkillOperator, calculatedChar, isPlayer);
			break;

		case CharacterEnums.SkillType.EnemyGP:
			SkillCalculator (PlayerManager.Player.gp, character.characterSkillOperator, calculatedChar, !isPlayer);
			break;

		case CharacterEnums.SkillType.PlayerSD:
			PlayerManager.SetIsPlayer (isPlayer);
			PlayerManager.Player.sdm = calculatedChar;
			PlayerManager.Player.sdb = true;
			break;

		case CharacterEnums.SkillType.EnemySD:
			PlayerManager.SetIsPlayer (!isPlayer);
			PlayerManager.Player.sdm = calculatedChar;
			PlayerManager.Player.sdb = true;
			break;
		}
	}

	private static void SkillCalculator (float calculation, int skillOperator, float calculatedChar, bool isPLayer)
	{
		PlayerManager.SetIsPlayer (isPLayer);
		calculation = OperatorCalculator (skillOperator, 
			calculation, calculatedChar);
	}

	private static float OperatorCalculator (int operatorType, float destinationVariable, float calculateCharAmount)
	{
		float finalCalculation = 0;

		switch (operatorType) {
		case 1:
			finalCalculation = destinationVariable + calculateCharAmount;
			break;
		case 2:
			finalCalculation = destinationVariable * calculateCharAmount;
			break;
		case 3:
			finalCalculation = destinationVariable + (destinationVariable * calculateCharAmount);
			break;
		}

		return finalCalculation;
	}
}
