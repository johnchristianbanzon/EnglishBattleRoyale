using UnityEngine;
using NCalc;

public static class CharacterManager
{
	private static CharacterModel character;
	private static float calculateValue;

	public static void  SetCharacter (int charID)
	{
		character = MyConst.GetCharacter (charID);
	}

	private static void Calculate ()
	{
		//sample

	}

	private static void SetCharacterCalculation ()
	{
		switch (character.characterID) {
		case 1:
			calculateValue = float.Parse (character.characterAmount);
			break;
		case 2:
			calculateValue = float.Parse (character.characterAmount);
			break;
		case 3:
			calculateValue = float.Parse (character.characterAmount);
			break;
		case 4:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 5:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 6:
			calculateValue =float.Parse(character.characterAmount);
			break;
		case 7:
			calculateValue =float.Parse(character.characterAmount);
			break;
		case 8:
			int EnemyHP = 10;
			Expression e8 = new Expression (character.characterAmount);
			e8.Parameters ["EnemyHP"] = EnemyHP;  
			calculateValue = float.Parse(e8.Evaluate ().ToString());
			break;
		case 9:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 10:
			int N = 2;
			Expression e10 = new Expression (character.characterAmount);
			e10.Parameters ["N"] = N;  
			calculateValue = float.Parse(e10.Evaluate ().ToString());
			break;
		case 11:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 12:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 13:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 14:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 15:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 16:
			calculateValue = float.Parse(character.characterAmount);
			break;
		case 17:
			calculateValue = float.Parse(character.characterAmount);
			break;
		}
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
