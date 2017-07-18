using System.Collections.Generic;

/* Player Object*/
public class PlayerModel
{
	public string playerName;

	public float playerHP;

	public float playerGP;

	public float playerMaxGP;

	public float playerBaseDamage;

	public float playerGuardDamage;

	public float playerCriticalDamageRate;

	public PlayerModel (string playerName, float playerHP, float playerGP, float playerMaxGP, float playerBaseDamage, float playerGuardDamage, float playerCriticalDamageRate)
	{
		this.playerName = playerName;
		this.playerHP = playerHP;
		this.playerGP = playerGP;
		this.playerMaxGP = playerMaxGP;
		this.playerBaseDamage = playerBaseDamage;
		this.playerGuardDamage = playerGuardDamage;
		this.playerCriticalDamageRate = playerCriticalDamageRate;
	}

	public PlayerModel (string playerName, float[] playerParam)
	{
		this.playerName = playerName;
		this.playerHP = playerParam[0];
		this.playerGP = playerParam[1];
		this.playerMaxGP = playerParam[2];
		this.playerBaseDamage = playerParam[3];
		this.playerGuardDamage = playerParam[4];
		this.playerCriticalDamageRate = playerParam[5];
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
