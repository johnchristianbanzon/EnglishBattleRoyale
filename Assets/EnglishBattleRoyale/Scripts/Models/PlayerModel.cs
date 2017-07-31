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

	public PlayerModel (string playerName, float[] playerParam)
	{
		this.playerName = playerName;
		this.playerHP = playerParam [0];
		this.playerGP = playerParam [1];
		this.playerMaxGP = playerParam [2];
		this.playerBaseDamage = playerParam [3];
		this.playerGuardDamage = playerParam [4];
		this.playerCriticalDamageRate = playerParam [5];
	}
		
}
