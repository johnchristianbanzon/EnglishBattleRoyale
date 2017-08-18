using UnityEngine;
using NCalc;

public class CharacterLogic
{

	public static void CharacterActivate (bool isPlayer, CharacterModel character)
	{
//		PlayerGP
//		EnemyDamage
//		PlayerBD
//		PlayerAwesome
//		EnemyRotten
//		PlayerNerf
//		EnemyHP
//		EnemyGPGain
//		PlayerDamage

		//parses the string formula from csv
		if (isPlayer) {
//			character.characterAmount = character.characterAmount.
//			Replace ("enemyHP", ScreenBattleController.Instance.partState.enemy.playerGP.ToString ()).
//			Replace ("playerHP", ScreenBattleController.Instance.partState.player.playerHP.ToString ()).
//				Replace ("enemyDamage", ScreenBattleController.Instance.partState.enemy.playerTD.ToString ()).
//			Replace ("playerDamage", ScreenBattleController.Instance.partState.player.playerBaseDamage.ToString ()).
//			Replace ("correctAnswer", GameManager.playerAnswerParam.correctCount.ToString ());
		} else {
//			character.characterAmount = character.characterAmount.
//			Replace ("enemyHP", ScreenBattleController.Instance.partState.player.playerGP.ToString ()).
//			Replace ("playerHP", ScreenBattleController.Instance.partState.enemy.playerHP.ToString ()).
//			Replace ("enemyDamage", ScreenBattleController.Instance.partState.player.playerBaseDamage.ToString ()).
//			Replace ("playerDamage", ScreenBattleController.Instance.partState.enemy.playerBaseDamage.ToString ()).
//			Replace ("correctAnswer", GameManager.enemyAnswerParam.correctCount.ToString ());
		}
//		Expression e = new Expression (character.characterAmount);
//		CharacterCompute (isPlayer, character.characterSkillID, float.Parse (e.Evaluate ().ToString ()));
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

		case CharacterEnums.SkillType.Stop:
			if (isPlayer) {
				
			} else {
				
			}

			break;
		}
	}
}
