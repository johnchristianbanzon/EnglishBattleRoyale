public class CharacterEnums{

	public enum CharacterType{
		Damage,
		Heal,
		Buff,
		Nerf
	}

	public enum SacrificeType{
		None,
		HP,
		Gold,
		Gem,
		Equipment,
		Turn,
		Skill,
		BD
	}

	public enum SkillType{
		IncreasePlayerHP,
		DecreaseEnemyHP,
		IncreasePlayerBD,
		DecreaseEnemyBD,
		IncreasePlayerTD,
		DecreaseEnemyTD,
		IncreasePlayerSD,
		DecreaseEnemySD,
		IncreasePlayerGP,
		DecreaseEnemyGP,
		Stop
	}
}
