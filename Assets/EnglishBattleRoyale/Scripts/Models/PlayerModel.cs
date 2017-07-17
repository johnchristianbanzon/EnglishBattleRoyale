using System.Collections.Generic;

/* Player Object*/
public class PlayerModel
{
	public string playerName;

	public int playerLife;

	public int playerGP;

	public int playerMaxGP;

	public float playerDamage;

	public PlayerModel (string playerName, int playerLife, int playerGP, int playerMaxGP, float playerDamage)
	{
		this.playerName = playerName;
		this.playerLife = playerLife;
		this.playerGP = playerGP;
		this.playerMaxGP = playerMaxGP;
		this.playerDamage = playerDamage;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result ["playerName"] = playerName;
		result ["playerLife"] = playerLife;
		result ["playerGP"] = playerGP;
		result ["playerMaxGP"] = playerMaxGP;
		result ["playerDamage"] = playerDamage;

		return result;
	}
}
