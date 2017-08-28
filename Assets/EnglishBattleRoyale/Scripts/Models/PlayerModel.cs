using System.Collections.Generic;

/* Player Object*/
public class PlayerModel
{
	public string name;

	public float hp;

	public float gp;

	public float maxGP;

	public float bd;

	//player has skill damage multiplier
	public float sdm;

	public float td;

	//player has skill damage buff
	public bool sdb;




	public PlayerModel (string name, float[] playerParam, bool sdb)
	{
		this.name = name;
		hp = playerParam [0];
		gp = playerParam [1];
		maxGP = playerParam [2];
		bd = playerParam [3];
		sdm = playerParam [4];
		td = playerParam [5];
		this.sdb = sdb;
	}
}
