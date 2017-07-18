using System.Collections.Generic;

/* Player Object*/
public class PlayerModel
{
	public string playerName;

	public int playerHP;

	public int playerGP;

	public int playerMaxGP;

	public float playerBaseDamage;

	public float playerGuardDamage;

	public float playerCriticalDamageRate;

	public PlayerModel (string playerName, int playerHP, int playerGP, int playerMaxGP, float playerBaseDamage, float playerGuardDamage, float playerCriticalDamageRate)
	{
		this.playerName = playerName;
		this.playerHP = playerHP;
		this.playerGP = playerGP;
		this.playerMaxGP = playerMaxGP;
		this.playerBaseDamage = playerBaseDamage;
		this.playerGuardDamage = playerGuardDamage;
		this.playerCriticalDamageRate = playerCriticalDamageRate;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result ["playerName"] = playerName;
		result ["playerHP"] = playerHP;
		result ["playerGP"] = playerGP;
		result ["playerMaxGP"] = playerMaxGP;
		result ["playerBaseDamage"] = playerBaseDamage;
		result ["playerGuardDamage"] = playerGuardDamage;
		result ["playerCriticalDamageRate"] = playerCriticalDamageRate;

		return result;
	}
}
