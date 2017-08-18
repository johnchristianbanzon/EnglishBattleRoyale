using UnityEngine;
using NCalc;

public class CharacterLogic
{

	public static void CharacterActivate (bool isPlayer, CharacterModel character)
	{
		if(character.characterCalculation.Contains("PlayerNerf")){
			//nerf code here
			if (isPlayer) {
				
			} else {
			
			}

			return;
			
		}

		if(character.characterCalculation.Contains("EnemyCharacterSlot")){
			//EnemyCharacterSlot code here
			if (isPlayer) {
				
			} else {
			
			}
			return;

		}

		//parses the string formula from csv
		if (isPlayer) {
			
			character.characterCalculation = character.characterCalculation.
				Replace ("PlayerHP", ScreenBattleController.Instance.partState.player.playerHP.ToString()).
				Replace ("PlayerGP", ScreenBattleController.Instance.partState.player.playerGP.ToString()).
				Replace ("PlayerBD", ScreenBattleController.Instance.partState.player.playerBD.ToString()).
				Replace ("PlayerSD", ScreenBattleController.Instance.partState.player.playerSD.ToString()).
				Replace ("PlayerTD", ScreenBattleController.Instance.partState.player.playerTD.ToString()).
				Replace ("PlayerRotten", GameManager.playerAnswerParam.speedyRottenCount.ToString()).
				Replace ("PlayerAwesome", GameManager.playerAnswerParam.speedyAwesomeCount.ToString());
		} else {
			character.characterCalculation = character.characterCalculation.
				Replace ("EnemyHP", ScreenBattleController.Instance.partState.enemy.playerHP.ToString()).
				Replace ("EnemyHP", ScreenBattleController.Instance.partState.enemy.playerGP.ToString()).
				Replace ("EnemyBD", ScreenBattleController.Instance.partState.enemy.playerBD.ToString()).
				Replace ("EnemySD", ScreenBattleController.Instance.partState.enemy.playerSD.ToString()).
				Replace ("EnemyTD", ScreenBattleController.Instance.partState.enemy.playerTD.ToString()).
				Replace ("EnemyRotten", GameManager.enemyAnswerParam.speedyRottenCount.ToString()).
				Replace ("EnemyAwesome", GameManager.enemyAnswerParam.speedyAwesomeCount.ToString());
		}
		Expression e = new Expression (character.characterCalculation);
		CharacterCompute (isPlayer, character.characterSkillType, float.Parse (e.Evaluate ().ToString ()));
	}

	//activates the character and calculate the respective skills
	private static void CharacterCompute (bool isPlayer, int skillType, float calculateCharAmount)
	{
		switch ((CharacterEnums.SkillType)skillType) {

		case CharacterEnums.SkillType.IncreasePlayerHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerHP += calculateCharAmount;
				Debug.Log ("PLAYER HP INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerHP += calculateCharAmount;
				Debug.Log ("ENEMY HP INCREASED BY " + calculateCharAmount);
			}
			break;

		case CharacterEnums.SkillType.DecreaseEnemyHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerHP -= calculateCharAmount;
				Debug.Log ("ENEMY HP DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerHP -= calculateCharAmount;
				Debug.Log ("PLAYER HP DECREASED BY " + calculateCharAmount);
			}
			break;

		case CharacterEnums.SkillType.IncreasePlayerBD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerBD += calculateCharAmount;
				Debug.Log ("PLAYER BASE DAMAGE INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerBD += calculateCharAmount;
				Debug.Log ("ENEMY BASE DAMAGE INCREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.DecreaseEnemyBD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerBD -= calculateCharAmount;
				Debug.Log ("ENEMY BASE DAMAGE DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerBD -= calculateCharAmount;
				Debug.Log ("PLAYER BASE DAMAGE DECREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.IncreasePlayerTD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerTD += calculateCharAmount;
				Debug.Log ("PLAYER TOTAL DAMAGE INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerTD += calculateCharAmount;
				Debug.Log ("ENEMY TOTAL DAMAGE INCREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.DecreaseEnemyTD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerTD -= calculateCharAmount;
				Debug.Log ("ENEMY TOTAL DAMAGE DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerTD -= calculateCharAmount;
				Debug.Log ("PLAYER TOTAL DAMAGE DECREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.IncreasePlayerSD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerSD += calculateCharAmount;
				Debug.Log ("PLAYER SKILL DAMAGE INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerSD += calculateCharAmount;
				Debug.Log ("ENEMY SKILL DAMAGE INCREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.DecreaseEnemySD:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerSD -= calculateCharAmount;
				Debug.Log ("ENEMY SKILL DAMAGE DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerSD -= calculateCharAmount;
				Debug.Log ("PLAYER SKILL DAMAGE DECREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.IncreasePlayerGP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerGP += calculateCharAmount;
				Debug.Log ("PLAYER GP INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerGP += calculateCharAmount;
				Debug.Log ("ENEMY GP INCREASED BY " + calculateCharAmount);
			}

			break;

		case CharacterEnums.SkillType.DecreaseEnemyGP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerGP -= calculateCharAmount;
				Debug.Log ("ENEMY GP DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerGP -= calculateCharAmount;
				Debug.Log ("PLAYER GP DECREASED BY " + calculateCharAmount);
			}

			break;

		}
	}
}
