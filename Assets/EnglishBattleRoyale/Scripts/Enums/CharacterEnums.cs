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
		PlayerHP,
		EnemyHP,
		PlayerBD,
		EnemyBD,
		PlayerTD,
		EnemyTD,
		PlayerSD,
		EnemySD,
		PlayerGP,
		EnemyGP,
		Custom
	}

	public enum SkillOperator{
		None,
		Add,
		Multiply
	}
}
