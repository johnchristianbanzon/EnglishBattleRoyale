using System.Collections.Generic;

/* Player Object*/
public class PlayerModel
{
	public string playerName;

	public float playerHP;

	public float playerGP;

	public float playerMaxGP;

	public float playerBD;

	//player has skill damage multiplier
	public float playerSDM;

	public float playerTD;

	//player has skill damage buff
	public bool playerSDB;



	public PlayerModel (string playerName, float[] playerParam, bool playerSDB)
	{
		this.playerName = playerName;
		this.playerHP = playerParam [0];
		this.playerGP = playerParam [1];
		this.playerMaxGP = playerParam [2];
		this.playerBD = playerParam [3];
		this.playerSDM = playerParam [4];
		this.playerTD = playerParam [5];
		this.playerSDB = playerSDB;
	}
}
