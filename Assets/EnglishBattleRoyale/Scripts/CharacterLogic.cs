using UnityEngine;
using NCalc;

public class CharacterLogic
{

	public static void CharacterActivate (bool isPlayer, CharacterModel character)
	{
		//parses the string formula from csv
		if (isPlayer) {
			character.characterAmount.
			Replace ("enemyHP", ScreenBattleController.Instance.partState.enemy.playerGP.ToString ()).
			Replace ("playerHP", ScreenBattleController.Instance.partState.player.playerHP.ToString ()).
			Replace ("enemyDamage", ScreenBattleController.Instance.partState.enemy.playerBaseDamage.ToString ()).
			Replace ("playerDamage", ScreenBattleController.Instance.partState.player.playerBaseDamage.ToString ()).
			Replace ("correctAnswer", GameManager.playerAnswerParam.correctCount.ToString ());
		} else {
			character.characterAmount.
			Replace ("enemyHP", ScreenBattleController.Instance.partState.player.playerGP.ToString ()).
			Replace ("playerHP", ScreenBattleController.Instance.partState.enemy.playerHP.ToString ()).
			Replace ("enemyDamage", ScreenBattleController.Instance.partState.player.playerBaseDamage.ToString ()).
			Replace ("playerDamage", ScreenBattleController.Instance.partState.enemy.playerBaseDamage.ToString ()).
			Replace ("correctAnswer", GameManager.enemyAnswerParam.correctCount.ToString ());
		}
		Expression e = new Expression (character.characterAmount);

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
