using UnityEngine;
using NCalc;

public class CharacterLogic{

	public static void CharacterActivate (bool isPlayer, CharacterModel character)
	{

		float variable = 0;
		switch (character.characterAmountVariable) {
		case "none":
			variable = 0;
			break;
		case "enemyHP":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.enemy.playerHP;
			} else {
				variable = ScreenBattleController.Instance.partState.player.playerHP;
			}


			break;
		case "playerHP":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.player.playerHP;
			} else {
				variable = ScreenBattleController.Instance.partState.enemy.playerHP;
			}


			break;
		case "enemyDamage":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.enemy.playerBaseDamage;
			} else {
				variable = ScreenBattleController.Instance.partState.player.playerBaseDamage;
			}


			break;
		case "playerDamage":
			if (isPlayer) {
				variable = ScreenBattleController.Instance.partState.player.playerBaseDamage;
			} else {
				variable = ScreenBattleController.Instance.partState.enemy.playerBaseDamage;
			}


			break;
		case "correctAnswer":
			if (isPlayer) {

			}
			break;
		}

		//parses the string formula from csv

		Expression e = new Expression (character.characterAmount);
		e.Parameters ["N"] = variable;  
		//		string amountStr = "10 + enemyHP";
		//		int enemyHP = 10;
		//		amountStr.Replace ("enemyHP", enemyHP).Replace ("playerHP", playerHP);



		CharacterCompute (isPlayer, character.characterSkillID, float.Parse (e.Evaluate ().ToString ()));
	}

	//activates the character and calculate the respective skills
	private static void CharacterCompute (bool isPlayer, int skillID, float calculateCharAmount)
	{
		switch ((SkillEnum)skillID) {

		case SkillEnum.DecreaseEnemyBaseDamage:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerBaseDamage -= calculateCharAmount;
				Debug.Log ("ENEMY BASE DAMAGE DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerBaseDamage -= calculateCharAmount;
				Debug.Log ("PLAYER BASE DAMAGE DECREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.DereaseEnemyHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.enemy.playerHP -= calculateCharAmount;
				Debug.Log ("ENEMY HP DECREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.player.playerHP -= calculateCharAmount;
				Debug.Log ("PLAYER HP DECREASED BY " + calculateCharAmount);
			}


			break;
		case SkillEnum.IncreasePlayerBaseDamage:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerBaseDamage += calculateCharAmount;
				Debug.Log ("PLAYER BASE DAMAGE INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerBaseDamage += calculateCharAmount;
				Debug.Log ("ENEMY BASE DAMAGE INCREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.IncreasePlayerGP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerGP += calculateCharAmount;
				Debug.Log ("PLAYER GP INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerGP += calculateCharAmount;
				Debug.Log ("ENEMY GP INCREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.IncreasePlayerHP:
			if (isPlayer) {
				ScreenBattleController.Instance.partState.player.playerHP += calculateCharAmount;
				Debug.Log ("PLAYER HP INCREASED BY " + calculateCharAmount);
			} else {
				ScreenBattleController.Instance.partState.enemy.playerHP += calculateCharAmount;
				Debug.Log ("ENEMY HP INCREASED BY " + calculateCharAmount);
			}

			break;
		case SkillEnum.LimitEnemySkill:
			Debug.Log ("CHARACTER EFFECT NOT IMPLEMENTED");
			break;
		case SkillEnum.Stop:
			Debug.Log ("CHARACTER EFFECT NOT IMPLEMENTED");
			break;
		default:
			break;
		}
	}
}
