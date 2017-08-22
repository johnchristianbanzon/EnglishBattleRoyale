using System.Collections.Generic;

/* Player Object*/
public class PlayerModel
{
	public string playerName;

	public float playerHP;

	public float playerGP;

	public float playerMaxGP;

	public float playerBD;

	public float playerSD;

	public float playerTD;


	public PlayerModel (string playerName, float[] playerParam)
	{
		this.playerName = playerName;
		this.playerHP = playerParam [0];
		this.playerGP = playerParam [1];
		this.playerMaxGP = playerParam [2];
		this.playerBD = playerParam [3];
		this.playerSD = playerParam [4];
		this.playerTD = playerParam [5];
	}
}
