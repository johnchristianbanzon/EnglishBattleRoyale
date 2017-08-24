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
				Replace ("PlayerHP", ScreenBattleController.Instance.partState.player.playerHP.ToString ()).
				Replace ("PlayerGP", ScreenBattleController.Instance.partState.player.playerGP.ToString ()).
				Replace ("PlayerBD", ScreenBattleController.Instance.partState.player.playerBD.ToString ()).
				Replace ("PlayerSDM", ScreenBattleController.Instance.partState.player.playerSDM.ToString ()).
				Replace ("PlayerTD", ScreenBattleController.Instance.partState.player.playerTD.ToString ()).
				Replace ("PlayerRotten", GameManager.playerAnswerParam.speedyRottenCount.ToString ()).
				Replace ("PlayerAwesome", GameManager.playerAnswerParam.speedyAwesomeCount.ToString ()).
				Replace ("EnemyHP", ScreenBattleController.Instance.partState.enemy.playerHP.ToString ()).
				Replace ("EnemyGP", ScreenBattleController.Instance.partState.enemy.playerGP.ToString ()).
				Replace ("EnemyBD", ScreenBattleController.Instance.partState.enemy.playerBD.ToString ()).
				Replace ("EnemySDM", ScreenBattleController.Instance.partState.enemy.playerSDM.ToString ()).
				Replace ("EnemyTD", ScreenBattleController.Instance.partState.enemy.playerTD.ToString ()).
				Replace ("EnemyRotten", GameManager.enemyAnswerParam.speedyRottenCount.ToString ()).
				Replace ("EnemyAwesome", GameManager.enemyAnswerParam.speedyAwesomeCount.ToString ());
		
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
			if (isPLayer) {
				ScreenBattleController.Instance.partState.player.playerSDM = MyConst.player.playerSDM;
				ScreenBattleController.Instance.partState.player.playerSDB = false;
			} else {
				ScreenBattleController.Instance.partState.enemy.playerSDM = MyConst.player.playerSDM;
				ScreenBattleController.Instance.partState.enemy.playerSDB = false;
			}
			break;

		case CharacterEnums.SkillType.EnemySD:
			if (isPLayer) {
				ScreenBattleController.Instance.partState.enemy.playerSDM = MyConst.player.playerSDM;
				ScreenBattleController.Instance.partState.enemy.playerSDB = false;
			} else {
				ScreenBattleController.Instance.partState.player.playerSDM = MyConst.player.playerSDM;
				ScreenBattleController.Instance.partState.player.playerSDB = false;
			}
			break;

		case CharacterEnums.SkillType.PlayerBD:
			if (isPLayer) {
				ScreenBattleController.Instance.partState.player.playerBD = MyConst.player.playerBD;
			} else {
				ScreenBattleController.Instance.partState.enemy.playerBD = MyConst.player.playerBD;
			}
			break;

		case CharacterEnums.SkillType.EnemyBD:
			if (isPLayer) {
				ScreenBattleController.Instance.partState.enemy.playerBD = MyConst.player.playerBD;
			} else {
				ScreenBattleController.Instance.partState.player.playerBD = MyConst.player.playerBD;
			}
			break;

		case CharacterEnums.SkillType.PlayerTD:
			if (isPLayer) {
				ScreenBattleController.Instance.partState.player.playerTD = MyConst.player.playerTD;
			} else {
				ScreenBattleController.Instance.partState.enemy.playerTD = MyConst.player.playerTD;
			}
			break;

		case CharacterEnums.SkillType.EnemyTD:
			if (isPLayer) {
				ScreenBattleController.Instance.partState.enemy.playerTD = MyConst.player.playerTD;
			} else {
				ScreenBattleController.Instance.partState.player.playerTD = MyConst.player.playerTD;
			}
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
			if (isPlayer) {
				Debug.Log ("Player Current HP" + ScreenBattleController.Instance.partState.player.playerHP);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerHP, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "HP " + (calculation - ScreenBattleController.Instance.partState.player.playerHP));
				ScreenBattleController.Instance.partState.player.playerHP = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player HP = " + (calculation - ScreenBattleController.Instance.partState.player.playerHP));
			} else {
				Debug.Log ("Enemy Current HP" + ScreenBattleController.Instance.partState.enemy.playerHP);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerHP, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "HP " + (calculation - ScreenBattleController.Instance.partState.enemy.playerHP));
				ScreenBattleController.Instance.partState.enemy.playerHP = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy HP = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerHP));
			}
			break;

		//DAMAGE
		case CharacterEnums.SkillType.EnemyHP:
			if (isPlayer) {
				//Skill damage to enemy

				if (ScreenBattleController.Instance.partState.player.playerSDB) {
					Debug.Log ("Enemy Current HP" + ScreenBattleController.Instance.partState.enemy.playerHP);
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.enemy.playerHP, calculatedChar) + (calculatedChar * ScreenBattleController.Instance.partState.enemy.playerSDM);
//					ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "HP " + (calculation - ScreenBattleController.Instance.partState.enemy.playerHP));
					ScreenBattleController.Instance.partState.enemy.playerHP = calculation;
					Debug.Log("Calculation = " + calculation);
					Debug.Log ("Enemy HP = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerHP));
				} else {
					Debug.Log ("Enemy Current HP" + ScreenBattleController.Instance.partState.enemy.playerHP);
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.enemy.playerHP, calculatedChar);
//					ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "HP " + (calculation - ScreenBattleController.Instance.partState.enemy.playerHP));
					ScreenBattleController.Instance.partState.enemy.playerHP = calculation;
					Debug.Log("Calculation = " + calculation);
					Debug.Log ("Enemy HP = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerHP));
				}


			} else {
				//Skill damage from enemy
				if (ScreenBattleController.Instance.partState.enemy.playerSDB) {
					Debug.Log ("Player Current HP" + ScreenBattleController.Instance.partState.player.playerHP);
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.player.playerHP, calculatedChar) + (calculatedChar * ScreenBattleController.Instance.partState.player.playerSDM);
//					ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "HP " + (calculation - ScreenBattleController.Instance.partState.player.playerHP));
					ScreenBattleController.Instance.partState.player.playerHP = calculation;
					Debug.Log("Calculation = " + calculation);
					Debug.Log ("Player HP = " + (calculation - ScreenBattleController.Instance.partState.player.playerHP));
				} else {
					Debug.Log ("Player Current HP" + ScreenBattleController.Instance.partState.player.playerHP);
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.player.playerHP, calculatedChar);
//					ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "HP " + (calculation - ScreenBattleController.Instance.partState.player.playerHP));
					ScreenBattleController.Instance.partState.player.playerHP = calculation;
					Debug.Log("Calculation = " + calculation);
					Debug.Log ("Player HP = " + (calculation - ScreenBattleController.Instance.partState.player.playerHP));
				}

			}
			break;

		case CharacterEnums.SkillType.PlayerBD:
			if (isPlayer) {
				Debug.Log ("Player Current BD" + ScreenBattleController.Instance.partState.player.playerBD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerBD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Base Damage " + (calculation - ScreenBattleController.Instance.partState.player.playerBD));
				ScreenBattleController.Instance.partState.player.playerBD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player BD = " + (calculation - ScreenBattleController.Instance.partState.player.playerBD));
			} else {
				Debug.Log ("Enemy Current BD" + ScreenBattleController.Instance.partState.enemy.playerBD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerBD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Base Damage " + (calculation - ScreenBattleController.Instance.partState.enemy.playerBD));
				ScreenBattleController.Instance.partState.enemy.playerBD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy BD = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerBD));
			}

			break;

		case CharacterEnums.SkillType.EnemyBD:
			if (isPlayer) {
				Debug.Log ("Enemy Current BD" + ScreenBattleController.Instance.partState.enemy.playerBD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerBD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Base Damage " + (calculation - ScreenBattleController.Instance.partState.enemy.playerBD));
				ScreenBattleController.Instance.partState.enemy.playerBD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy BD = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerBD));

			} else {
				Debug.Log ("Player Current BD" + ScreenBattleController.Instance.partState.player.playerBD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerBD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Base Damage " + (calculation - ScreenBattleController.Instance.partState.player.playerBD));
				ScreenBattleController.Instance.partState.player.playerBD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player BD = " + (calculation - ScreenBattleController.Instance.partState.player.playerBD));
			}

			break;

		case CharacterEnums.SkillType.PlayerTD:
			if (isPlayer) {
				Debug.Log ("Player Current TD" + ScreenBattleController.Instance.partState.player.playerTD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerTD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Total Damage " + (calculation - ScreenBattleController.Instance.partState.player.playerTD));
				ScreenBattleController.Instance.partState.player.playerTD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player TD = " + (calculation - ScreenBattleController.Instance.partState.player.playerTD));
			} else {
				Debug.Log ("Enemy Current TD" + ScreenBattleController.Instance.partState.enemy.playerTD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerTD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Total Damage " + (calculation - ScreenBattleController.Instance.partState.enemy.playerTD));
				ScreenBattleController.Instance.partState.enemy.playerTD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy TD = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerTD));
			}

			break;

		case CharacterEnums.SkillType.EnemyTD:
			if (isPlayer) {
				Debug.Log ("Enemy Current TD" + ScreenBattleController.Instance.partState.enemy.playerTD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerTD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Total Damage " + (calculation - ScreenBattleController.Instance.partState.enemy.playerTD));
				ScreenBattleController.Instance.partState.enemy.playerTD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy TD = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerTD));
			} else {
				Debug.Log ("Player Current TD" + ScreenBattleController.Instance.partState.player.playerTD);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerTD, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Total Damage " + (calculation - ScreenBattleController.Instance.partState.player.playerTD));
				ScreenBattleController.Instance.partState.player.playerTD = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player TD = " + (calculation - ScreenBattleController.Instance.partState.player.playerTD));
			}

			break;

		case CharacterEnums.SkillType.PlayerSD:
			if (isPlayer) {
				Debug.Log ("Player Current SD" + ScreenBattleController.Instance.partState.player.playerSDM);
				ScreenBattleController.Instance.partState.player.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.player.playerSDB = true;
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Skill Damage " + (calculatedChar));
				Debug.Log ("Player SD = " + (calculatedChar));
			} else {
				Debug.Log ("Enemy Current SD" + ScreenBattleController.Instance.partState.enemy.playerSDM);
				ScreenBattleController.Instance.partState.enemy.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.enemy.playerSDB = true;
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Skill Damage " + (calculatedChar));
				Debug.Log ("Enemy SD = " + (calculatedChar));
			}

			break;

		case CharacterEnums.SkillType.EnemySD:
			if (isPlayer) {
				Debug.Log ("Enemy Current SD" + ScreenBattleController.Instance.partState.enemy.playerSDM);
				ScreenBattleController.Instance.partState.enemy.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.enemy.playerSDB = true;
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Skill Damage " + (calculatedChar));
				Debug.Log ("Enemy SD = " + (calculatedChar));
			} else {
				Debug.Log ("Player Current SD" + ScreenBattleController.Instance.partState.player.playerSDM);
				ScreenBattleController.Instance.partState.player.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.player.playerSDB = true;
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Skill Damage " + (calculatedChar));
				Debug.Log ("Player SD = " + (calculatedChar));
			}

			break;

		case CharacterEnums.SkillType.PlayerGP:
			if (isPlayer) {
				Debug.Log ("Player Current GP" + ScreenBattleController.Instance.partState.player.playerGP);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerGP, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "GP " + (calculation - ScreenBattleController.Instance.partState.player.playerGP));
				ScreenBattleController.Instance.partState.player.playerGP = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player GP = " + (calculation - ScreenBattleController.Instance.partState.player.playerGP));
			} else {
				Debug.Log ("Enemy Current GP" + ScreenBattleController.Instance.partState.enemy.playerGP);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerGP, calculatedChar); 
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "GP " + (calculation - ScreenBattleController.Instance.partState.enemy.playerGP));
				ScreenBattleController.Instance.partState.enemy.playerGP = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy GP = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerGP));
			}

			break;

		case CharacterEnums.SkillType.EnemyGP:
			if (isPlayer) {
				Debug.Log ("Enemy Current GP" + ScreenBattleController.Instance.partState.enemy.playerGP);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerGP, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "GP " + (calculation - ScreenBattleController.Instance.partState.enemy.playerGP));
				ScreenBattleController.Instance.partState.enemy.playerGP = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Enemy GP = " + (calculation - ScreenBattleController.Instance.partState.enemy.playerGP));

			} else {
				Debug.Log ("Player Current GP" + ScreenBattleController.Instance.partState.player.playerGP);
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerGP, calculatedChar);
//				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "GP " + (calculation - ScreenBattleController.Instance.partState.player.playerGP));
				ScreenBattleController.Instance.partState.player.playerGP = calculation;
				Debug.Log("Calculation = " + calculation);
				Debug.Log ("Player GP = " + (calculation - ScreenBattleController.Instance.partState.player.playerGP));
			}

			break;

		}
	}

	private static float OperatorCalculator (int operatorType, float destinationVariable, float calculateCharAmount)
	{
		float finalCalculation = 0;

		switch(operatorType){
		case 1:
			finalCalculation = destinationVariable + calculateCharAmount;
			break;
		case 2:
			finalCalculation = destinationVariable * calculateCharAmount;
			break;
		case 3:
			finalCalculation = destinationVariable +  (destinationVariable * calculateCharAmount);
			break;
		}

		return finalCalculation;
	}
}
