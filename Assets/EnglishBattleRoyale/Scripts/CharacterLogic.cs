using UnityEngine;
using NCalc;

public class CharacterLogic
{

	public static void CharacterActivate (bool isPlayer, CharacterModel character)
	{
		if (character.characterSkillCalculation.Contains ("PlayerNerf")) {
			//nerf code here
			if (isPlayer) {
				
			} else {
			
			}

			return;
			
		}

		if (character.characterSkillCalculation.Contains ("EnemyCharacterSlot")) {
			//EnemyCharacterSlot code here
			if (isPlayer) {
				
			} else {
			
			}
			return;

		}

		//parses the string formula from csv
		if (isPlayer) {
			
			character.characterSkillCalculation = character.characterSkillCalculation.
				Replace ("PlayerHP", ScreenBattleController.Instance.partState.player.playerHP.ToString ()).
				Replace ("PlayerGP", ScreenBattleController.Instance.partState.player.playerGP.ToString ()).
				Replace ("PlayerBD", ScreenBattleController.Instance.partState.player.playerBD.ToString ()).
				Replace ("PlayerSD", ScreenBattleController.Instance.partState.player.playerSD.ToString ()).
				Replace ("PlayerTD", ScreenBattleController.Instance.partState.player.playerTD.ToString ()).
				Replace ("PlayerRotten", GameManager.playerAnswerParam.speedyRottenCount.ToString ()).
				Replace ("PlayerAwesome", GameManager.playerAnswerParam.speedyAwesomeCount.ToString ());
		} else {
			character.characterSkillCalculation = character.characterSkillCalculation.
				Replace ("EnemyHP", ScreenBattleController.Instance.partState.enemy.playerHP.ToString ()).
				Replace ("EnemyHP", ScreenBattleController.Instance.partState.enemy.playerGP.ToString ()).
				Replace ("EnemyBD", ScreenBattleController.Instance.partState.enemy.playerBD.ToString ()).
				Replace ("EnemySD", ScreenBattleController.Instance.partState.enemy.playerSD.ToString ()).
				Replace ("EnemyTD", ScreenBattleController.Instance.partState.enemy.playerTD.ToString ()).
				Replace ("EnemyRotten", GameManager.enemyAnswerParam.speedyRottenCount.ToString ()).
				Replace ("EnemyAwesome", GameManager.enemyAnswerParam.speedyAwesomeCount.ToString ());
		}
		Expression e = new Expression (character.characterSkillCalculation);
		CharacterCompute (isPlayer, character, float.Parse (e.Evaluate ().ToString ()));

	}
		
	//activates the character and calculate the respective skills
	private static void CharacterCompute (bool isPlayer, CharacterModel character, float calculateCharAmount)
	{

		switch ((CharacterEnums.SkillType)character.characterSkillType) {

		case CharacterEnums.SkillType.PlayerHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerHP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerHP, calculateCharAmount);

			} else {
				ScreenBattleController.Instance.partState.enemy.playerHP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerHP, calculateCharAmount);
			}
			break;

		//DAMAGE
		case CharacterEnums.SkillType.EnemyHP:
			if (isPlayer) {
				//Skill damage to enemy
				ScreenBattleController.Instance.partState.enemy.playerHP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerHP, calculateCharAmount) * ScreenBattleController.Instance.partState.enemy.playerSD;
			} else {
				//Skill damage from enemy

				ScreenBattleController.Instance.partState.player.playerHP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerHP, calculateCharAmount) * ScreenBattleController.Instance.partState.player.playerSD;
			}
			break;

		case CharacterEnums.SkillType.PlayerBD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerBD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerBD, calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerBD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerBD, calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.EnemyBD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerBD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerBD, calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerBD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerBD, calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.PlayerTD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerTD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerTD, calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerTD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerTD, calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.EnemyTD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerTD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerTD, calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerTD = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerTD, calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.PlayerSD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerSD = calculateCharAmount;
			} else {
				ScreenBattleController.Instance.partState.enemy.playerSD = calculateCharAmount;
			}

			break;

		case CharacterEnums.SkillType.EnemySD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerSD = calculateCharAmount;
			} else {
				ScreenBattleController.Instance.partState.player.playerSD = calculateCharAmount;
			}

			break;

		case CharacterEnums.SkillType.PlayerGP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerGP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerGP, calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerGP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerGP, calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.EnemyGP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerGP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.enemy.playerGP, calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerGP = OperatorCalculator (character.characterSkillOperator, 
					ScreenBattleController.Instance.partState.player.playerGP, calculateCharAmount);
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
