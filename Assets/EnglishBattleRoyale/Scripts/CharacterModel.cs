using System.Collections.Generic;
using System;

[Serializable]
public class CharacterModel
{
	public int iD;
	public string name;
	public string effectDescription;
	public string conditionDescription;
	public string turnDescription;
	public int gpCost;
	public int particleID;
	public string skillCalculation;

	public CharacterModel (
		int iD,
		string name,
		string effectDescription,
		string conditionDescription,
		string turnDescription,
		int gpCost,
		int particleID,
		string skillCalculation
		)
	{
		this.iD = iD;
		this.name = name;
		this.effectDescription = effectDescription;
		this.conditionDescription = conditionDescription;
		this.turnDescription = turnDescription;
		this.gpCost = gpCost;
		this.particleID = particleID;
		this.skillCalculation = skillCalculation;
	}

}
//make list class for charactermodel and serializable so that it can be wrapped and send to firebase
[Serializable]
public class CharacterModelList
{
	public List<CharacterModel> list;
}