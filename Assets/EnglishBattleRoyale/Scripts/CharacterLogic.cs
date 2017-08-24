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

		Debug.Log ("CALCULATION = " + calculateString);
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

		switch ((CharacterEnums.SkillType)character.characterSkillType) {

		case CharacterEnums.SkillType.PlayerHP:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerHP, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "HP " + (ScreenBattleController.Instance.partState.enemy.playerHP - calculation));
				ScreenBattleController.Instance.partState.enemy.playerHP = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerHP, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "HP " + (ScreenBattleController.Instance.partState.player.playerHP - calculation));
				ScreenBattleController.Instance.partState.player.playerHP = calculation;
			}
			break;

		//DAMAGE
		case CharacterEnums.SkillType.EnemyHP:
			if (isPlayer) {
				//Skill damage to enemy
				if (ScreenBattleController.Instance.partState.player.playerSDB) {
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.enemy.playerHP, calculatedChar) + (calculatedChar * ScreenBattleController.Instance.partState.enemy.playerSDM);
					ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "HP " + (ScreenBattleController.Instance.partState.enemy.playerHP - calculation));
					ScreenBattleController.Instance.partState.enemy.playerHP = calculation;
				} else {
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.enemy.playerHP, calculatedChar);
					ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "HP " + (ScreenBattleController.Instance.partState.enemy.playerHP - calculation));
					ScreenBattleController.Instance.partState.enemy.playerHP = calculation;
				}


			} else {
				//Skill damage from enemy
				if (ScreenBattleController.Instance.partState.enemy.playerSDB) {
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.player.playerHP, calculatedChar) + (calculatedChar * ScreenBattleController.Instance.partState.player.playerSDM);
					ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "HP " + (ScreenBattleController.Instance.partState.player.playerHP - calculation));
					ScreenBattleController.Instance.partState.player.playerHP = calculation;
				} else {
					float calculation = OperatorCalculator (character.characterSkillOperator, 
						                    ScreenBattleController.Instance.partState.player.playerHP, calculatedChar);
					ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "HP " + (ScreenBattleController.Instance.partState.player.playerHP - calculation));
					ScreenBattleController.Instance.partState.player.playerHP = calculation;
				}

			}
			break;

		case CharacterEnums.SkillType.PlayerBD:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerBD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Base Damage " + (ScreenBattleController.Instance.partState.player.playerBD - calculation));
				ScreenBattleController.Instance.partState.player.playerBD = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerBD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Base Damage " + (ScreenBattleController.Instance.partState.enemy.playerBD - calculation));
				ScreenBattleController.Instance.partState.enemy.playerBD = calculation;
			}

			break;

		case CharacterEnums.SkillType.EnemyBD:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerBD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Base Damage " + (ScreenBattleController.Instance.partState.enemy.playerBD - calculation));
				ScreenBattleController.Instance.partState.enemy.playerBD = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerBD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Base Damage " + (ScreenBattleController.Instance.partState.player.playerBD - calculation));
				ScreenBattleController.Instance.partState.player.playerBD = calculation;
			}

			break;

		case CharacterEnums.SkillType.PlayerTD:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerTD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Total Damage " + (ScreenBattleController.Instance.partState.player.playerTD - calculation));
				ScreenBattleController.Instance.partState.player.playerTD = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerTD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Total Damage " + (ScreenBattleController.Instance.partState.enemy.playerTD - calculation));
				ScreenBattleController.Instance.partState.enemy.playerTD = calculation;
			}

			break;

		case CharacterEnums.SkillType.EnemyTD:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerTD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Total Damage " + (ScreenBattleController.Instance.partState.enemy.playerTD - calculation));
				ScreenBattleController.Instance.partState.enemy.playerTD = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerTD, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Total Damage " + (ScreenBattleController.Instance.partState.player.playerTD - calculation));
				ScreenBattleController.Instance.partState.player.playerTD = calculation;
			}

			break;

		case CharacterEnums.SkillType.PlayerSD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.player.playerSDB = true;
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Skill Damage " + (calculatedChar));
			} else {
				ScreenBattleController.Instance.partState.enemy.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.enemy.playerSDB = true;
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Skill Damage " + (calculatedChar));
			}

			break;

		case CharacterEnums.SkillType.EnemySD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.enemy.playerSDB = true;
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "Skill Damage " + (calculatedChar));
			} else {
				ScreenBattleController.Instance.partState.player.playerSDM = calculatedChar;
				ScreenBattleController.Instance.partState.player.playerSDB = true;
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "Skill Damage " + (calculatedChar));
			}

			break;

		case CharacterEnums.SkillType.PlayerGP:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerGP, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "GP " + (ScreenBattleController.Instance.partState.player.playerGP - calculation));
				ScreenBattleController.Instance.partState.player.playerGP = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerGP, calculatedChar); 
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "GP " + (ScreenBattleController.Instance.partState.enemy.playerGP - calculation));
				ScreenBattleController.Instance.partState.enemy.playerGP = calculation;
			}

			break;

		case CharacterEnums.SkillType.EnemyGP:
			if (isPlayer) {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.enemy.playerGP, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (false, "GP " + (ScreenBattleController.Instance.partState.player.playerGP - calculation));
				ScreenBattleController.Instance.partState.enemy.playerGP = calculation;
			} else {
				float calculation = OperatorCalculator (character.characterSkillOperator, 
					                    ScreenBattleController.Instance.partState.player.playerGP, calculatedChar);
				ScreenBattleController.Instance.partState.ShowSkillIndicator (true, "GP " + (ScreenBattleController.Instance.partState.player.playerGP - calculation));
				ScreenBattleController.Instance.partState.player.playerGP = calculation;
			}

			break;

		}
	}

	private static float OperatorCalculator (int operatorType, float destinationVariable, float calculateCharAmount)
	{
		float finalCalculation = 0;
		if (operatorType == 1) {
			finalCalculation = destinationVariable + calculateCharAmount;
		} else if (operatorType == 2) {
			finalCalculation = destinationVariable * calculateCharAmount;
		}

		return finalCalculation;
	}
}
