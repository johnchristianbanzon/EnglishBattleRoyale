using UnityEngine;
using NCalc;

public static class CharacterManager
{
	private static CharacterModel character;
	private static float calculateValue;

	public static void  SetCharacter (int charID)
	{
		character = MyConst.GetCharacterByCharID (charID);
	}

	//calculate the skill effect of character from csv
	private static void Calculate ()
	{
		float variable = 0;
		switch (character.characterAmountVariable) {
		case "none":
			variable = 0;
			break;
		case "enemyHP":
			variable = ScreenBattleController.Instance.partState.enemy.playerHP;
			break;
		case "playerHP":
			variable = ScreenBattleController.Instance.partState.player.playerHP;
			break;
		case "enemyDamage":
			variable = ScreenBattleController.Instance.partState.enemy.playerBaseDamage;
			break;
		case "playerDamage":
			variable = ScreenBattleController.Instance.partState.player.playerBaseDamage;
			break;
		case "correctAnswer":
			break;
		}

		//parses the string formula from csv
		Expression e = new Expression (character.characterAmount);
		e.Parameters ["N"] = variable;  
		calculateValue = float.Parse (e.Evaluate ().ToString ());
	}


	private static void Activate ()
	{
		SkillCalcEnum skillCalc = (SkillCalcEnum)character.characterCalculationType;

		switch ((SkillEnum)character.characterSkillID) {
		case SkillEnum.DecreaseEnemyBaseDamage:
			break;
		case SkillEnum.DereaseEnemyHP:
			break;
		case SkillEnum.IncreasePlayerBaseDamage:
			break;
		case SkillEnum.IncreasePlayerGP:
			break;
		case SkillEnum.IncreasePlayerHP:
			break;
		case SkillEnum.LimitEnemySkill:
			break;
		case SkillEnum.Stop:
			break;
		}
	}

}
