public class GameSettingsModel
{
	public float correctGPBonus;
	public float correctDamageBonus;
	public float speedyAwesomeGPBonus;
	public float speedyAwesomeDamageBonus;
	public float speedyGoodGPBonus;
	public float speedyGoodDamageBonus;

	public GameSettingsModel 
	(
		float[] gameSettingsParam
	)
	{

		this.correctGPBonus = gameSettingsParam[0];
		this.correctDamageBonus = gameSettingsParam[1];
		this.speedyAwesomeGPBonus = gameSettingsParam[2];
		this.speedyAwesomeDamageBonus = gameSettingsParam[3];
		this.speedyGoodGPBonus = gameSettingsParam[4];
		this.speedyGoodDamageBonus = gameSettingsParam[5];
	}

}
